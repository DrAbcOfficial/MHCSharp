#pragma once

void InitClr();

void FiniClr();


extern void (*CSharpInit)(metahook_api_t* pAPI, mh_interface_t* pInterface, mh_enginesave_t* pSave);
extern void (*CSharpLoadClient)(cl_exportfuncs_s*);
extern void (*CSharpShutDown)();
extern void (*CSharpLoadEngine)();
extern void (*CSharpExitGame)(int);