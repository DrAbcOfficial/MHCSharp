#include <metahook.h>
#include "Clr.h"
#include <iostream>

cl_exportfuncs_t gExportfuncs;
mh_interface_t *g_pInterface;
metahook_api_t *g_pMetaHookAPI;
mh_enginesave_t *g_pMetaSave;

void IPlugins::Init(metahook_api_t *pAPI, mh_interface_t *pInterface, mh_enginesave_t *pSave)
{
	InitClr();
	g_pInterface = pInterface;
	g_pMetaHookAPI = pAPI;
	g_pMetaSave = pSave;
	CSharpInit(pAPI, pInterface, pSave);
}

void IPlugins::Shutdown(void)
{
	CSharpShutDown();
	FiniClr();
}

void IPlugins::LoadEngine(void)
{
	CSharpLoadEngine();
}

void IPlugins::LoadClient(cl_exportfuncs_t *pExportFunc)
{
	memcpy(&gExportfuncs, pExportFunc, sizeof(gExportfuncs));
	CSharpLoadClient(pExportFunc);
}

void IPlugins::ExitGame(int iResult)
{
	CSharpExitGame(iResult);
}

EXPOSE_SINGLE_INTERFACE(IPlugins, IPlugins, METAHOOK_PLUGIN_API_VERSION);