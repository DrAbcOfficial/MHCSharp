using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MHSharpLibrary.SDK;
using MHSharpLibrary.Util;
using System.IO;

namespace MHHelloWorld;

public class Plugin
{
    public static MetaHookApiStruct MetaHookApi;
    public static CLEngineFucsStruct IEngineFucs;

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe void Init(MetaHookApiStruct* Api, void* Interface, MHEngineSaveStrct* Save)
    {
        MetaHookApi = *Api;
        MyExportFuncs.Init();
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe void LoadEngine(CLEngineFucsStruct* lEngineFucs)
    {
        IEngineFucs = *lEngineFucs;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe void LoadClient(ClExportFuncsStruct* IExportFunc)
    {
        MyExportFuncs.IExportFuncs = *IExportFunc;
        // 必须覆盖，在这个事件中驱动单线程协程的调度器
        //TODO: Move his to one dll, and call other dll from that one
        IExportFunc->HudFrame = &MyExportFuncs.HudFrame;

        IExportFunc->HudInit = &MyExportFuncs.HudInit;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void ShutDown()
    {
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void ExitGame(int Result)
    {

    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe byte* GetVersion()
    {
        string version = "This from C#! WOW!";
        return Utility.GetNativeString(version);
    }
}
