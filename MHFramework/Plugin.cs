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
    public static MetaHookApiStruct MetaHookApi;



    static unsafe delegate* unmanaged[Cdecl]<byte*, int, int, int, byte*, int, int, byte*, int> LoadTexturePtr = null;

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe void Init(MetaHookApiStruct* Api, void* Interface, MHEngineSaveStrct* Save)
    {
        MetaHookApi = *Api;
        MyExportFuncs.Init();
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe void LoadClient(ClExportFuncsStruct* ExportFunc)
    {
        MyExportFuncs.ExportFuncs = *ExportFunc;
        // 必须覆盖，在这个事件中驱动单线程协程的调度器
        ExportFunc->HudFrame = &MyExportFuncs.Frame;
        ExportFunc->HudReDraw = &MyExportFuncs.Hud_Redraw;
        ExportFunc->Initialize = &MyExportFuncs.Initialize;

        var GL_LOADTEXTURE_SIG = new byte[] { (byte)'\xA1',(byte)'\x2A',(byte)'\x2A',(byte)'\x2A',(byte)'\x2A',(byte)'\x8B',(byte)'\x4C',(byte)'\x24',(byte)'\x20',(byte)'\x8B',(byte)'\x54',(byte)'\x24',(byte)'\x1C',(byte)'\x50',(byte)'\x8B',(byte)'\x44',(byte)'\x24',(byte)'\x1C',(byte)'\x51',(byte)'\x8B',(byte)'\x4C',(byte)'\x24',(byte)'\x1C',(byte)'\x52',(byte)'\x8B',(byte)'\x54',(byte)'\x24',(byte)'\x1C',(byte)'\x50',(byte)'\x8B',(byte)'\x44',(byte)'\x24',(byte)'\x1C' };
        fixed(byte* SizePtr = GL_LOADTEXTURE_SIG)
        {
            var ptr = MetaHookApi.SearchPattern(MetaHookApi.GetEngineBase(), MetaHookApi.GetEngineSize(), SizePtr, (uint)GL_LOADTEXTURE_SIG.Length);
            Console.WriteLine(1);
            LoadTexturePtr = (delegate* unmanaged[Cdecl]<byte*, int, int, int, byte*, int, int, byte*, int>)ptr;
            delegate* unmanaged[Cdecl]<byte*, int, int, int, byte*, int, int, byte*, int> LoadTexturePtr2 = &LoadTexture;
            if (LoadTexturePtr != null)
            {
                fixed(void* pPtr = &LoadTexturePtr)
                {
                    var f = MetaHookApi.InlineHook(LoadTexturePtr, LoadTexturePtr2, pPtr);
                }
            }
        }
    }


    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe int LoadTexture(byte* identifier, int textureType, int width, int height, byte* data, int mipmap, int iType, byte* pPal)
    {
        return LoadTexturePtr(identifier, textureType, width, height, data, mipmap, iType, pPal);

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
