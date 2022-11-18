#include <cstdio>
#include <memory>
#include <string>
#include <array>
#include <vector>
#include <locale>
#include <codecvt>

#include <nethost.h>
#include <coreclr_delegates.h>
#include <hostfxr.h>
#include <metahook.h>


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
void CSharpGetVersion() {

}

void InitClr(){
    auto trim = [&](std::string& s, char c = ' ') {
        s.erase(0, s.find_first_not_of(c));
        s.erase(s.find_last_not_of(c) + 1);
    };
    auto findDotnetPath = [&]() {
        DWORD dwLen = GetLogicalDriveStrings(0, NULL);	//��ȡϵͳ�ַ�������.
        char* pszDriver = new char[dwLen];				//����һ����Ӧ���ȵ�����.
        GetLogicalDriveStrings(dwLen, pszDriver);		//��ȡ�̷��ַ���.
        std::array<char, MAX_PATH> buffer;
        std::string runtimehead = "  Microsoft.WindowsDesktop.App 7";
        std::string sdkhead = " Base Path:   ";
        //Use powershell make everything fucking ok
        std::shared_ptr<FILE> pipe(_popen(("powershell &\"" + std::string(pszDriver) + "Program Files (x86)\\dotnet\\" + std::string("dotnet.exe\" --info")).c_str(), "r"), _pclose);
        std::vector<std::string> result(3);
        while (!feof(pipe.get())) {
            if (fgets(buffer.data(), MAX_PATH, pipe.get()) != nullptr) {
                std::string temp = buffer.data();
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
                    size_t comma = temp.find_first_of(':');
                    result[2] = temp.substr(comma + 4, temp.size() - comma);
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
    
    auto to_wide_string = [&](const std::string & input){
        std::wstring_convert<std::codecvt_utf8<wchar_t>> converter;
        return converter.from_bytes(input);
    };
    // ��ʼ��clr
    if (InitializeFunPtr(to_wide_string(turple[2] + std::string("/dotnet.runtimeconfig.json")).c_str(), nullptr, &ClrHandle)) {
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

    //��ȡpluginsharp.lst

    //���λ�ȡÿ���������ڵ�
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
