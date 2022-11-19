#include <metahook.h>

#include "Clr.h"

cl_enginefunc_t gEngfuncs;

void SharpPluginList() {
	size_t index = 0;
	gEngfuncs.Con_Printf("|index|         plugin name|      plugin version|\n");
	for (auto p : arySharpPlugins) {
		gEngfuncs.Con_Printf("|%5d|%20s|%20s|\n", index, p->Name, p->GetVersion());
		index++;
	}
}
void HUD_Init() {
	gEngfuncs.pfnAddCommand("mh_cspluginlist", SharpPluginList);
	gExportfuncs.HUD_Init();
}