#include <metahook.h>
#include "plugins.h"

#include "Clr.h"

cl_exportfuncs_t gExportfuncs;
mh_interface_t *g_pInterface;
metahook_api_t *g_pMetaHookAPI;
mh_enginesave_t *g_pMetaSave;
IFileSystem *g_pFileSystem;

HINSTANCE g_hInstance, g_hThisModule, g_hEngineModule;
PVOID g_dwEngineBase;
DWORD g_dwEngineSize;
PVOID g_dwEngineTextBase;
DWORD g_dwEngineTextSize;
PVOID g_dwEngineDataBase;
DWORD g_dwEngineDataSize;
PVOID g_dwEngineRdataBase;
DWORD g_dwEngineRdataSize;
DWORD g_dwEngineBuildnum;
int g_iEngineType;
PVOID g_dwClientBase;
DWORD g_dwClientSize;

void IPluginsV4::Init(metahook_api_t *pAPI, mh_interface_t *pInterface, mh_enginesave_t *pSave)
{
	g_pInterface = pInterface;
	g_pMetaHookAPI = pAPI;
	g_pMetaSave = pSave;
	g_hInstance = GetModuleHandle(NULL);
	
	InitClr();
	CSharpInit(pAPI, pInterface, pSave);
}

void IPluginsV4::Shutdown(void)
{
	CSharpShutDown();
	FiniClr();
}

void IPluginsV4::LoadEngine(cl_enginefunc_t *pEngfuncs)
{
	g_pFileSystem = g_pInterface->FileSystem;
	g_iEngineType = g_pMetaHookAPI->GetEngineType();
	g_dwEngineBuildnum = g_pMetaHookAPI->GetEngineBuildnum();
	g_hEngineModule = g_pMetaHookAPI->GetEngineModule();
	g_dwEngineBase = g_pMetaHookAPI->GetEngineBase();
	g_dwEngineSize = g_pMetaHookAPI->GetEngineSize();
	g_dwEngineTextBase = g_pMetaHookAPI->GetSectionByName(g_dwEngineBase, ".text\x0\x0\x0", &g_dwEngineTextSize);
	g_dwEngineDataBase = g_pMetaHookAPI->GetSectionByName(g_dwEngineBase, ".data\x0\x0\x0", &g_dwEngineDataSize);
	g_dwEngineRdataBase = g_pMetaHookAPI->GetSectionByName(g_dwEngineBase, ".rdata\x0\x0", &g_dwEngineRdataSize);

	CSharpLoadEngine(pEngfuncs);
}

void IPluginsV4::LoadClient(cl_exportfuncs_t *pExportFunc)
{
	memcpy(&gExportfuncs, pExportFunc, sizeof(gExportfuncs));

	g_dwClientBase = g_pMetaHookAPI->GetClientBase();
	g_dwClientSize = g_pMetaHookAPI->GetClientSize();

	CSharpLoadClient(pExportFunc);
}

void IPluginsV4::ExitGame(int iResult)
{
	CSharpExitGame(iResult);
}

const char *IPluginsV4::GetVersion(void)
{
	return CSharpGetVersion();
}

EXPOSE_SINGLE_INTERFACE(IPluginsV4, IPluginsV4, METAHOOK_PLUGIN_API_VERSION_V4);