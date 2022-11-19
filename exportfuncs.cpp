#include <metahook.h>

#include "Clr.h"

cl_enginefunc_t gEngfuncs;

void SharpPluginList() {
	size_t index = 0;
	gEngfuncs.Con_Printf("|index|\t\tplugin name|\t\tplugin version|\n");
	for (auto p : arySharpPlugins) {
		gEngfuncs.Con_Printf("|\t%d|\t\t%s|\t\t%s|\n", index, p->Name, p->GetVersion());
		index++;
	}
}
void HUD_Init() {
	gEngfuncs.pfnAddCommand("mh_cspluginlist", SharpPluginList);
	gExportfuncs.HUD_Init();
}