#include <metahook.h>
#include "Clr.h"

cl_enginefunc_t gEngfuncs;

void HUD_Init() {
	gEngfuncs.pfnAddCommand("mh_listcsplugin", {

	});
}