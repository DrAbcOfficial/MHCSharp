using MHSharpLibrary.Util;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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

    public unsafe static int HudReDraw(float flTime, int iFrame)
    {
        MHUtility.SendNativeString("fuck world", (IntPtr ptr) =>
        {
            CSharpPlugin.IEngineFucs.DrawString(512, 512, (byte*)ptr.ToPointer(), 0, 255, 0);
        });
        return 0;
    }
}
