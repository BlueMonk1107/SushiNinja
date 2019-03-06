using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static string _mission_Name;
    public static string MissionName
    {
        set { _mission_Name = value; }
        get { return _mission_Name; }
    }
    private int _pauseTime;//游戏到后台以后，程序暂停会执行两次，这个变量是为了计数
    private bool _whether_Start_After_Buy;//购买体力之后是否开始游戏

    // Use this for initialization
    void Start()
    {
        //测试代码
        TestCode();

        if (MainUITool.Instance.TheActiveStack.Count != 0 && SceneManager.GetActiveScene().name.Equals("UI"))
        {
            WindowID window = MainUITool.Instance.TheActiveStack.Peek();
            ActiveWindow(window);
        }
        //多点触控开关
        Input.multiTouchEnabled = false;

        _whether_Start_After_Buy = false;
    }

    //测试代码
    void TestCode()
    {
        Debug.Log("开启了测试代码");
        //Screen.SetResolution(480, 800, false);

        //PlayerPrefs.DeleteAll();
        //GuidanceBase.GuidanceMark = 4;
        //MyKeys.PassMission = 25;
        //MyKeys.CiShen_Level_Value = 24;

        //MyKeys.GuiYuZi_Level_Value = 0;
        //MyKeys.NowPlayingMission = 1;

        //StartCoroutine(Change());
        //MyKeys.YuZi_Buy = 1;
        //MyKeys.ShouSi_Buy = 1;
        //MyKeys.CiShen_Buy = 1;
    }

    IEnumerator Change()
    {
        yield return new WaitForSeconds(1);
        MyKeys.Gold_Value = 10000;
        MyKeys.Diamond_Value = 10000;
        MyKeys.Physical_Power_Value = 5;
    }

    void Update()
    {
        //if (Time.frameCount % 20 == 0) { GC.Collect(); }

        if ((Application.platform == RuntimePlatform.Android) && Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    /// <summary>
    /// 点击UI调用的方法
    /// </summary>
    /// <param name="x"></param>
    public void TheClicked(int x)
    {
        if (global::StartGame.First_In)
            return;

        MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);
        WindowID temp = (WindowID)x;
        MainUITool.Instance.SetTheActive(temp);
    }

    /// <summary>
    /// 支付人民币购买钻石
    /// </summary>
    /// <param name="x"></param>
    public void RMB(int index)
    {
        int RMB = ShopData.DiamondShopCost[index];
        bool isSucceed = PayRMB.Instance.PayRMBFun(RMB);
        if (isSucceed)
        {
            RMBToDiamond(RMB);
        }
    }
    /// <summary>
    /// 支付人民币购买钻石
    /// </summary>
    private void RMBToDiamond(int RMB)
    {
        try
        {
            MyKeys.Diamond_Value += ShopData.DiamondShop[RMB];
        }
        catch (Exception)
        {
            Debug.Log("未设定此支付数值");
        }

    }

    /// <summary>
    /// 支付钻石，购买金币
    /// </summary>
    public void DiamondToGold(int index)
    {
        int diamond = ShopData.GoldShopCost[index];
        if (MyKeys.Diamond_Value > diamond)
        {
            MyAudio.PlayAudio(StaticParameter.s_Buy, false, StaticParameter.s_Buy_Volume);
            MyKeys.Diamond_Value -= diamond;
            MyKeys.Gold_Value += GetGoldNumber(diamond);
        }
        else
        {
            ActiveWindow(WindowID.WindowID_ShopDiamond);
        }
    }
    /// <summary>
    /// 礼包，0是新手礼包，1是超值礼包
    /// </summary>
    public void Gift(int mark)
    {
        bool isPay = false;
        switch (mark)
        {
            case 0:
                isPay = PayRMB.Instance.PayRMBFun(ShopData.NoviceGiftCost);

                if (isPay)
                {
                    global::Gift.PaySucceedToSet = 0;
                    MyKeys.Gold_Value += ShopData.NoviceGiftGet[0];
                    MyKeys.Diamond_Value += ShopData.NoviceGiftGet[1];
                }
                break;
            case 1:
                isPay = PayRMB.Instance.PayRMBFun(ShopData.SuperGiftCost);

                if (isPay)
                {
                    global::Gift.PaySucceedToSet = 1;
                    MyKeys.Gold_Value += ShopData.SuperGiftGet[0];
                    MyKeys.Diamond_Value += ShopData.SuperGiftGet[1];
                    MyKeys.YuZi_Buy = 1;
                }
                break;
            default:
                Debug.Log("礼包输入参数错误");
                break;
                
        }
        Return();
    }

    int GetGoldNumber(int diamond)
    {
        int gold = 0;
        try
        {
            gold = ShopData.GoldShop[diamond];
        }
        catch (Exception)
        {
            Debug.Log("未设定此支付数值");
        }
        return gold;
    }

   
    /// <summary>
    /// 支付金币，购买体力
    /// </summary>
    public void DiamondToPower()
    {
        MyAudio.PlayAudio(StaticParameter.s_Buy, false, StaticParameter.s_Buy_Volume);
        //用5钻石购买1体力
        MainUITool.Instance.Business(2);
        if (_whether_Start_After_Buy)
        {
            StartGame();
        }
        Return();
    }
    /// <summary>
    /// 开始前购买道具
    /// </summary>
    /// <param name="x"></param>
    public void BuyItems(int x)
    {
        Items temp = (Items)x;
        if (Judge(temp))
        {
            //加入购买的道具
            BuyTheItems.AddItems = temp;
            //购买后按键的变化
            MainUITool.Instance.ItemsButton(EventSystem.current.currentSelectedGameObject);
            //友盟统计数据
            UMManager.BuyItem(temp.ToString(), 1, 0);
            UMManager.Event(EventID.BuyItem, temp+" "+MyKeys.MissionName);
        }

    }

    bool Judge(Items items)
    {
        int gold;
        int diamond;
        ItemCost(items, out gold, out diamond);
        if (gold > 0)
        {
            if (MyKeys.Gold_Value >= gold)
            {
                MyAudio.PlayAudio(StaticParameter.s_Buy, false, StaticParameter.s_Buy_Volume);
                MyKeys.Gold_Value -= gold;
                return true;
            }
            else
            {
                MyAudio.PlayAudio(StaticParameter.s_No, false, StaticParameter.s_No_Volume);
                return false;
            }
        }
        else
        {
            if (MyKeys.Diamond_Value >= diamond)
            {
                MyAudio.PlayAudio(StaticParameter.s_Buy, false, StaticParameter.s_Buy_Volume);
                MyKeys.Diamond_Value -= diamond;
                return true;
            }
            else
            {
                MyAudio.PlayAudio(StaticParameter.s_No, false, StaticParameter.s_No_Volume);
                return false;
            }
        }
    }
    void ItemCost(Items items, out int gold, out int diamond)
    {
        gold = 0;
        diamond = 0;

        try
        {
            gold = ShopData.ItemShop[items][0];
            diamond = ShopData.ItemShop[items][1];
        }
        catch (Exception)
        {
            Debug.Log("未设定此支付数值");
        }
        
    }

    /// <summary>
    /// 代码中切换界面的方法
    /// </summary>
    /// <param name="x"></param>
    public static void ActiveWindow(WindowID x)
    {
        MainUITool.Instance.SetTheActive(x);
    }
    /// <summary>
    /// 返回按钮调用方法
    /// </summary>
    public void Return()
    {
        MyAudio.PlayAudio(StaticParameter.s_Cancel, false, StaticParameter.s_Cancel_Volume);
        if (!SceneManager.GetActiveScene().name.Equals("UI"))
        {
            //GameUITool.Instance.Prefab_Dictionary[GameUITool.Instance.TheActiveStack.Pop()].SetActive(false);
            //GameUITool.Instance.Prefab_Dictionary[GameUITool.Instance.TheActiveStack.Peek()].SetActive(true);
            //GameUITool.Instance.Prefab_Dictionary[GameUI.GameUI_MyMask].SetActive(false);
            //GameUITool.Instance.SetTheActive(GameUI.GameUI_DuMiao);

            //Debug.Log(MainUITool.Instance.TheActiveStack.Peek());
            //MainUITool.Instance.Prefab_Dictionary[MainUITool.Instance.TheActiveStack.Pop()].SetActive(false);
            //MainUITool.Instance.Prefab_Dictionary[WindowID.WindowID_MyMask].SetActive(false);

            GameUITool.Instance.Return();
        }
        else
        {
            MainUITool.Instance.ReturnPrevious();
        }

    }

    public void StartGame()
    {
        MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);

        if (MyKeys.Physical_Power_Value > 0)
        {
            _whether_Start_After_Buy = false;
            //体力大于0，直接开始加载场景
            //开始游戏减去一点体力，通过游戏会加一点体力
            TimeManager.ReducePhysicalPower();
            //获取点击关卡名称并保存
            int now_Mission = int.Parse(MissionName);
            MyKeys.NowPlayingMission = now_Mission;
            int index = now_Mission + 1;
            //显示加载界面
            Loading(index);
        }
        else
        {
            //体力小于0
            //在主UI界面，弹出购买体力框
            //在游戏界面，关闭当前ui后，弹出购买体力框
            if (!SceneManager.GetActiveScene().name.Equals("UI"))
            {
                GameUITool.Instance.Prefab_Dictionary[GameUITool.Instance.TheActiveStack.Peek()].SetActive(false);
                GameUITool.Instance.Prefab_Dictionary[GameUI.GameUI_MyMask].SetActive(false);
                MainUITool.Instance.SetTheActive(WindowID.WindowID_MyMask);
            }
            ActiveWindow(WindowID.WindowID_ShopPower);
            _whether_Start_After_Buy = true;
        }
    }

    void Loading(int index)
    {
        GameObject temp = Resources.Load(UIResourceDefine.MainUIPrefabPath + "Loading") as GameObject;
        GameObject copy = Instantiate(temp);
        copy.GetComponent<Loading>().SceneIndex = index;
        if (SceneManager.GetActiveScene().name.Equals("UI"))
        {
            while (MainUITool.Instance.TheActiveStack.Count>1)
            {
                MainUITool.Instance.Prefab_Dictionary[MainUITool.Instance.TheActiveStack.Pop()].SetActive(false);
            }
            try
            {
                MainUITool.Instance.Prefab_Dictionary[WindowID.WindowID_MyMask].SetActive(false);
            }
            catch (Exception)
            {
                
            }
            
        }
        else
        {
            while (GameUITool.Instance.TheActiveStack.Count>1)
            {
                GameUITool.Instance.Prefab_Dictionary[GameUITool.Instance.TheActiveStack.Pop()].SetActive(false);
            }
            GameUITool.Instance.Prefab_Dictionary[GameUI.GameUI_MyMask].SetActive(true);
        }
    }

    public void Load(string scene_Name)
    {
        MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);
        SceneManager.LoadScene(scene_Name);
    }
    public void Load(int scene_Index)
    {
        MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);
        SceneManager.LoadScene(scene_Index);
    }
    public void Load()
    {
        MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);
        SceneManager.LoadScene("UI");
    }
    public void LoadMap()
    {
        MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);
        MainUITool.Instance.SetMapActive();
        SceneManager.LoadScene("UI");
    }

    void OnDestroy()
    {
        MainUITool.Instance.TheActiveStack.Clear();
        MainUITool.Instance.Prefab_Dictionary.Clear();
    }

    void OnApplicationPause()
    {
        //第一次进游戏时会执行一次这个函数，排除这次计数
        if (global::StartGame.First_In)
            return;

        _pauseTime++;
        //在退出时保存时间
        if (_pauseTime == 1)
        {
            MyKeys.Time_Day_Value = DateTime.Now.Day;
            MyKeys.Time_Hour_Value = DateTime.Now.Hour;
            MyKeys.Time_Minute_Value = DateTime.Now.Minute;
            MyKeys.Time_Second_Value = DateTime.Now.Second;
        }
        if (_pauseTime == 2)
        {
            if (!SceneManager.GetActiveScene().name.Equals("UI") 
                && HumanManager.Nature.HumanManager_Script.CurrentState != HumanState.Dead
                && GuidanceBase.GuidanceMark>0)
            {
                GameUI current_ID = GameUITool.Instance.TheActiveStack.Peek();
                if (UIResourceDefine.gameUILayer[GameUI.GameUI_Pause] != UIResourceDefine.gameUILayer[current_ID])
                {
                    MyKeys.Pause_Game = true;
                    GameUITool.Instance.SetTheActive(GameUI.GameUI_Pause);
                }
            }

            _pauseTime = 0;
        }
    }

    void OnApplicationQuit()
    {
        MyKeys.Time_Day_Value = DateTime.Now.Day;
        MyKeys.Time_Hour_Value = DateTime.Now.Hour;
        MyKeys.Time_Minute_Value = DateTime.Now.Minute;
        MyKeys.Time_Second_Value = DateTime.Now.Second;

        UMManager.GameEnd();
    }
}
