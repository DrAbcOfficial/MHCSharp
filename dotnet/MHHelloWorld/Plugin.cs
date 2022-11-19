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
using MHSharpLibrary;

namespace MHHelloWorld;

public class CSharpPlugin
{
    public static MetaHookApiStruct MetaHookApi;
    public static CLEngineFucsStruct IEngineFucs;

    public unsafe void PluginInit(MetaHookApiStruct* Api, void* Interface, MHEngineSaveStrct* Save)
    {
        MetaHookApi = *Api;
    }

    public unsafe void LoadEngine(CLEngineFucsStruct* lEngineFucs)
    {
        IEngineFucs = *lEngineFucs;
    }

    public unsafe void LoadClient(ClExportFuncsStruct* IExportFunc)
    {
        IExportFunc->HudInit = &MyExportFuncs.HudInit;
    }

    public void ShutDown()
    {
        
    }

    public void ExitGame(int Result)
    {
        
    }

    public string GetVersion()
    {
        return "2022-11-20";
    }
}
