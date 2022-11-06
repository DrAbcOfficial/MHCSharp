using Flurl.Http;
using MHFramework.SDK;
using MHFramework.Util;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MHFramework;

public class MyExportFuncs
{
    // 单线程同步上下文
    static GameSynchronizationContext? ctx;
    // 保存好的原版函数
    public static ClExportFuncsStruct ExportFuncs;

    static GL gl = OpenGL.gl;
    public static void Init()
    {
        // 协程初始化
        ctx = new GameSynchronizationContext() { CurrentThread = Thread.CurrentThread };
        SynchronizationContext.SetSynchronizationContext(ctx);


    }


    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public unsafe static void Frame(double time)
    {
        // 不要删除，这是单线程协程的调度器
        ctx?.Update();
        ExportFuncs.HudFrame(time);

    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe int Hud_Redraw(float time, int xx)
    {
        return ExportFuncs.HudReDraw(time, xx);

    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe int Initialize(CLEngineFucsStruct* engineFuncs, int none)
    {
        CLEngineFucs.FunSet = *engineFuncs;
        // 测试在控制台打印
        _ = TestHttp();
        return ExportFuncs.Initialize(engineFuncs, none);
    }

    public static async Task TestHttp()
    {
        var html = await "http://www.baidu.com".GetAsync().ReceiveString();
        CLEngineFucs.ConsolePrintLine(html);
    }
}
