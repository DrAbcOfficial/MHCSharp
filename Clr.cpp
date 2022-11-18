#include <cstdio>
#include <memory>
#include <string>
#include <array>
#include <vector>
#include <algorithm>

#include <nethost.h>
#include <coreclr_delegates.h>
#include <hostfxr.h>
#include <metahook.h>
#include <Windows.h>

extern cl_exportfuncs_t gExportfuncs;

hostfxr_handle ClrHandle = 0;
hostfxr_close_fn CloseFunPtr = nullptr;
void CSharpInit(metahook_api_t* pAPI, mh_interface_t* pInterface, mh_enginesave_t* pSave) {

};
void CSharpLoadClient(cl_exportfuncs_s* pExportFunc) {

};
void CSharpShutDown() {

};
void CSharpLoadEngine() {

};
void CSharpExitGame(int iResult) {

};
void InitClr(){
    // 加载hostfxr.dll
    HMODULE ClrDllHandle = LoadLibraryW(L"dotnet/host/fxr/7.0.0-rc.2.22472.3/hostfxr.dll");
    if (!ClrDllHandle) {
        g_pMetaHookAPI->SysError("Can not find .NET 7 runtime!\nDownload .NET runtime:\nhttps://dotnet.microsoft.com/en-us/download/dotnet/7.0");
        return;
    }
    // 获取函数
    auto InitializeFunPtr = (hostfxr_initialize_for_runtime_config_fn)::GetProcAddress(ClrDllHandle, "hostfxr_initialize_for_runtime_config");
    auto GetDelegateFunPtr = (hostfxr_get_runtime_delegate_fn)::GetProcAddress(ClrDllHandle, "hostfxr_get_runtime_delegate");
    CloseFunPtr = (hostfxr_close_fn)::GetProcAddress(ClrDllHandle, "hostfxr_close");
    if (InitializeFunPtr == nullptr || GetDelegateFunPtr == nullptr || CloseFunPtr == nullptr){
        g_pMetaHookAPI->SysError("Can not find .NET 7 function ptr!");
        return;
    }

    // 初始化clr
    if (InitializeFunPtr(L"dotnet/dotnet.runtimeconfig.json", nullptr, &ClrHandle)){

        g_pMetaHookAPI->SysError("Can not find .NET 7 config!\ndotnet/dotnet.runtimeconfig.json");
        CloseFunPtr(ClrHandle);
        return;
    }

    // 获取一个叫做 “加载汇编并获得函数指针”的函数
    void* funptr;
    GetDelegateFunPtr(ClrHandle, hdt_load_assembly_and_get_function_pointer, &funptr);
    // 获取成功以后就能把句柄关了，我也不知道因为什么
    CloseFunPtr(ClrHandle);
    load_assembly_and_get_function_pointer_fn LoadAsmAndGetFunPtr = (load_assembly_and_get_function_pointer_fn)funptr;
    // 判断是否获得成功
    if (funptr == nullptr){
        g_pMetaHookAPI->SysError("Can not find .NET 7 function handle!");
        return;
    }

    auto ret = LoadAsmAndGetFunPtr(L"dotnet/MHFramework/MHFramework.dll", L"MHFramework.Plugin, MHFramework", L"Init", UNMANAGEDCALLERSONLY_METHOD, nullptr, (void**)&CSharpInit);
    ret = LoadAsmAndGetFunPtr(L"dotnet/MHFramework/MHFramework.dll", L"MHFramework.Plugin, MHFramework", L"LoadClient", UNMANAGEDCALLERSONLY_METHOD, nullptr, (void**)&CSharpLoadClient);

    ret = LoadAsmAndGetFunPtr(L"dotnet/MHFramework/MHFramework.dll", L"MHFramework.Plugin, MHFramework", L"ShutDown", UNMANAGEDCALLERSONLY_METHOD, nullptr, (void**)&CSharpShutDown);

    ret = LoadAsmAndGetFunPtr(L"dotnet/MHFramework/MHFramework.dll", L"MHFramework.Plugin, MHFramework", L"LoadEngine", UNMANAGEDCALLERSONLY_METHOD, nullptr, (void**)&CSharpLoadEngine);
    ret = LoadAsmAndGetFunPtr(L"dotnet/MHFramework/MHFramework.dll", L"MHFramework.Plugin, MHFramework", L"ExitGame", UNMANAGEDCALLERSONLY_METHOD, nullptr, (void**)&CSharpExitGame);
  }


void FiniClr()
{
    if (ClrHandle != 0)
    {
        CloseFunPtr(ClrHandle);
    }
}
