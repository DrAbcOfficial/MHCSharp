using MHSharpLibrary.SDK;
using MHSharpLibrary.Event;
using MHSharpLibrary.Util;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MHSharpFrame;

public class MyExportFuncs
{

    //Native call
    public class CExportEvents
    {
        public event HudInitializeEventHandler? Initialize = null;
        public virtual void OnInitialize(CLEngineFucsStruct EngineFuncs, int iVersion)
        {
            Initialize?.Invoke(EngineFuncs, iVersion);
        }
        public event HudEventHandler? HudInit = null;
        public virtual void OnHudInit()
        {
            HudInit?.Invoke();
        }

        public event HudVidInitEventHandler? HudVidInit = null;
        public event HudRedrawEventHandler? HudReDraw = null;
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
    }

    //C# event object
    public static CExportEvents pEvent = new CExportEvents();

    // 单线程同步上下文
    static GameSynchronizationContext? ctx;
    // 保存好的原版函数
    public static ClExportFuncsStruct IExportFuncs;

    public static void Init()
    {
        // 协程初始化
        ctx = new GameSynchronizationContext() { CurrentThread = Thread.CurrentThread };
        SynchronizationContext.SetSynchronizationContext(ctx);
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public unsafe static void HudFrame(double time)
    {
        // 不要删除，这是单线程协程的调度器
        ctx?.Update();
        IExportFuncs.HudFrame(time);

    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public unsafe static void ListPlugins()
    {
        string message = string.Format("|{0,5}|{1,32}|{2,32}|\n","index","plugin name","plugin version");

        for (int i = 0; i < Plugin.sharpPlugins.Count; i++)
        {
            string szVersion = string.Empty;
            if (Plugin.sharpPlugins[i].GetVersion != null)
            {
                object[] args = { };
                string? temp = (string?)Plugin.sharpPlugins[i].GetVersion.Invoke(Plugin.sharpPlugins[i].Handle, args);
                szVersion = temp == null ? string.Empty : temp;
            }
            message += string.Format("|{0,5}|{1,32}|{2,32}|\n", i, Plugin.sharpPlugins[i].Name, szVersion);
        }
        byte* cplain = Utility.GetNativeString(message);
        Plugin.IEngineFucs.ConsolePrint(cplain);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public unsafe static void HudInit()
    {
        string message = "mh_cspluginlist";
        byte* cplain = Utility.GetNativeString(message);
        Plugin.IEngineFucs.AddCommand(cplain, &ListPlugins);

        pEvent.OnHudInit();
        IExportFuncs.HudInit();
    }
}
