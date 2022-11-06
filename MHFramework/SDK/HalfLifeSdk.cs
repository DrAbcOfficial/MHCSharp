using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MHFramework.SDK;


public unsafe struct ClExportFuncsStruct
{
    public delegate* unmanaged[Cdecl]<CLEngineFucsStruct*, int, int> Initialize = null;
    public delegate* unmanaged[Cdecl]<void> HudInit = null;
    public delegate* unmanaged[Cdecl]<int> HudVidIni = null;
    public delegate* unmanaged[Cdecl]<float, int, int> HudReDraw = null;
    public delegate* unmanaged[Cdecl]<ClientDataStruct*, float, int> HudUpdateClientData = null;
    public delegate* unmanaged[Cdecl]<void> HudReset = null;
    public delegate* unmanaged[Cdecl]<PlayerMoveStruct*, int, void> HudPlayerMove = null;
    public delegate* unmanaged[Cdecl]<PlayerMoveStruct*, void> HudPlayerMoveInit = null;
    public delegate* unmanaged[Cdecl]<byte*, byte> HudPlayerMoveTexture = null;
    public delegate* unmanaged[Cdecl]<void> InActivateMouse = null;
    public delegate* unmanaged[Cdecl]<void> InDeactivateMouse = null;
    public delegate* unmanaged[Cdecl]<int, void> InMouseEvent = null;
    public delegate* unmanaged[Cdecl]<void> InClearStates = null;
    public delegate* unmanaged[Cdecl]<void> InAccumulate = null;
    public delegate* unmanaged[Cdecl]<float, UserCmdStruct*, int, void> ClCreateMove = null;
    public delegate* unmanaged[Cdecl]<int> IsThridPerson = null;
    public delegate* unmanaged[Cdecl]<float*, void> ClCameraOffset = null;
    public delegate* unmanaged[Cdecl]<byte*, KButtonStruct*> KbFind = null;
    public delegate* unmanaged[Cdecl]<void> CamThink = null;
    public delegate* unmanaged[Cdecl]<RefParamStruct*, void> VCalcRefDef = null;
    public delegate* unmanaged[Cdecl]<int, ClEntityStruct*, byte*, int> HudAddEntity = null;
    public delegate* unmanaged[Cdecl]<void> HudCreateEntities = null;
    public delegate* unmanaged[Cdecl]<void> HudDrawNormalTriangles = null;
    public delegate* unmanaged[Cdecl]<void> HudDrawTransparentTrianges = null;
    public delegate* unmanaged[Cdecl]<MStudioEventStruct*, ClEntityStruct*, void> HudStudioEvent = null;
    public delegate* unmanaged[Cdecl]<LocalStateStruct*, LocalStateStruct*, UserCmdStruct*, int, double, uint, void> HudPostRunCmd = null;
    public delegate* unmanaged[Cdecl]<void> HudShutDown = null;
    public delegate* unmanaged[Cdecl]<EntityStateStruct*, ClientDataStruct*, void> HudTxferLocalOverrides = null;
    public delegate* unmanaged[Cdecl]<EntityStateStruct*, EntityStateStruct*, void> HudProcessPlayerState = null;
    public delegate* unmanaged[Cdecl]<EntityStateStruct*, EntityStateStruct*, ClientDataStruct*, ClientDataStruct*, WeaponDataStruct*, WeaponDataStruct*, void> HudTxferPredictionData = null;
    public delegate* unmanaged[Cdecl]<int, byte*, void> DemoReadBuffer = null;
    public delegate* unmanaged[Cdecl]<NetAdrStruct*, byte*, byte*, int*, int> HudConnectionlessPack = null;
    public delegate* unmanaged[Cdecl]<int, float*, float*, int> HudGetHullBounds = null;
    public delegate* unmanaged[Cdecl]<double, void> HudFrame = null;
    public delegate* unmanaged[Cdecl]<int, int, byte*, int> HudKeyEvent = null;
    public delegate* unmanaged[Cdecl]<double, double, double, TempentStruct**, TempentStruct**, delegate* unmanaged[Cdecl]<ClEntityStruct*, int>, delegate* unmanaged[Cdecl]<TempentStruct*, float, void>, void> HudTempEntUpdate = null;
    public delegate* unmanaged[Cdecl]<int, ClEntityStruct*> HudGetUserEntity = null;
    public delegate* unmanaged[Cdecl]<int, int, void> HudVoiceStatus = null;
    public delegate* unmanaged[Cdecl]<int, void*, void> HudDirectorMessage = null;
    public delegate* unmanaged[Cdecl]<int, RStudioInterfaceStruct**, EngineStudioApiStruct*, int> HudGetStudioModelInterface = null;
    public delegate* unmanaged[Cdecl]<int*, int*, void> HudChatInputPosition = null;
    public delegate* unmanaged[Cdecl]<int, int> HudGetPayerTeam = null;
    public delegate* unmanaged[Cdecl]<void*> ClientFactory = null;
    public ClExportFuncsStruct()
    {
    }
}

public unsafe struct CLEngineFucsStruct
{
    public delegate* unmanaged[Cdecl]<byte*, int> SprLoad = null;
    public delegate* unmanaged[Cdecl]<int, int> SprFrames = null;
    public delegate* unmanaged[Cdecl]<int, int, int> SprHeight = null;
    public delegate* unmanaged[Cdecl]<int, int, int> SprWidth = null;
    public delegate* unmanaged[Cdecl]<int, int, int, int, void> SprSet = null;
    public delegate* unmanaged[Cdecl]<int, int, int, RectStruct*, void> SprDraw = null;
    public delegate* unmanaged[Cdecl]<int, int, int, RectStruct*, void> SprDrawHoles = null;
    public delegate* unmanaged[Cdecl]<int, int, int, RectStruct*, void> SprDrawAdditive = null;
    public delegate* unmanaged[Cdecl]<int, int, int, int> SprEnableScissor = null;
    public delegate* unmanaged[Cdecl]<void> SprDisableScissor = null;
    public delegate* unmanaged[Cdecl]<byte*, int*, ClientSpriteStruct*> SprGetList = null;
    public delegate* unmanaged[Cdecl]<int, int, int, int, int, int, int, int, void> FillRgba = null;
    public delegate* unmanaged[Cdecl]<ScreenInfoStruct*, int> GetScreenInfo = null;
    public delegate* unmanaged[Cdecl]<int, RectStruct, int, int, int, void> SetCrosshair = null;
    public delegate* unmanaged[Cdecl]<byte*, byte*, int, CVarStruct*> RegisterVariable = null;
    public delegate* unmanaged[Cdecl]<byte*, float> GetCVarFloat = null;
    public delegate* unmanaged[Cdecl]<byte*, byte*> GetCVarString = null;
    public delegate* unmanaged[Cdecl]<byte*, delegate* unmanaged[Cdecl]<void>, int> AddCommand = null;
    public delegate* unmanaged[Cdecl]<byte*, delegate* unmanaged[Cdecl]<byte, int, void*, int>, int> HookUserMsg = null;
    public delegate* unmanaged[Cdecl]<byte*, int> ServerCmd = null;
    public delegate* unmanaged[Cdecl]<byte*, int> ClientCmd = null;
    public delegate* unmanaged[Cdecl]<int, HudPlayerInfoStruct*, void> GetPlayerInfo = null;
    public delegate* unmanaged[Cdecl]<byte*, float, void> PlaySoundByName = null;
    public delegate* unmanaged[Cdecl]<int, float, void> PlaySoundByIndex = null;
    public delegate* unmanaged[Cdecl]<float*, float*, float*, float*, void> AngleVectors = null;
    public delegate* unmanaged[Cdecl]<byte*, ClientTextMessageStruct*> TextMessageGet = null;
    public delegate* unmanaged[Cdecl]<int, int, int, int, int, int, int> DrawCharacter = null;
    public delegate* unmanaged[Cdecl]<int, int, byte*, int> DrawConsoleString = null;
    public delegate* unmanaged[Cdecl]<float, float, float, void> DrawSetTextColor = null;
    public delegate* unmanaged[Cdecl]<byte*, int*, int*, void> DrawConsoleStringLen = null;
    public delegate* unmanaged[Cdecl]<byte*, void> ConsolePrint = null;
    public delegate* unmanaged[Cdecl]<byte*, void> CenterPrint = null;
    public delegate* unmanaged[Cdecl]<int> GetWindowCenterX = null;
    public delegate* unmanaged[Cdecl]<int> GetWindowCenterY = null;
    public delegate* unmanaged[Cdecl]<float*, void> GetViewAngles = null;
    public delegate* unmanaged[Cdecl]<float*, void> SetViewAngles = null;
    public delegate* unmanaged[Cdecl]<int> GetMaxClients = null;
    public delegate* unmanaged[Cdecl]<byte*, float, void> CVarSetValue = null;
    public delegate* unmanaged[Cdecl]<int> CmdArgc = null;
    public delegate* unmanaged[Cdecl]<int, byte*> CmdArgv = null;
    public IntPtr ConPrintf = default; //不会抄
    public IntPtr ConDPrintf = default;//不会抄
    public IntPtr ConNPrintf = default;//不会抄
    public IntPtr ConNXPrintf = default;//不会抄
    public delegate* unmanaged[Cdecl]<byte*, byte*> PhyInfoValueForKey = null;
    public delegate* unmanaged[Cdecl]<byte*, byte*> ServerInfoValueForKey = null;
    public delegate* unmanaged[Cdecl]<byte*, byte**, int> CheckParm = null;
    public delegate* unmanaged[Cdecl]<int, int, void> KeyEvent = null;
    public delegate* unmanaged[Cdecl]<int*, int*, void> GetMousePosition = null;
    public delegate* unmanaged[Cdecl]<int> IsNoClipping = null;
    public delegate* unmanaged[Cdecl]<ClEntityStruct*> GetLocalPlayer = null;
    public delegate* unmanaged[Cdecl]<ClEntityStruct*> GetViewModel = null;
    public delegate* unmanaged[Cdecl]<int, ClEntityStruct*> GetEntityByIndex = null;
    public delegate* unmanaged[Cdecl]<float> GetClientTime = null;
    public delegate* unmanaged[Cdecl]<void> VCakcShake = null;
    public delegate* unmanaged[Cdecl]<float*, float*, float, void> VApplyShake = null;
    public delegate* unmanaged[Cdecl]<float*, int*, int> PMPointContents = null;
    public delegate* unmanaged[Cdecl]<float*, int> PMWaterEntity = null;
    public delegate* unmanaged[Cdecl]<float*, float*, int, int, int, PMTraceStruct*> PMTraceLine = null;
    public delegate* unmanaged[Cdecl]<byte*, int*, ModelStruct*> ClLoadModel = null;
    public delegate* unmanaged[Cdecl]<int, ClEntityStruct*, int> ClCreateVisibleEntity = null;
    public delegate* unmanaged[Cdecl]<int, ModelStruct*> GetSpritePointer = null;
    public delegate* unmanaged[Cdecl]<byte*, float, float*, void> PlaySoundByNameAtLocation = null;
    public delegate* unmanaged[Cdecl]<int, byte*, ushort> PrecacheEvent = null;
    public delegate* unmanaged[Cdecl]<int, int, void> WeaponAnim = null;
    public delegate* unmanaged[Cdecl]<float, float, float> RandomFloat = null;
    public delegate* unmanaged[Cdecl]<int, int, int> RandomLong = null;
    public delegate* unmanaged[Cdecl]<byte*, delegate* unmanaged[Cdecl]<EventArgsStruct*, void>, void> HookEvent = null;
    public delegate* unmanaged[Cdecl]<byte*> ConIsVisible = null;
    public delegate* unmanaged[Cdecl]<byte*> GetGameDirectory = null;
    public delegate* unmanaged[Cdecl]<byte*, CVarStruct*> GFetCVarPointer = null;
    public delegate* unmanaged[Cdecl]<byte*, byte*> KeyLookupBinding = null;
    public delegate* unmanaged[Cdecl]<byte*> GetLevelName = null;
    public delegate* unmanaged[Cdecl]<ScreenFadeStruct*, void> GetScreenFade = null;
    public delegate* unmanaged[Cdecl]<ScreenFadeStruct*, void> SetScreenFade = null;
    public delegate* unmanaged[Cdecl]<void*> GetPanel = null;
    public delegate* unmanaged[Cdecl]<int*, void> VGuiViewportPaintBackground = null;
    public delegate* unmanaged[Cdecl]<byte*, int, int*, byte*> ComLoadFile = null;
    public delegate* unmanaged[Cdecl]<byte*, byte*, byte*> ComParseFile = null;
    public delegate* unmanaged[Cdecl]<void*, void> ComFreeFile = null;
    public IntPtr TriApi = default;
    public IntPtr EfxApi = default;
    public IntPtr EventApi = default;
    public IntPtr DemoApi = default;
    public IntPtr NetApi = default;
    public IntPtr VoiceTweak = default;
    public delegate* unmanaged[Cdecl]<int> IsSpectateOnly = null;
    public delegate* unmanaged[Cdecl]<byte*, ModelStruct*> LoadMapSprite = null;
    public delegate* unmanaged[Cdecl]<byte*, byte*, void> ComAddAppDirectoryToSearchPath = null;
    public delegate* unmanaged[Cdecl]<byte*, byte*, int, byte*> ComExpandFilename = null;
    public delegate* unmanaged[Cdecl]<int, byte*, byte*> PlayerInfoValueForKey = null;
    public delegate* unmanaged[Cdecl]<byte*, byte*, void> PlayerInfoSetValueForKey = null;
    public delegate* unmanaged[Cdecl]<int, byte*, int> GetPlayerUniqueID = null;
    public delegate* unmanaged[Cdecl]<int, int> GetTrackerIDForPlayer = null;
    public delegate* unmanaged[Cdecl]<int, int> GetPlayerForTrackerID = null;
    public delegate* unmanaged[Cdecl]<byte*, int> ServerCmdUnReliable = null;
    public delegate* unmanaged[Cdecl]<PointStruct*, void> GetMousePos = null;
    public delegate* unmanaged[Cdecl]<int, int, void> SetMousePos = null;
    public delegate* unmanaged[Cdecl]<int, void> SetMouseEnable = null;
    public delegate* unmanaged[Cdecl]<CVarStruct*> GetFirstVarPtr = null;
    public delegate* unmanaged[Cdecl]<uint> GetFirstCmdFunctionHandle = null;
    public delegate* unmanaged[Cdecl]<uint, uint> GetNextCmdFunctionHandle = null;
    public delegate* unmanaged[Cdecl]<uint, byte*> GetCmdFunctionName = null;
    public delegate* unmanaged[Cdecl]<float> HudGetClientOldTime = null;
    public delegate* unmanaged[Cdecl]<float> HudGetServerGravityValue = null;
    public delegate* unmanaged[Cdecl]<int, ModelStruct*> GetModelByIndex = null;
    public delegate* unmanaged[Cdecl]<int, void> SetFilterMode = null;
    public delegate* unmanaged[Cdecl]<float, float, float, void> SetFilterColor = null;
    public delegate* unmanaged[Cdecl]<float, void> SetFilterBrightness = null;
    public delegate* unmanaged[Cdecl]<byte*, byte*, SequenceEntryStrct*> SequenceGet = null;
    public delegate* unmanaged[Cdecl]<int, int, int, RectStruct*, int, int, int, int, void> SprDrawGeneric = null;
    public delegate* unmanaged[Cdecl]<byte*, int, int*, SequenceEntryStrct*> SequecePickSentence = null;
    public delegate* unmanaged[Cdecl]<int, int, byte*, int, int, int, int> DrawString = null;
    public delegate* unmanaged[Cdecl]<int, int, byte*, int, int, int, int> DrawStringReverse = null;
    public delegate* unmanaged[Cdecl]<byte*, byte*> LocalPlayerInfoValueForKey = null;
    public delegate* unmanaged[Cdecl]<int, int, int, uint, int> VGUI2DrawCharacter = null;
    public delegate* unmanaged[Cdecl]<int, int, int, int, int, int, uint, int> VGUI2DrawCharacterAdd = null;
    public delegate* unmanaged[Cdecl]<byte*, uint> GetApproxWavaPlayLength = null;
    public delegate* unmanaged[Cdecl]<void*> GetCarerrUI = null;
    public delegate* unmanaged[Cdecl]<byte*, byte*, void> CVarSet = null;
    public delegate* unmanaged[Cdecl]<int> IsCareerMatch = null;
    public delegate* unmanaged[Cdecl]<byte*, float, int, void> PlaySoundVoiceByName = null;
    public delegate* unmanaged[Cdecl]<byte*, int, void> PrimeMusicStream = null;
    public delegate* unmanaged[Cdecl]<double> GetAbsoluteTime = null;
    public delegate* unmanaged[Cdecl]<int*, int, void> ProcessTutorMessageDecayBuffer = null;
    public delegate* unmanaged[Cdecl]<int*, int, void> ConstructTutorMessageDecayBuffer = null;
    public delegate* unmanaged[Cdecl]<byte*, float, int, void> PlaySoundByNameAtPitch = null;
    public delegate* unmanaged[Cdecl]<int, int, int, int, int, int, int, int, void> FillRGBABlend = null;
    public delegate* unmanaged[Cdecl]<int> GetAppID = null;
    public delegate* unmanaged[Cdecl]<CmdAliasStruct*> GetAliasList = null;
    public delegate* unmanaged[Cdecl]<int*, int*, void> VguiWrap2GetMouseDelta = null;

    public CLEngineFucsStruct()
    {

    }
}

public unsafe struct MHEngineSaveStrct
{
    public ClExportFuncsStruct* ExportFuncs = null;
    public CLEngineFucsStruct* EngineFuncs = null;

    public MHEngineSaveStrct()
    {
    }
}


public unsafe struct HudPlayerInfoStruct
{
    public HudPlayerInfoStruct()
    {
    }
    public byte* Name = null;
    public short Ping = 0;
    public byte ThisPlayer = 0;
    public byte Spectator = 0;
    public byte PacketLoss = 0;
    public byte* Model = null;
    public short TopColor = 0;
    public short BottomColor = 0;
    public ulong SteamId = 0;
}
public struct PointStruct
{
    public long X = default;
    public long Y = default;
    public PointStruct()
    {

    }
}
public unsafe struct ClientTextMessageStruct
{
    public int Effect = default;
    public byte R1 = default, G1 = default, B1 = default, A1 = default;
    public byte R2 = default, G2 = default, B2 = default, A2 = default;
    public float X = default;
    public float Y = default;
    public float FadeIn = default;
    public float FadeOut = default;
    public float HoldTime = default;
    public float FxTime = default;
    public byte* Name = null;
    public byte* Message = null;

    public ClientTextMessageStruct()
    {
    }
}

public unsafe struct RectStruct
{
    public int Left = 0;
    public int Right = 0;
    public int Top = 0;
    public int Bottom = 0;

    public RectStruct()
    {
    }
}
public unsafe struct ClientDataStruct
{
    public Vector3 Origin = Vector3.Zero;
    public Vector3 ViewAngles = Vector3.Zero;
    public int WeaponBits = 0;
    public float Fov = 0;

    public ClientDataStruct()
    {
    }
}
public unsafe struct CmdAliasStruct
{

}
public unsafe struct SequenceEntryStrct
{

}
public unsafe struct ScreenFadeStruct
{

}
public unsafe struct EventArgsStruct
{
}
public unsafe struct PMTraceStruct
{

}

public unsafe struct ModelStruct
{

}

public unsafe struct CVarStruct
{
}
public unsafe struct ClientSpriteStruct
{
}
public unsafe struct ScreenInfoStruct
{

}
public unsafe struct PlayerMoveStruct
{

}

public unsafe struct UserCmdStruct
{
}

public unsafe struct KButtonStruct
{

}
public unsafe struct RefParamStruct
{
}

public unsafe struct ClEntityStruct
{

}

public unsafe struct MStudioEventStruct
{

}

public unsafe struct LocalStateStruct
{

}

public unsafe struct EntityStateStruct
{

}

public unsafe struct WeaponDataStruct
{

}

public unsafe struct NetAdrStruct
{

}

public unsafe struct TempentStruct
{

}

public unsafe struct RStudioInterfaceStruct
{

}

public unsafe struct EngineStudioApiStruct
{

}