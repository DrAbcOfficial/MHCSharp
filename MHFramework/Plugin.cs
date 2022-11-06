using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.OpenGL;
using System.Drawing;
using MHFramework.SDK;
using MHFramework.Util;

namespace MHFramework;

public class Plugin
{
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe void Init(MetaHookApiStruct* Api, void* Interface, MHEngineSaveStrct* Save)
    {
        OpenGL.InitOpenGL();
        MyExportFuncs.Init();
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe void LoadClient(ClExportFuncsStruct* ExportFunc)
    {
        MyExportFuncs.ExportFuncs = *ExportFunc;
        // 必须覆盖，在这个事件中驱动单线程协程的调度器
        ExportFunc->HudFrame = &MyExportFuncs.Frame;
        ExportFunc->HudReDraw = &MyExportFuncs.Hud_Redraw;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void ShutDown()
    {
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void LoadEngine()
    {

    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void ExitGame(int Result)
    {

    }



}
