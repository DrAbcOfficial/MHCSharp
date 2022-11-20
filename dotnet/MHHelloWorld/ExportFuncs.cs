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
    // 保存好的原版函数
    public static ClExportFuncsStruct IExportFuncs;

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public unsafe static void FuckWorld()
    {
        string message = "Fuck World\n";
        byte* cplain = Utility.GetNativeString(message);
        CSharpPlugin.IEngineFucs.ConsolePrint(cplain);
    }

    public unsafe static void HudInit()
    {
        string message = "mh_fuck";
        byte* cplain = Utility.GetNativeString(message);
        CSharpPlugin.IEngineFucs.AddCommand(cplain, &FuckWorld);
        IExportFuncs.HudInit();
    }
}
