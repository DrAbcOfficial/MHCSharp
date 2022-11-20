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

    public struct ExportEventsPublisherStuct
    {
        public event HudInitializeEventHandler? Initialize = null;
        public void OnInitialize(CLEngineFucsStruct EngineFuncs, int iVersion)
        {
            Initialize?.Invoke(EngineFuncs, iVersion);
        }
        public event HudEventHandler? HudInit = null;
        public void OnHudInit()
        {
            HudInit?.Invoke();
        }
        public event HudVidInitEventHandler? HudVidInit = null;
        public event HudRedrawEventHandler? HudReDraw = null;
        public int? OnHudReDraw(float flTime, int iFrame)
        {
            return HudReDraw?.Invoke(flTime, iFrame);
        }
        public event HudUpdateClientDataEventHandler? HudUpdateClientData = null;
        public event HudEventHandler? HudReset = null;
        public event HudClientMoveEventHandler? HudClientMove = null;
        public event HudClientMoveInitEventHandler? HudClientMoveInit = null;
        public event HudTextureTypeEventHandler? HudTextureType = null;
        public event HudEventHandler? InActivateMouse = null;
        public event HudEventHandler? InDeactivateMouse = null;
        public event HudInMonseEventEventHandler? InMouseEvent = null;
        public event HudEventHandler? InClearStates = null;
        public event HudEventHandler? InAccumulate = null;
        public event HudClCreateMoveEventHandler? ClCreateMove = null;
        public event HudIsThirdPersonEventHandler? IsThridPerson = null;
        public event HudClGetCameraOffsetEventHandler? ClCameraOffset = null;
        public event HudKBFindEventHandler? KbFind = null;
        public event HudEventHandler? CamThink = null;
        public event HudCalcRefEventHandler? VCalcRefDef = null;
        public event HudAddEntityEventHandler? HudAddEntity = null;
        public event HudEventHandler? HudCreateEntities = null;
        public event HudEventHandler? HudDrawNormalTriangles = null;
        public event HudEventHandler? HudDrawTransparentTrianges = null;
        public event HudStudioEventHandler? HudStudioEvent = null;
        public event HudPostRunCmdEventHandler? HudPostRunCmd = null;
        public event HudEventHandler? HudShutDown = null;
        public event HudTxferLocalOverridesEventHandler? HudTxferLocalOverrides = null;
        public event HudProcessPlayerStateEventHandler? HudProcessPlayerState = null;
        public event HudTxferPredictionDataEventHandler? HudTxferPredictionData = null;
        public event HudDemoReadEventHandler? DemoReadBuffer = null;
        public event HudConnectionlessEventHandler? HudConnectionlessPack = null;
        public event HudGetHullBoundsEventHandler? HudGetHullBounds = null;
        public event HudFrameEventHandler? HudFrame = null;
        public event HudKeyEventEventHandler? HudKeyEvent = null;
        public event HudTempEntUpdateEventHandler? HudTempEntUpdate = null;
        public event HudGetUserEntityEventHandler? HudGetUserEntity = null;
        public event HudVoiceStatusEventHandler? HudVoiceStatus = null;
        public event HudDirectorMessageEventHandler? HudDirectorMessage = null;
        public event HudStudioInterFaceEventHandler? HudGetStudioModelInterface = null;
        public event HudChatInputPositionEventHandler? HudChatInputPosition = null;
        public event HudGetPayerTeamEventHandler? HudGetPayerTeam = null;
        public event HudEventHandler? ClientFactory = null;

        public ExportEventsPublisherStuct() { }
    }
}
