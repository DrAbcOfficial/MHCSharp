using MHSharpLibrary.SDK;
using MHSharpLibrary.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MHHelloWorld;

public class MyExportFuncs
{
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public unsafe static void FuckWorld()
    {
        MHUtility.SendNativeString("Fuck World\n", (IntPtr ptr) =>
        {
            CSharpPlugin.IEngineFucs.ConsolePrint((byte*)ptr.ToPointer());
        });    
    }

    public unsafe static void HudInit()
    {
        CSharpPlugin.IEngineFucs.AddCommand(MHUtility.GetNativeString("mh_fuck"), &FuckWorld);
    }
}
