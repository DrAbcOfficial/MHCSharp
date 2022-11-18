#pragma once

typedef struct sharpplugin_s {
	char Name[32];
	void (*PluginInit) (metahook_api_t* pAPI, mh_interface_t* pInterface, mh_enginesave_t* pSave);
	void (*LoadClient) (cl_exportfuncs_s*);
	void (*ShutDown) ();
	void (*LoadEngine) ();
	void (*ExitGame) (int iResult);
	char* (*GetVersion) ();
}sharpplugin_t;

void InitClr();

void FiniClr();


void CSharpInit(metahook_api_t* pAPI, mh_interface_t* pInterface, mh_enginesave_t* pSave);
void CSharpLoadClient(cl_exportfuncs_s* pExportFunc);
void CSharpShutDown();
void CSharpLoadEngine();
void CSharpExitGame(int iResult);
void CSharpGetVersion();