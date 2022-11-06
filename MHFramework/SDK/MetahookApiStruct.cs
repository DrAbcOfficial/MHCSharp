using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHFramework.SDK;

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
    public delegate* unmanaged[Cdecl]<void*, void*, void*, HookStruct*> InlineHook = null;
    public delegate* unmanaged[Cdecl]<void*, int, int, void*, void*, HookStruct*> VFTHook = null;
    public delegate* unmanaged[Cdecl]<HModuleStruct, byte*, byte*, void*, void*, HookStruct*> IATHook = null;
    public delegate* unmanaged[Cdecl]<void*> GetClassFuncAddr = null;           // 不会抄
    public delegate* unmanaged[Cdecl]<HModuleStruct, ulong> GetModuleBase = null;
    public delegate* unmanaged[Cdecl]<HModuleStruct, ulong> GetModleSize = null;
    public delegate* unmanaged[Cdecl]<HModuleStruct> GetEngineModule = null;
    public delegate* unmanaged[Cdecl]<void*, ulong, byte*, ulong, void*> SearchPattern = null;
    public delegate* unmanaged[Cdecl]<void*, ulong, void> WriteDWord = null;
    public delegate* unmanaged[Cdecl]<void*, ulong> ReadDWord = null;
    public delegate* unmanaged[Cdecl]<void*, byte*, ulong, ulong> WriteMemory = null;
    public delegate* unmanaged[Cdecl]<void*, byte*, ulong, ulong> ReadMemory = null;
    public delegate* unmanaged[Cdecl]<int*, int*, int*, bool*, ulong> GetVideoMode = null;
    public delegate* unmanaged[Cdecl]<ulong> GetEngineBuildnum = null;
    public IntPtr GetEngineFactory = 0;             // 不会抄
    public delegate* unmanaged[Cdecl]<void*, ulong, ulong> GetNextCallAddr = null;
    public delegate* unmanaged[Cdecl]<void*, byte, void> WriteByte = null;
    public delegate* unmanaged[Cdecl]<void*, byte> ReadByte = null;
    public delegate* unmanaged[Cdecl]<void*, ulong, void> WriteNOP = null;

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

