#include <cstdio>
#include <memory>
#include <string>
#include <array>
#include <locale>
#include <codecvt>
#include <iostream>
#include <fstream>
#include <vector>

#include <nethost.h>
#include <coreclr_delegates.h>
#include <hostfxr.h>
#include <metahook.h>

#include <Clr.h>

using namespace std;

extern cl_exportfuncs_t gExportfuncs;

hostfxr_handle ClrHandle = 0;
hostfxr_close_fn CloseFunPtr = nullptr;

void (*CSharpInit)(metahook_api_t* pAPI, mh_interface_t* pInterface, mh_enginesave_t* pSave) = nullptr;
void (*CSharpLoadClient)(cl_exportfuncs_s* pExportFunc) = nullptr;
void (*CSharpShutDown)() = nullptr;
void (*CSharpLoadEngine)(cl_enginefunc_t* pEngfuncs) = nullptr;
void (*CSharpExitGame)(int iResult) = nullptr;
char* (*CSharpGetVersion)() = nullptr;

void InitClr(){
    auto trim = [&](string& s, char c = ' ') {
        s.erase(0, s.find_first_not_of(c));
        s.erase(s.find_last_not_of(c) + 1);
    };
    auto findDotnetPath = [&]() {
        DWORD dwLen = GetLogicalDriveStrings(0, NULL);	//��ȡϵͳ�ַ�������.
        char* pszDriver = new char[dwLen];				//����һ����Ӧ���ȵ�����.
        GetLogicalDriveStrings(dwLen, pszDriver);		//��ȡ�̷��ַ���.
        array<char, MAX_PATH> buffer;
        string runtimehead = "  Microsoft.WindowsDesktop.App 7";
        string sdkhead = " Base Path:   ";
        //Use powershell make everything fucking ok
        shared_ptr<FILE> pipe(_popen(("powershell &\"" + string(pszDriver) + "Program Files (x86)\\dotnet\\" + string("dotnet.exe\" --info")).c_str(), "r"), _pclose);
        vector<string> result(3);
        while (!feof(pipe.get())) {
            if (fgets(buffer.data(), MAX_PATH, pipe.get()) != nullptr) {
                string temp = buffer.data();
                if (temp.compare(0, runtimehead.size(), runtimehead) == 0) {
                    size_t seven = temp.find_first_of('7');
                    size_t left = temp.find_first_of('[');
                    //runtime version
                    result[1] = temp.substr(seven, left - seven - 1);
                    //base path
                    result[0] = temp.substr(left + 1, temp.size() - left - 38);
                }
                else  if (temp.compare(0, sdkhead.size(), sdkhead) == 0) {
                    //sdk path
                    size_t comma = temp.find_first_of(':') + 4;
                    result[2] = temp.substr(comma, temp.find_first_of('\n') - comma - 1);
                }
            }
        }
        return result;
    };
    // ����hostfxr.dll
    auto turple = findDotnetPath();
    HMODULE ClrDllHandle = LoadLibraryEx((turple[0] + "host\\fxr\\" + turple[1] + "\\hostfxr.dll").c_str(), nullptr, LOAD_WITH_ALTERED_SEARCH_PATH);
    if (!ClrDllHandle) {
        g_pMetaHookAPI->SysError("Can not find x86 .NET 7 sdk!\nDownload x86 .NET sdk:\nhttps://dotnet.microsoft.com/en-us/download/dotnet/7.0");
        return;
    }
    // ��ȡ����
    auto InitializeFunPtr = (hostfxr_initialize_for_runtime_config_fn)::GetProcAddress(ClrDllHandle, "hostfxr_initialize_for_runtime_config");
    auto GetDelegateFunPtr = (hostfxr_get_runtime_delegate_fn)::GetProcAddress(ClrDllHandle, "hostfxr_get_runtime_delegate");
    CloseFunPtr = (hostfxr_close_fn)::GetProcAddress(ClrDllHandle, "hostfxr_close");
    if (InitializeFunPtr == nullptr || GetDelegateFunPtr == nullptr || CloseFunPtr == nullptr){
        g_pMetaHookAPI->SysError("Can not find .NET 7 function ptr!");
        return;
    }
    
    auto to_wide_string = [&](const string & input){
        wstring_convert<codecvt_utf8<wchar_t>> converter;
        return converter.from_bytes(input);
    };
    // ��ʼ��clr
    if (InitializeFunPtr(to_wide_string(turple[2] + string("/dotnet.runtimeconfig.json")).c_str(), nullptr, &ClrHandle)) {
        g_pMetaHookAPI->SysError("Can not find x86 .NET 7 config!\ndotnet/dotnet.runtimeconfig.json");
        CloseFunPtr(ClrHandle);
        return;
    }

    // ��ȡһ������ �����ػ�ಢ��ú���ָ�롱�ĺ���
    void* funptr;
    GetDelegateFunPtr(ClrHandle, hdt_load_assembly_and_get_function_pointer, &funptr);
    // ��ȡ�ɹ��Ժ���ܰѾ�����ˣ���Ҳ��֪����Ϊʲô
    CloseFunPtr(ClrHandle);
    load_assembly_and_get_function_pointer_fn LoadAsmAndGetFunPtr = (load_assembly_and_get_function_pointer_fn)funptr;
    // �ж��Ƿ��óɹ�
    if (funptr == nullptr){
        g_pMetaHookAPI->SysError("Can not find .NET 7 function handle!");
        return;
    }

    //���λ�ȡÿ���������ڵ�
#define DOTNET_PLUGIN_PATH L"svencoop/metahook/plugins/dotnet/MHSharpFrame/MHSharpFrame.dll"
#define DOTNET_PLUGIN_SIGN L"MHSharpFrame.Plugin, MHSharpFrame"
    int ret = LoadAsmAndGetFunPtr(DOTNET_PLUGIN_PATH, DOTNET_PLUGIN_SIGN, L"Init", UNMANAGEDCALLERSONLY_METHOD, nullptr, (void**)&CSharpInit);
    if (ret != 0) {
        g_pMetaHookAPI->SysError("Can not MHSharpFrame in that path!\nsvencoop/metahook/plugins/dotnet/MHSharpFrame/MHSharpFrame.dll");
        return;
    }
    ret = LoadAsmAndGetFunPtr(DOTNET_PLUGIN_PATH, DOTNET_PLUGIN_SIGN, L"LoadClient", UNMANAGEDCALLERSONLY_METHOD, nullptr, (void**)&CSharpLoadClient);
    ret = LoadAsmAndGetFunPtr(DOTNET_PLUGIN_PATH, DOTNET_PLUGIN_SIGN, L"ShutDown", UNMANAGEDCALLERSONLY_METHOD, nullptr, (void**)&CSharpShutDown);
    ret = LoadAsmAndGetFunPtr(DOTNET_PLUGIN_PATH, DOTNET_PLUGIN_SIGN, L"LoadEngine", UNMANAGEDCALLERSONLY_METHOD, nullptr, (void**)&CSharpLoadEngine);
    ret = LoadAsmAndGetFunPtr(DOTNET_PLUGIN_PATH, DOTNET_PLUGIN_SIGN, L"ExitGame", UNMANAGEDCALLERSONLY_METHOD, nullptr, (void**)&CSharpExitGame);
    ret = LoadAsmAndGetFunPtr(DOTNET_PLUGIN_PATH, DOTNET_PLUGIN_SIGN, L"GetVersion", UNMANAGEDCALLERSONLY_METHOD, nullptr, (void**)&CSharpGetVersion);
  }

void FiniClr()
{
    if (ClrHandle != 0)
    {
        CloseFunPtr(ClrHandle);
    }
}
