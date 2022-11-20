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
    auto split = [&](const string& str, char delim) {
        size_t previous = 0;
        size_t current = str.find(delim);
        vector<string> elems;
        while (current != string::npos) {
            if (current > previous) {
                elems.push_back(str.substr(previous, current - previous));
            }
            previous = current + 1;
            current = str.find(delim, previous);
        }
        if (previous != str.size()) {
            elems.push_back(str.substr(previous));
        }
        return elems;
    };
    auto findDotnetPath = [&]() {
        vector<string> result(3);
        DWORD dwLen = GetLogicalDriveStrings(0, NULL);	//获取系统字符串长度.
        char* pszDriver = new char[dwLen];				//构建一个相应长度的数组.
        GetLogicalDriveStrings(dwLen, pszDriver);		//获取盘符字符串.
        string runtimehead = "  Microsoft.WindowsDesktop.App 7";
        string sdkhead = " Base Path:   ";

        string cmd = string(pszDriver) + "Program Files (x86)\\dotnet\\" + string("dotnet.exe --info");

        SECURITY_ATTRIBUTES sa;
        HANDLE hRead, hWrite;
        sa.nLength = sizeof(SECURITY_ATTRIBUTES);
        sa.lpSecurityDescriptor = NULL;
        sa.bInheritHandle = TRUE;

        if (!CreatePipe(&hRead, &hWrite, &sa, 0)) {
            g_pMetaHookAPI->SysError("Can not open pipe for searching...\nLast Error: %d", GetLastError());
            return result;
        }

        STARTUPINFO si;
        PROCESS_INFORMATION pi;
        ZeroMemory(&si, sizeof(STARTUPINFO));

        si.cb = sizeof(STARTUPINFO);
        GetStartupInfo(&si);
        si.hStdError = hWrite;
        si.hStdOutput = hWrite;
        si.wShowWindow = SW_HIDE;
        si.dwFlags = STARTF_USESHOWWINDOW | STARTF_USESTDHANDLES;

        char cmdline[200] = { 0 };
        sprintf(cmdline, "%s", cmd.c_str());
        if (!CreateProcess(NULL, cmdline, NULL, NULL, TRUE, NULL, NULL, NULL, &si, &pi)) {
            CloseHandle(hRead);
            CloseHandle(hWrite);
            g_pMetaHookAPI->SysError("Can create process for searching...\nLast Error: %d", GetLastError());
            return result;
        }
        CloseHandle(hWrite);
        char buffer[4096] = { 0 };
        DWORD bytesRead;
        string temp = "";
        while (true) {
            if (!ReadFile(hRead, buffer, 4095, &bytesRead, NULL)) 
                break;
            temp.append(buffer, bytesRead);
        }
        vector<string> aryT = split(temp, '\n');
        for (string s : aryT) {
            if (s.compare(0, runtimehead.size(), runtimehead) == 0) {
                size_t seven = s.find_first_of('7');
                size_t left = s.find_first_of('[');
                //runtime version
                result[1] = s.substr(seven, left - seven - 1);
                //base path
                result[0] = s.substr(left + 1, s.size() - left - 38);
            }
            else  if (s.compare(0, sdkhead.size(), sdkhead) == 0) {
                //sdk path
                size_t comma = s.find_first_of(':') + 4;
                result[2] = s.substr(comma, s.find_first_of('\n') - comma - 1);
                trim(result[2], '\r');
            }
        }
        DWORD exitCode = 0;
        GetExitCodeProcess(pi.hProcess, &exitCode);
        CloseHandle(hRead);
        CloseHandle(pi.hThread);
        CloseHandle(pi.hProcess);
        return result;
    };
    // 加载hostfxr.dll
    auto turple = findDotnetPath();
    HMODULE ClrDllHandle = LoadLibraryEx((turple[0] + "host\\fxr\\" + turple[1] + "\\hostfxr.dll").c_str(), nullptr, LOAD_WITH_ALTERED_SEARCH_PATH);
    if (!ClrDllHandle) {
        g_pMetaHookAPI->SysError("Can not find x86 .NET 7 sdk!\nDownload x86 .NET sdk:\nhttps://dotnet.microsoft.com/en-us/download/dotnet/7.0");
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
    
    auto to_wide_string = [&](const string & input){
        wstring_convert<codecvt_utf8<wchar_t>> converter;
        return converter.from_bytes(input);
    };
    // 初始化clr
    if (InitializeFunPtr(to_wide_string(turple[2] + string("/dotnet.runtimeconfig.json")).c_str(), nullptr, &ClrHandle)) {
        g_pMetaHookAPI->SysError("Can not find x86 .NET 7 config!\ndotnet/dotnet.runtimeconfig.json");
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

    //依次获取每个插件的入口点
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
