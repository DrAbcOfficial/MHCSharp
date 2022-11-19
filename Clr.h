#pragma once
#include <vector>

#define MAX_SHARPPLUGIN_NAME 32

typedef struct sharpplugin_s {
	char Name[MAX_SHARPPLUGIN_NAME];
	void (*PluginInit) (metahook_api_t* pAPI, mh_interface_t* pInterface, mh_enginesave_t* pSave);
	void (*LoadClient) (cl_exportfuncs_s*);
	void (*ShutDown) ();
	void (*LoadEngine) (cl_enginefunc_t* pEngfunc);
	void (*ExitGame) (int iResult);
	char* (*GetVersion) ();
}sharpplugin_t;

extern std::vector<sharpplugin_t*> arySharpPlugins;

void InitClr();

void FiniClr();


void CSharpInit(metahook_api_t* pAPI, mh_interface_t* pInterface, mh_enginesave_t* pSave);
void CSharpLoadClient(cl_exportfuncs_s* pExportFunc);
void CSharpShutDown();
void CSharpLoadEngine(cl_enginefunc_t* pEngfuncs);
void CSharpExitGame(int iResult);
void CSharpGetVersion();