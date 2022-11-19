using MHSharpLibrary.SDK;
using MHSharpLibrary.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MHHelloWorld;

public class MyExportFuncs
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
    public unsafe static void FuckWorld()
    {
        string message = "Fuck World\n";
        byte* cplain = Utility.GetNativeString(message);
        Plugin.IEngineFucs.ConsolePrint(cplain);
    }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public unsafe static void HudInit()
    {
        string message = "mh_fuck";
        byte* cplain = Utility.GetNativeString(message);
        Plugin.IEngineFucs.AddCommand(cplain, &FuckWorld);
        IExportFuncs.HudInit();
    }
}
