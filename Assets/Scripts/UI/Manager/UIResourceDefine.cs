using System.Collections.Generic;


/// <summary>
/// 每一个UI界面的唯一标识
/// </summary>
public enum WindowID
{
    WindowID_MainMenu = 0,  //主菜单
    WindowID_MissionPreparation,//关卡模式准备界面
    Window_Preparation,  //比赛模式准备界面
    WindowID_ShopDiamond,   //钻石商店
    WindowID_ShopGold,     //金币商店
    WindowID_ShopPower,      //体力商店
    WindowID_MyMask,        //遮罩层
    WindowID_SelectMission, //选关界面
    WindowID_SelectHero,     //选人界面
    WindowID_Help,     //帮助界面
    WindowID_NoviceGift,//新手礼包
    WindowID_SuperGift,//超值礼包
}
/// <summary>
/// 游戏中UI界面的唯一标识
/// </summary>
public enum GameUI
{
    GameUI_Failure,        //失败
    GameUI_Pass,       //过关
    GameUI_MainUI,        //游戏界面的UI
    GameUI_Pause,  //游戏中暂停
    GameUI_Seconds,        //读秒界面
    GameUI_Resurgence,         //复活
    GameUI_MyMask,        //遮罩层
    GameUI_ShopDiamond,   //钻石商店
    GameUI_ShopGold,   //金币商店
    GameUI_ShowShop       //在游戏界面中显示商店
    
}

/// <summary>
/// UI的显示类型
/// </summary>
public enum UIWindowType
{
    /// <summary>
    /// 可推出界面
    /// </summary>
    Normal,
    /// <summary>
    /// 固定窗口
    /// </summary>
    Fixed,
    /// <summary>
    /// 模式窗口(弹窗)
    /// </summary>
    PopUp,
}

/// <summary>
/// UI在显示时，其他窗口的显示模式
/// </summary>
public enum UIWindowShowMode
{
    DoNothing,
    /// <summary>
    /// 关闭闭其他界面
    /// </summary>
    HideOther,
    /// <summary>
    /// 点击返回按钮关闭当前,不关闭其他界面(需要调整好层级关系)
    /// </summary>
    NeedBack,
    /// <summary>
    /// 关闭TopBar,关闭其他界面,不加入backSequence队列
    /// </summary>
    NoNeedBack,
}

/// <summary>
/// UI预制的地址类
/// </summary>
public class UIResourceDefine
{
    /// <summary>
    /// 预制的名称字典
    /// </summary>
    public static Dictionary<WindowID, string> windowPrefabName = new Dictionary<WindowID, string>()
    {
        { WindowID.WindowID_MainMenu, "01-mainUI" },
        { WindowID.WindowID_MissionPreparation, "02-zhunbei" },
        { WindowID.WindowID_SelectMission, "Mission" },
        { WindowID.Window_Preparation, "03-bisaizhunbei" },
        { WindowID.WindowID_ShopDiamond, "09-shop" },
        { WindowID.WindowID_ShopGold,"10-shop2" },
        { WindowID.WindowID_ShopPower, "11-shop3" },
        { WindowID.WindowID_MyMask, "MyMask" },
        { WindowID.WindowID_SelectHero, "chooseHero" },
        { WindowID.WindowID_Help, "Help" },
        { WindowID.WindowID_NoviceGift, "XinShouLiBao" },
        { WindowID.WindowID_SuperGift, "ChaoZhiLiBao" },

    };
    public static Dictionary<GameUI, string> gameUIPrefabName = new Dictionary<GameUI, string>()
    {
        { GameUI.GameUI_Failure, "04-shibai" },
        { GameUI.GameUI_Pass, "05-guoguan" },
        { GameUI.GameUI_MainUI, "07-ingame" },
        { GameUI.GameUI_Pause, "08-youxizanting" },
        { GameUI.GameUI_Seconds, "dumiao" },
        { GameUI.GameUI_Resurgence, "fuhuo" },
        { GameUI.GameUI_MyMask, "MyMask" },
        { GameUI.GameUI_ShopDiamond, "09-shop" },
        { GameUI.GameUI_ShopGold,"10-shop2" },
        { GameUI.GameUI_ShowShop, "ShowShop" }
    };

    public static Dictionary<WindowID, int> windowLayer = new Dictionary<WindowID, int>()
    {
        { WindowID.WindowID_MainMenu, 1 },
        { WindowID.WindowID_MissionPreparation, 1 },
        { WindowID.WindowID_SelectMission, 1 },
        { WindowID.Window_Preparation, 1 },
        { WindowID.WindowID_ShopDiamond, 3 },
        { WindowID.WindowID_ShopGold,3 },
        { WindowID.WindowID_ShopPower, 3 },
        { WindowID.WindowID_MyMask, 2},
        { WindowID.WindowID_SelectHero, 1},
        { WindowID.WindowID_Help, 3 },
        { WindowID.WindowID_NoviceGift, 3},
        { WindowID.WindowID_SuperGift, 3 },
    };
    public static Dictionary<GameUI, int> gameUILayer = new Dictionary<GameUI, int>()
    {
        { GameUI.GameUI_Failure, 12 },
        { GameUI.GameUI_Pass, 12 },
        { GameUI.GameUI_MainUI, 10 },
        { GameUI.GameUI_Pause, 12 },
        { GameUI.GameUI_Seconds, 12 },
        { GameUI.GameUI_Resurgence, 12 },
        { GameUI.GameUI_MyMask, 11 },
        { GameUI.GameUI_ShopDiamond, 12 },
        { GameUI.GameUI_ShopGold,12},
        { GameUI.GameUI_ShowShop, 100}
    };

    /// <summary>
    /// 主界面UI预制体文件夹地址
    /// </summary>
    public static string MainUIPrefabPath = "UIPrefab/MainUI/";
    /// <summary>
    /// 游戏界面UI预制体文件夹地址
    /// </summary>
    public static string GameUIPrefabPath = "UIPrefab/GameUI/";
}

