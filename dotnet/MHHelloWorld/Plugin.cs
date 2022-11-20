using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MHSharpLibrary;
using MHSharpLibrary.Event;
using MHSharpLibrary.SDK;
using MHSharpLibrary.Util;
using System.IO;

namespace MHHelloWorld;

public class CSharpPlugin
{
    public static MetaHookApiStruct MetaHookApi;
    public static CLEngineFucsStruct IEngineFucs;

    public unsafe void PluginInit(MetaHookApiStruct* Api, IntPtr Interface, MHEngineSaveStrct* Save)
    {
        MetaHookApi = *Api;
    }

    public unsafe void LoadEngine(CLEngineFucsStruct* lEngineFucs)
    {
        IEngineFucs = *lEngineFucs;
    }

    public unsafe void LoadClient(ExportEventsPublisherStuct* IExportFunc)
    {
        //订阅
        IExportFunc->HudInit += MyExportFuncs.HudInit;
    }

    public string GetVersion()
    {
        return "2022-11-20";
    }
}
