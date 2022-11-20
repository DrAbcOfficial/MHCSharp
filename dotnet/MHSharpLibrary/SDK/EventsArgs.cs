using MHSharpLibrary.SDK;

namespace MHSharpLibrary.Event
{
    public delegate void HudEventHandler();
    public delegate int HudInitializeEventHandler(CLEngineFucsStruct EngineFuncs, int iVersion);
    public delegate int HudVidInitEventHandler();
    public delegate int HudRedrawEventHandler(float flTime, int iFrame);
    public delegate int HudUpdateClientDataEventHandler(ClientDataStruct ClientData, float flTime);
    public delegate void HudClientMoveEventHandler(PlayerMoveStruct ppmove, int server);
    public delegate void HudClientMoveInitEventHandler(PlayerMoveStruct ppmove);
    public delegate byte HudTextureTypeEventHandler(string name);
    public delegate void HudInMonseEventEventHandler(int mstate);
    public delegate void HudClCreateMoveEventHandler(float frametime, UserCmdStruct cmd, int active);
    public delegate int HudIsThirdPersonEventHandler();
    public delegate void HudClGetCameraOffsetEventHandler(float[] args);
    public delegate KButtonStruct HudKBFindEventHandler(string name);
    public delegate void HudCalcRefEventHandler(RefParamStruct pparams);
    public delegate int HudAddEntityEventHandler(int type, ClEntityStruct ent, string modelname);
    public delegate void HudStudioEventHandler(MStudioEventStruct StudioEvent, ClEntityStruct Ent);
    public delegate void HudPostRunCmdEventHandler(LocalStateStruct from, LocalStateStruct to, UserCmdStruct cmd, int runfuncs, double time, uint random_seed);
    public delegate void HudTxferLocalOverridesEventHandler(EntityStateStruct state, ClientDataStruct client);
    public delegate void HudProcessPlayerStateEventHandler(EntityStateStruct det, EntityStateStruct src);
    public delegate void HudTxferPredictionDataEventHandler(EntityStateStruct ps, EntityStateStruct pps, ClientDataStruct pcd, WeaponDataStruct wd, WeaponDataStruct pwd);
    public delegate void HudDemoReadEventHandler(int szie, byte[] buffer);
    public delegate int HudConnectionlessEventHandler(NetAdrStruct net_from, string args, byte[] response_buffer, int response_buffer_size);
    public delegate int HudGetHullBoundsEventHandler(int hullnumber, float[] mins, float[] maxs);
    public delegate void HudFrameEventHandler(double frameTime);
    public delegate int HudKeyEventEventHandler(int eventcode, int keynum, string pszCurrentBinding);
    public delegate void HudTempEntUpdateEventHandler(double frametime, double client_time, double cl_gravity, TempentStruct ppTempEntFree, TempentStruct ppTempEntActive, Func<ClEntityStruct, int> Callback_AddVisibleEntity, Action<TempentStruct,float> Callback_TempEntPlaySound);
    public delegate ClEntityStruct HudGetUserEntityEventHandler(int index);
    public delegate void HudVoiceStatusEventHandler(int entindex, int bTalking);
    public delegate void HudDirectorMessageEventHandler(int iSize, byte[] pbuf);
    public delegate int HudStudioInterFaceEventHandler(int version, RStudioInterfaceStruct ppinterface, EngineStudioApiStruct pstudio);
    public delegate void HudChatInputPositionEventHandler(int x, int y);
    public delegate int HudGetPayerTeamEventHandler(int iplayer);

    public class CExportEvents
    {
        public HudInitializeEventHandler? Initialize = null;
        public HudEventHandler? HudInit = null;
        public HudVidInitEventHandler? HudVidInit = null;
        public HudRedrawEventHandler? HudReDraw = null;
        public HudUpdateClientDataEventHandler? HudUpdateClientData = null;
        public HudEventHandler? HudReset = null;
        public HudClientMoveEventHandler? HudClientMove = null;
        public HudClientMoveInitEventHandler? HudClientMoveInit = null;
        public HudTextureTypeEventHandler? HudTextureType = null;
        public HudEventHandler? InActivateMouse = null;
        public HudEventHandler? InDeactivateMouse = null;
        public HudInMonseEventEventHandler? InMouseEvent = null;
        public HudEventHandler? InClearStates = null;
        public HudEventHandler? InAccumulate = null;
        public HudClCreateMoveEventHandler? ClCreateMove = null;
        public HudIsThirdPersonEventHandler? IsThridPerson = null;
        public HudClGetCameraOffsetEventHandler? ClCameraOffset = null;
        public HudKBFindEventHandler? KbFind = null;
        public HudEventHandler? CamThink = null;
        public HudCalcRefEventHandler? VCalcRefDef = null;
        public HudAddEntityEventHandler? HudAddEntity = null;
        public HudEventHandler? HudCreateEntities = null;
        public HudEventHandler? HudDrawNormalTriangles = null;
        public HudEventHandler? HudDrawTransparentTrianges = null;
        public HudStudioEventHandler? HudStudioEvent = null;
        public HudPostRunCmdEventHandler? HudPostRunCmd = null;
        public HudEventHandler? HudShutDown = null;
        public HudTxferLocalOverridesEventHandler? HudTxferLocalOverrides = null;
        public HudProcessPlayerStateEventHandler? HudProcessPlayerState = null;
        public HudTxferPredictionDataEventHandler? HudTxferPredictionData = null;
        public HudDemoReadEventHandler? DemoReadBuffer = null;
        public HudConnectionlessEventHandler? HudConnectionlessPack = null;
        public HudGetHullBoundsEventHandler? HudGetHullBounds = null;
        public HudFrameEventHandler? HudFrame = null;
        public HudKeyEventEventHandler? HudKeyEvent = null;
        public HudTempEntUpdateEventHandler? HudTempEntUpdate = null;
        public HudGetUserEntityEventHandler? HudGetUserEntity = null;
        public HudVoiceStatusEventHandler? HudVoiceStatus = null;
        public HudDirectorMessageEventHandler? HudDirectorMessage = null;
        public HudStudioInterFaceEventHandler? HudGetStudioModelInterface = null;
        public HudChatInputPositionEventHandler? HudChatInputPosition = null;
        public HudGetPayerTeamEventHandler? HudGetPayerTeam = null;
        public HudEventHandler? ClientFactory = null;
    }
}
