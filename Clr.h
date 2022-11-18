#pragma once

void InitClr();

void FiniClr();


void CSharpInit(metahook_api_t* pAPI, mh_interface_t* pInterface, mh_enginesave_t* pSave);
void CSharpLoadClient(cl_exportfuncs_s*);
void CSharpShutDown();
void CSharpLoadEngine();
void CSharpExitGame(int);