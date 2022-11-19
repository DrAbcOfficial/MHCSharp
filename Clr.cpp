#include <cstdio>
#include <memory>
#include <string>
#include <array>
#include <locale>
#include <codecvt>
#include <iostream>
#include <fstream>

#include <nethost.h>
#include <coreclr_delegates.h>
#include <hostfxr.h>
#include <metahook.h>

#include <Clr.h>

using namespace std;

extern cl_exportfuncs_t gExportfuncs;

vector<sharpplugin_t*> arySharpPlugins = {};

hostfxr_handle ClrHandle = 0;
hostfxr_close_fn CloseFunPtr = nullptr;
void CSharpInit(metahook_api_t* pAPI, mh_interface_t* pInterface, mh_enginesave_t* pSave) {
    for (auto p : arySharpPlugins) {
        p->PluginInit(pAPI, pInterface, pSave);
    }
};
void CSharpLoadEngine(cl_enginefunc_t* pEngfuncs) {
    for (auto p : arySharpPlugins) {
        p->LoadEngine(pEngfuncs);
    }
};
void CSharpLoadClient(cl_exportfuncs_s* pExportFunc) {
    for (auto p : arySharpPlugins) {
        p->LoadClient(pExportFunc);
    }
};
void CSharpShutDown() {
    for (auto p : arySharpPlugins) {
        p->ShutDown();
    }
};
void CSharpExitGame(int iResult) {
    for (auto p : arySharpPlugins) {
        p->ExitGame(iResult);
    }
};
void CSharpGetVersion() {

}

void InitClr(){
    auto trim = [&](string& s, char c = ' ') {
        s.erase(0, s.find_first_not_of(c));
        s.erase(s.find_last_not_of(c) + 1);
    };
    auto findDotnetPath = [&]() {
        DWORD dwLen = GetLogicalDriveStrings(0, NULL);	//获取系统字符串长度.
        char* pszDriver = new char[dwLen];				//构建一个相应长度的数组.
        GetLogicalDriveStrings(dwLen, pszDriver);		//获取盘符字符串.
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

    //读取pluginsharp.lst
#define DOTNET_PLUGIN_LIST "svencoop/metahook/configs/plugins_dotnet.lst"
    ifstream fp;
    fp.open(DOTNET_PLUGIN_LIST, ios::in);
    if (!fp.good())
        return;
    while (!fp.eof()) {
        string str;
        getline(fp, str);
        if (str.size() <= MAX_SHARPPLUGIN_NAME) {
            sharpplugin_t* newPlugin = new sharpplugin_t();
            strcpy_s(newPlugin->Name, str.c_str());
            arySharpPlugins.push_back(newPlugin);
        }
    }
    fp.close();

    //依次获取每个插件的入口点
#define DOTNET_PLUGIN_PATH "svencoop/metahook/plugins/dotnet/%s/%s.dll"
#define DOTNET_PLUGIN_SIGN "%s.Plugin, %s"
    for (auto iter = arySharpPlugins.begin(); iter != arySharpPlugins.end();iter++) {
        auto plug = *iter;
        char path[MAX_PATH];
        sprintf(path, DOTNET_PLUGIN_PATH, plug->Name, plug->Name);
        if (!ifstream(path).good()) {
            delete plug;
            *iter = nullptr;
            arySharpPlugins.erase(iter);
            return;
        }

        wchar_t* wpath = new wchar_t[MAX_PATH];
        MultiByteToWideChar(CP_ACP, 0, path, -1, wpath, MAX_PATH);

        char sign[MAX_PATH];
        sprintf(sign, DOTNET_PLUGIN_SIGN, plug->Name, plug->Name);
        wchar_t* wsign = new wchar_t[MAX_PATH];
        MultiByteToWideChar(CP_ACP, 0, sign, -1, wsign, MAX_PATH);

        auto ret = LoadAsmAndGetFunPtr(wpath, wsign, L"Init", UNMANAGEDCALLERSONLY_METHOD, nullptr, (void**)&plug->PluginInit);
        ret = LoadAsmAndGetFunPtr(wpath, wsign, L"LoadClient", UNMANAGEDCALLERSONLY_METHOD, nullptr, (void**)&plug->LoadClient);
        ret = LoadAsmAndGetFunPtr(wpath, wsign, L"ShutDown", UNMANAGEDCALLERSONLY_METHOD, nullptr, (void**)&plug->ShutDown);
        ret = LoadAsmAndGetFunPtr(wpath, wsign, L"LoadEngine", UNMANAGEDCALLERSONLY_METHOD, nullptr, (void**)&plug->LoadEngine);
        ret = LoadAsmAndGetFunPtr(wpath, wsign, L"ExitGame", UNMANAGEDCALLERSONLY_METHOD, nullptr, (void**)&plug->ExitGame);
        ret = LoadAsmAndGetFunPtr(wpath, wsign, L"GetVersion", UNMANAGEDCALLERSONLY_METHOD, nullptr, (void**)&plug->GetVersion);
    }
  }


void FiniClr()
{
    if (ClrHandle != 0)
    {
        CloseFunPtr(ClrHandle);
    }
}
