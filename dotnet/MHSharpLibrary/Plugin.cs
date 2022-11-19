using MHSharpLibrary.SDK;
using MHSharpLibrary.Util;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MHSharpLibrary;

public class CSharpPlugin
{
    public string? Name;
    //Object
    public object? Handle = null;
    //MetaHookApiStruct* Api, void* Interface, MHEngineSaveStrct* Save
    public MethodInfo? PluginInit = null;
    //CLEngineFucsStruct* lEngineFucs
    public MethodInfo? LoadEngine = null;
    //ClExportFuncsStruct* IExportFunc
    public MethodInfo? LoadClient = null;
    //无参
    public MethodInfo? ShutDown = null;
    //int Result
    public MethodInfo? ExitGame = null;
    //无参
    public MethodInfo? GetVersion = null;
}

public class Plugin
{
    public static List<CSharpPlugin> sharpPlugins = new List<CSharpPlugin>();
    public static CLEngineFucsStruct IEngineFucs;

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe void Init(MetaHookApiStruct* Api, void* Interface, MHEngineSaveStrct* Save)
    {
        MyExportFuncs.Init();
        //Load every Dll
        List<string> plugins = new List<string>();
        using (StreamReader sr = new StreamReader(string.Format("{0}/metahook/configs/plugins_dotnet.lst", "svencoop")))
        {
            string? line = sr.ReadLine();
            if (line != null)
            {
                line = line.Trim().Trim('\n');
                plugins.Add(line);
            }
        }
        foreach (string s in plugins)
        {
            string path = string.Format("{0}/metahook/plugins/dotnet/{1}/{1}.dll", "svencoop", s);
            Assembly assembly = Assembly.LoadFrom(Path.GetFullPath(path));
            foreach (Type t in assembly.GetTypes())
            {
                if (t.Name == "CSharpPlugin" && t.Namespace == s)
                {
                    CSharpPlugin plugin = new CSharpPlugin();
                    plugin.Name = s;
                    plugin.Handle = Activator.CreateInstance(t);
                    plugin.PluginInit = t.GetMethod("PluginInit");
                    plugin.LoadEngine = t.GetMethod("LoadEngine");
                    plugin.ShutDown = t.GetMethod("ShutDown");
                    plugin.ExitGame = t.GetMethod("ExitGame");
                    plugin.GetVersion = t.GetMethod("GetVersion");
                    sharpPlugins.Add(plugin);
                }
            }
        }
        foreach (CSharpPlugin i in sharpPlugins)
        {
            if (i.PluginInit != null)
            {
                object[] parames = { (IntPtr)Api, (IntPtr)Interface, (IntPtr)Save };
                i.PluginInit.Invoke(i.Handle, parames);
            }
        }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe void LoadEngine(CLEngineFucsStruct* lEngineFucs)
    {
        IEngineFucs = *lEngineFucs;
        foreach (CSharpPlugin i in sharpPlugins)
        {
            if (i.LoadEngine != null)
            {
                object[] parames = { (IntPtr)lEngineFucs };
                i.LoadEngine.Invoke(i.Handle, parames);
            }
        }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe void LoadClient(ClExportFuncsStruct* IExportFunc)
    {
        MyExportFuncs.IExportFuncs = *IExportFunc;
        // 必须覆盖，在这个事件中驱动单线程协程的调度器
        IExportFunc->HudFrame = &MyExportFuncs.HudFrame;
        //注册
        IExportFunc->HudInit = &MyExportFuncs.HudInit;
        foreach (CSharpPlugin i in sharpPlugins)
        {
            if (i.LoadClient != null)
            {
                object[] parames = { (IntPtr)IExportFunc };
                i.LoadClient.Invoke(i.Handle, parames);
            }
        }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void ShutDown()
    {
        foreach (CSharpPlugin i in sharpPlugins)
        {
            if (i.ShutDown != null)
            {
                object[] parames = { };
                i.ShutDown.Invoke(i.Handle, parames);
            }
        }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void ExitGame(int Result)
    {
        foreach (CSharpPlugin i in sharpPlugins)
        {
            if (i.ExitGame != null)
            {
                object[] parames = { Result };
                i.ExitGame.Invoke(i.Handle, parames);
            }
        }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe byte* GetVersion()
    {
        string version = Assembly.GetExecutingAssembly().GetName().ToString();
        return Utility.GetNativeString(version);
    }
}
