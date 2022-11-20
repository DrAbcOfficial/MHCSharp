using MHSharpLibrary.SDK;
using MHSharpLibrary.Util;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MHSharpFrame;

public class FrameExportFuncs
{
    // 单线程同步上下文
    static GameSynchronizationContext? ctx;
    // 保存好的原版函数
    public static ClExportFuncsStruct IExportFuncs;

    public static void Init()
    {
        // 协程初始化
        ctx = new GameSynchronizationContext() { CurrentThread = Thread.CurrentThread };
        SynchronizationContext.SetSynchronizationContext(ctx);
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public unsafe static void HudFrame(double time)
    {
        // 不要删除，这是单线程协程的调度器
        ctx?.Update();
        IExportFuncs.HudFrame(time);

    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public unsafe static void ListPlugins()
    {
        string message = string.Format("|{0,5}|{1,32}|{2,32}|\n", "index", "plugin name", "plugin version");

        for (int i = 0; i < Plugin.sharpPlugins.Count; i++)
        {
            string szVersion = string.Empty;
            if (Plugin.sharpPlugins[i].GetVersion != null)
            {
                object[] args = { };
                string? temp = (string?)Plugin.sharpPlugins[i].GetVersion?.Invoke(Plugin.sharpPlugins[i].Handle, args);
                szVersion = temp == null ? string.Empty : temp;
            }
            message += string.Format("|{0,5}|{1,32}|{2,32}|\n", i, Plugin.sharpPlugins[i].Name, szVersion);
        }
        MHUtility.SendNativeString(message, (IntPtr ptr) =>
        {
            Plugin.IEngineFucs.ConsolePrint((byte*)ptr.ToPointer());
        });
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public unsafe static void HudInit()
    {
        Plugin.IEngineFucs.AddCommand(MHUtility.GetNativeString("mh_cspluginlist"), &ListPlugins); 
        Plugin.IHudEvent.OnHudInit();

        IExportFuncs.HudInit();
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public unsafe static int HudReDraw(float flTime, int iFrame)
    {
        int? result = Plugin.IHudEvent.OnHudReDraw(flTime, iFrame);
        if (result != null && result != 0)
            return (int)result;
        return IExportFuncs.HudReDraw(flTime, iFrame);
    }
}
