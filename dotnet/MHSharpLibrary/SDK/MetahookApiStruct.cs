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
    public delegate* unmanaged[Cdecl]<void*, void*, void*, HookStruct*> InlineHook = null;
    public delegate* unmanaged[Cdecl]<void*, int, int, void*, void*, HookStruct*> VFTHook = null;
    public delegate* unmanaged[Cdecl]<HModuleStruct, byte*, byte*, void*, void*, HookStruct*> IATHook = null;
    public delegate* unmanaged[Cdecl]<void*> GetClassFuncAddr = null;
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
    public delegate* unmanaged[Cdecl]<int> GetEngineType = null;
    public delegate* unmanaged[Cdecl]<byte*> GetEngineTypeName = null;
    public delegate* unmanaged[Cdecl]<void*, uint, uint> ReverseSearchFunctionBegin = null;
    public delegate* unmanaged[Cdecl]<void*, byte*, ulong, uint> GetSectionByName = null;
    public delegate* unmanaged[Cdecl]<void*, void*, void*, int> DisasmSingleInstruction = null;
    public delegate* unmanaged[Cdecl]<void*, ulong, void*, int, void*, int> DisasmRanges = null;
    public delegate* unmanaged[Cdecl]<void*, int, byte*, int, void> ReverseSearchPattern = null;
    public delegate* unmanaged[Cdecl]<HModuleStruct> GetClientModule = null;
    public delegate* unmanaged[Cdecl]<void*> GetClientBase = null;
    public delegate* unmanaged[Cdecl]<uint> GetClientSize = null;
    public IntPtr GetClientFactory = 0;             // 不会抄
    public delegate* unmanaged[Cdecl]<int, MHPluginInfo*, int> QueryPluginInfo = null;
    public delegate* unmanaged[Cdecl]<byte*, MHPluginInfo*, int> GetPluginInfo = null;
    public IntPtr HookUserMsg = 0;             // 不会抄
    public IntPtr HookCvarCallback = 0;             // 不会抄
    public IntPtr HookCmd = 0;             // 不会抄
    public delegate* unmanaged[Cdecl]<byte*, void> SysError = null;             // 不会抄
    public delegate* unmanaged[Cdecl]<void*, int, IntPtr,void*> ReverseSearchFunctionBeginEx = null;
    public MetaHookApiStruct()
    {
    }
}

public unsafe struct MHPluginInfo
{
    int Index;
    byte* PluginName;
    byte* PluginPath;
    byte* PluginVersion;
    int InterfaceVersion;
    void* PluginModuleBase;
    uint PluginModuleSize;
}

public struct HModuleStruct
{
    public int UnUsed = 0;

    public HModuleStruct()
    {
    }
}

