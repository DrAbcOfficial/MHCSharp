#pragma once
#define MAX_SHARPPLUGIN_NAME 32


void InitClr();

void FiniClr();


extern void (*CSharpInit)(metahook_api_t* pAPI, mh_interface_t* pInterface, mh_enginesave_t* pSave);
extern void (*CSharpLoadClient)(cl_exportfuncs_s* pExportFunc);
extern void (*CSharpShutDown)();
extern void (*CSharpLoadEngine)(cl_enginefunc_t* pEngfuncs);
extern void (*CSharpExitGame)(int iResult);
extern char* (*CSharpGetVersion)();