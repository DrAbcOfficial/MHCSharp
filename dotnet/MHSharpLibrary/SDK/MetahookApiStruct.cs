using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSharpLibrary.SDK;

public unsafe struct HookStruct
{
    public void* OldFuncAddr = null;
    public void* NewFuncAddr = null;
    public void* Class = null;
    public int TableIndex = 0;
    public int FuncIndex = 0;
    public byte* ModuleName = null;
    public byte* FuncName = null;
    public HookStruct* Next = null;
    public void* Info = null;

    public HookStruct()
    {
    }
}

public unsafe struct MetaHookApiStruct
{
    public delegate* unmanaged[Cdecl]<HookStruct*, bool> UnHook = null;
    public delegate* unmanaged[Cdecl]<void*, void*, void*, void*> InlineHook = null;
    public delegate* unmanaged[Cdecl]<void*, int, int, void*, void*, HookStruct*> VFTHook = null;
    public delegate* unmanaged[Cdecl]<HModuleStruct, byte*, byte*, void*, void*, HookStruct*> IATHook = null;
    public delegate* unmanaged[Cdecl]<void*> GetClassFuncAddr = null;           // 不会抄
    public delegate* unmanaged[Cdecl]<HModuleStruct, uint> GetModuleBase = null;
    public delegate* unmanaged[Cdecl]<HModuleStruct, uint> GetModleSize = null;
    public delegate* unmanaged[Cdecl]<HModuleStruct> GetEngineModule = null;
    public delegate* unmanaged[Cdecl]<void*> GetEngineBase = null;
    public delegate* unmanaged[Cdecl]<uint> GetEngineSize = null;
    public delegate* unmanaged[Cdecl]<void*, uint, byte*, uint, void*> SearchPattern = null;
    public delegate* unmanaged[Cdecl]<void*, uint, void> WriteDWord = null;
    public delegate* unmanaged[Cdecl]<void*, uint> ReadDWord = null;
    public delegate* unmanaged[Cdecl]<void*, byte*, uint, uint> WriteMemory = null;
    public delegate* unmanaged[Cdecl]<void*, byte*, uint, uint> ReadMemory = null;
    public delegate* unmanaged[Cdecl]<int*, int*, int*, bool*, uint> GetVideoMode = null;
    public delegate* unmanaged[Cdecl]<uint> GetEngineBuildnum = null;
    public IntPtr GetEngineFactory = 0;             // 不会抄
    public delegate* unmanaged[Cdecl]<void*, uint, uint> GetNextCallAddr = null;
    public delegate* unmanaged[Cdecl]<void*, byte, void> WriteByte = null;
    public delegate* unmanaged[Cdecl]<void*, byte> ReadByte = null;
    public delegate* unmanaged[Cdecl]<void*, uint, void> WriteNOP = null;

    public MetaHookApiStruct()
    {
    }
}

public struct HModuleStruct
{
    public int UnUsed = 0;

    public HModuleStruct()
    {
    }
}

