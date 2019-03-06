using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class MyKeys
{
    /// <summary>
    /// 是否购买了退出黑暗模式
    /// </summary>
    public static bool IsBuyClearDarkness { set; get; }

    /// <summary>
    /// 通关数(就是打到了第几关)
    /// </summary>
    public static int PassMission
    {
        get
        {
            //return PlayerPrefs.GetInt("Pass_Mission", 0);
            return PlayerPrefs.GetInt("Pass_Mission", 25);
        }
        set
        {
            PlayerPrefs.SetInt("Pass_Mission", 25);
            //if (value > PassMission)
            //{
            //    PlayerPrefs.SetInt("Pass_Mission", value);
            //}
        }
    }
    /// <summary>
    /// 当前正在玩的关卡
    /// </summary>
    public static int NowPlayingMission { set; get; }

    #region 自定义数据

    /// <summary>
    /// 当前最大关卡数
    /// </summary>
    public static int MissionMaxIndex
    {
        get { return 25; }
    }

    /// <summary>
    /// 复活花费10钻石
    /// </summary>
    public static int ResurgenceCost
    {
        get { return 10; }
    }
    /// <summary>
    /// 购买体力花费5钻石
    /// </summary>
    public static int BuyPhysicalPowerCost
    {
        get { return 5; }
    }
    /// <summary>
    /// 购买冲刺花费20钻石
    /// </summary>
    public static int BuyDashCost
    {
        get { return 20; }
    }
    /// <summary>
    /// 购买护盾花费10钻石
    /// </summary>
    public static int BuyProtectCost
    {
        get { return 10; }
    }
    /// <summary>
    /// 自动回复体力的最大值
    /// </summary>
    public static int Power_Replay_Max
    {
        get { return 5; }
    }
    /// <summary>
    /// 回复每点体力需要的时间,单位：分钟
    /// </summary>
    public static int Power_Replay_Time
    {
        get { return 5; }
    }


    #endregion

    private static bool _pause_Game = false;
    /// <summary>
    /// 游戏暂停
    /// </summary>
    public static bool Pause_Game
    {
        get { return _pause_Game; }
        set
        {
            _pause_Game = value;
            try
            {
                if (HumanManager.Nature.HumanManager_Script.CurrentState != HumanState.Dead)
                {
                    if (_pause_Game)
                    {
                        HumanManager.Nature.Human_Ani.speed = 0;
                    }
                    else
                    {
                        HumanManager.Nature.Human_Ani.speed = 1;
                    }
                }
            }
            catch (Exception)
            {
            }

        }
    }
    /// <summary>
    /// 游戏中的分数
    /// </summary>
    public static int Game_Score { set; get; }

    private static string _top_Score;
    public static string Top_Score_Key
    {
        get
        {
            return _top_Score;
        }

        set
        {
            _top_Score = "top_Score" + value;
        }
    }
    /// <summary>
    /// 记录最高分
    /// </summary>
    public static int Top_Score
    {
        get
        {
            return PlayerPrefs.GetInt(Top_Score_Key, 0);
        }

        set
        {
            int temp = PlayerPrefs.GetInt(Top_Score_Key, 0);

            if (temp < value)
            {
                PlayerPrefs.SetInt(Top_Score_Key, value);
            }
        }
    }

    private static StringBuilder _missionName = new StringBuilder("");
    /// <summary>
    /// 当前关卡名称
    /// </summary>
    public static StringBuilder MissionName {
        get{ return _missionName; }
        set { _missionName = value; } }
    /// <summary>
    /// 关卡星数
    /// </summary>
    public static int PassStars
    {
        get
        {
            if (!PlayerPrefs.HasKey(MissionName.ToString()))
            {
                PlayerPrefs.SetInt(MissionName.ToString(), 0);

            }
            return PlayerPrefs.GetInt(MissionName.ToString());
        }
        set
        {
            PlayerPrefs.SetInt(MissionName.ToString(), value);
        }
    }

    private static StringBuilder StarsMaxKey
    {
        get { return new StringBuilder(MissionName + "Max"); }
    }
    /// <summary>
    /// 通过此关的最大星数
    /// </summary>
    public static int PassStarsMax
    {
        get
        {
            if (!PlayerPrefs.HasKey(StarsMaxKey.ToString()))
            {
                PlayerPrefs.SetInt(StarsMaxKey.ToString(), 0);

            }
            return PlayerPrefs.GetInt(StarsMaxKey.ToString());
        }
        set
        {
            if (value > PassStarsMax)
            {
                PlayerPrefs.SetInt(StarsMaxKey.ToString(), value);
            }
        }
    }
    /// <summary>
    /// 用于关卡中获取星级
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static int GetStarsMax(string name)
    {
        return PlayerPrefs.GetInt(new StringBuilder(name + "Max").ToString());
    }
    /// <summary>
    /// 把关卡名转换成数字
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static int GetNameToInt(string name)
    {
        int mark = 0;
        try
        {
            Regex temp = new Regex(@"\d{1,}");
            string a = temp.Match(name).Value;
            mark = Int32.Parse(a);
        }
        catch (Exception)
        {
            Debug.Log("选关界面，关卡名称不对");
        }
        return mark;
    }

    #region 商店
    /// <summary>
    /// 钻石文本对象
    /// </summary>
    public static Text Diamond_Text { get; set; }
    /// <summary>
    /// 金币文本对象
    /// </summary>
    public static Text Gold_Text { get; set; }
    /// <summary>
    /// 体力文本对象
    /// </summary>
    public static Text Physical_Power_Text { get; set; }

    static string Physical_Power_Key = "Physical_Power";// 玩家偏好：体力关键字
    /// <summary>
    /// 玩家偏好：体力数值
    /// </summary>
    public static int Physical_Power_Value
    {
        set
        {
            if (value >= 0 && value < Power_Replay_Max)
            {
                PlayerPrefs.SetInt(Physical_Power_Key, value);
                ChangeNumber(Physical_Power_Text, value);
            }
            else
            {
                PlayerPrefs.SetInt(Physical_Power_Key, Power_Replay_Max);
                ChangeNumber(Physical_Power_Text, Power_Replay_Max);
                TimeManager.NotReplayTotalTime = 0;
            }
        }
        get
        {
            return PlayerPrefs.GetInt(Physical_Power_Key, Power_Replay_Max);
        }
    }

    static string Gold_Key = "Gold";// 玩家偏好：金币关键字
    /// <summary>
    /// 玩家偏好：金币数值
    /// </summary>
    public static int Gold_Value
    {
        set
        {
            int temp = 0;
            if (value > 99999)
            {
                temp = 99999;
            }
            else
            {
                temp = value;
            }

            if (value < Gold_Value)
            {
                UMManager.Event(EventID.Gold_Cost, (Gold_Value - temp).ToString());
            }
            PlayerPrefs.SetInt(Gold_Key, temp);
            ChangeNumber(Gold_Text, temp);
            UMManager.Event(EventID.Gold_Now, temp.ToString());
        }
        get
        {
            return PlayerPrefs.GetInt(Gold_Key, 0);
        }
    }


    static string Diamond_Key = "Diamond";// 玩家偏好：钻石关键字
    /// <summary>
    /// 玩家偏好：钻石数值
    /// </summary>
    public static int Diamond_Value
    {
        set
        {
            int temp = 0;
            if (value > 99999)
            {
                temp = 99999;
            }
            else
            {
                temp = value;
            }

            if (value < Diamond_Value)
            {
                UMManager.Event(EventID.Diamond_Cost, (Diamond_Value - temp).ToString());
            }
            PlayerPrefs.SetInt(Diamond_Key, temp);
            ChangeNumber(Diamond_Text, temp);
            UMManager.Event(EventID.Diamond_Now, temp.ToString());
        }
        get
        {
            return PlayerPrefs.GetInt(Diamond_Key, 0);
        }
    }


    //改变显示的数字（仅在UI界面中执行）
    static void ChangeNumber(Text text, int number)
    {
        if (SceneManager.GetActiveScene().name.Equals("UI"))
        {
            if (text)
            {
                text.text = number.ToString();
            }
        }
    }
    #endregion
    #region 时间

    /// <summary>
    /// 冲刺每次升级增加的时间
    /// </summary>
    public static float Dash_Time_Add_Once
    {
        get { return 0.5f; }
    }
    /// <summary>
    /// 冲刺每次升级增加的时间
    /// </summary>
    public static float Protect_Time_Add_Once
    {
        get { return 1; }
    }
    /// <summary>
    /// 免费人物每次升级增加的时间（冲刺，盾牌除外）
    /// </summary>
    public static float Free_Hero_Time_Add_Once
    {
        get { return 0.6f; }
    }

    static string Time_Day_Key = "Time_Day_Key";
    public static int Time_Day_Value
    {
        set
        {
            PlayerPrefs.SetInt(Time_Day_Key, value);
        }
        get
        {
            return PlayerPrefs.GetInt(Time_Day_Key, 0);
        }
    }
    static string Time_Hour_Key = "Time_Hour_Key";
    public static int Time_Hour_Value
    {
        set
        {
            PlayerPrefs.SetInt(Time_Hour_Key, value);
        }
        get
        {
            return PlayerPrefs.GetInt(Time_Hour_Key, 0);
        }
    }
    static string Time_Minute_Key = "Time_Minute_Key";
    public static int Time_Minute_Value
    {
        set
        {
            PlayerPrefs.SetInt(Time_Minute_Key, value);
        }
        get
        {
            return PlayerPrefs.GetInt(Time_Minute_Key, 0);
        }
    }
    static string Time_Second_Key = "Time_Second_Key";
    public static int Time_Second_Value
    {
        set
        {
            PlayerPrefs.SetInt(Time_Second_Key, value);
        }
        get
        {
            return PlayerPrefs.GetInt(Time_Second_Key, 0);
        }
    }
    #endregion
    #region 人物属性

    public enum CurrentHero
    {
        GuiYuZi,
        CiShen,
        YuZi,
        ShouSi
    }
    /// <summary>
    /// 多少级以内花费的是金币
    /// </summary>
    public static int Level_Cost_Change_Point
    {
        get { return 26; }
    }

    /// <summary>
    /// 每级花费的钻石数
    /// </summary>
    public static int Level_Cost_Diamond
    {
        get { return 20; }
    }

    /// <summary>
    /// 升满级花费
    /// </summary>
    public static int Up_Max_Cost
    {
        get { return 90; }
    }

    static string CurrentSelectedHero_Key = "CurrentSelectedHero_Level";
    /// <summary>
    /// 当前选择的角色
    /// </summary>
    public static CurrentHero CurrentSelectedHero
    {
        set
        {
            PlayerPrefs.SetInt(CurrentSelectedHero_Key, (int)value);
        }
        get
        {
            return (CurrentHero)PlayerPrefs.GetInt(CurrentSelectedHero_Key, 0);
        }
    }

    /// <summary>
    /// 判断当前英雄是否满级
    /// </summary>
    /// <returns></returns>
    public static bool JudgeLevelMax()
    {
        switch (CurrentSelectedHero)
        {
            case CurrentHero.GuiYuZi:
                if (GuiYuZi_Level_Value >= GuiYuZiLevelMax)
                {
                    GuiYuZi_Level_Value = GuiYuZiLevelMax;
                    return true;
                }
                else
                {
                    return false;
                }
            case CurrentHero.CiShen:
                if (CiShen_Level_Value >= CiShenLevelMax)
                {
                    CiShen_Level_Value = CiShenLevelMax;
                    return true;
                }
                else
                {
                    return false;
                }
            case CurrentHero.YuZi:
                if (YuZi_Level_Value >= YuZiLevelMax)
                {
                    YuZi_Level_Value = YuZiLevelMax;
                    return true;
                }
                else
                {
                    return false;
                }
            case CurrentHero.ShouSi:
                if (ShouSi_Level_Value >= ShouSiLevelMax)
                {
                    ShouSi_Level_Value = ShouSiLevelMax;
                    return true;
                }
                else
                {
                    return false;
                }
        }
        return false;
    }

    /// <summary>
    /// 判断当前英雄是否已经购买
    /// </summary>
    /// <returns></returns>
    public static bool JudgeWhetherBuy(CurrentHero current_Hero)
    {
        switch (current_Hero)
        {
            case CurrentHero.GuiYuZi:
                return true;
            case CurrentHero.CiShen:
                if (CiShen_Buy > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case CurrentHero.YuZi:
                if (YuZi_Buy > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case CurrentHero.ShouSi:
                if (ShouSi_Buy > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
        return false;
    }

    //获取人物等级
    public static int GetHeroLevel(CurrentHero current_Hero)
    {
        int hero_Level = 0;
        switch (current_Hero)
        {
            case CurrentHero.GuiYuZi:
                hero_Level = GuiYuZi_Level_Value;
                break;
            case CurrentHero.CiShen:
                hero_Level = CiShen_Level_Value;
                break;
            case CurrentHero.YuZi:
                hero_Level = YuZi_Level_Value;
                break;
            case CurrentHero.ShouSi:
                hero_Level = ShouSi_Level_Value;
                break;
        }
        return hero_Level;
    }

    #region GuiYuZi（免费）
    static string GuiYuZi_Level_Key = "GuiYuZi_Level";
    /// <summary>
    /// 鲑鱼籽等级
    /// </summary>
    public static int GuiYuZi_Level_Value
    {
        set
        {
            if (value <= GuiYuZiLevelMax)
            {
                PlayerPrefs.SetInt(GuiYuZi_Level_Key, value);
            }

        }
        get
        {
            return PlayerPrefs.GetInt(GuiYuZi_Level_Key, 0);
        }
    }
    public static int GuiYuZiLevelMax
    {
        get { return 50; }
    }
    #endregion

    #region CiShen
    static string CiShen_Level_Key = "CiShen_Level";
    /// <summary>
    /// 刺身等级
    /// </summary>
    public static int CiShen_Level_Value
    {
        set
        {
            if (value <= CiShenLevelMax)
            {
                PlayerPrefs.SetInt(CiShen_Level_Key, value);
            }
        }
        get
        {
            return PlayerPrefs.GetInt(CiShen_Level_Key, 0);
        }
    }
    /// <summary>
    /// 刺身最大等级
    /// </summary>
    public static int CiShenLevelMax
    {
        get { return 50; }
    }

    /// <summary>
    /// 刺身售价。单位RMB
    /// </summary>
    public static int CiShenPrice
    {
        get { return 6; }
    }
    static string CiShen_Buy_Key = "CiShen_Buy_Key";
    /// <summary>
    /// 刺身是否购买标志位,0代表未购买，1代表已购买
    /// </summary>
    public static int CiShen_Buy
    {
        set
        {
            if (CiShen_Buy > 0)
                return;
            PlayerPrefs.SetInt(CiShen_Buy_Key, value);
        }
        get { return PlayerPrefs.GetInt(CiShen_Buy_Key, 0); }
    }
    #endregion

    #region ShouSi
    static string ShouSi_Level_Key = "ShouSi_Level";
    /// <summary>
    /// 寿司忍者等级
    /// </summary>
    public static int ShouSi_Level_Value
    {
        set
        {
            if (value <= ShouSiLevelMax)
            {
                PlayerPrefs.SetInt(ShouSi_Level_Key, value);
            }

        }
        get
        {
            return PlayerPrefs.GetInt(ShouSi_Level_Key, 0);
        }
    }
    public static int ShouSiLevelMax
    {
        get { return 50; }
    }
    /// <summary>
    /// 寿司忍者售价，单位RMB
    /// </summary>
    public static int ShouSiPrice
    {
        get { return 15; }
    }
    static string ShouSi_Buy_Key = "ShouSi_Buy_Key";
    /// <summary>
    /// 寿司忍者是否购买标志位,0代表未购买，1代表已购买
    /// </summary>
    public static int ShouSi_Buy
    {
        set
        {
            if (ShouSi_Buy > 0)
                return;
            PlayerPrefs.SetInt(ShouSi_Buy_Key, value);
        }
        get { return PlayerPrefs.GetInt(ShouSi_Buy_Key, 0); }
    }
    #endregion

    #region YuZi
    static string YuZi_Level_Key = "Yuzi_Level";
    /// <summary>
    /// 玉子等级
    /// </summary>
    public static int YuZi_Level_Value
    {
        set
        {
            if (value <= YuZiLevelMax)
            {
                PlayerPrefs.SetInt(YuZi_Level_Key, value);
            }

        }
        get
        {
            return PlayerPrefs.GetInt(YuZi_Level_Key, 0);
        }
    }
    public static int YuZiLevelMax
    {
        get { return 50; }
    }
    /// <summary>
    /// 玉子售价，单位RMB
    /// </summary>
    public static int YuZiPrice
    {
        get { return 6; }
    }
    static string YuZi_Buy_Key = "YuZi_Buy_Key";
    /// <summary>
    /// 玉子是否购买标志位,0代表未购买，1代表已购买
    /// </summary>
    public static int YuZi_Buy
    {
        set
        {
            if (YuZi_Buy > 0)
                return;
            PlayerPrefs.SetInt(YuZi_Buy_Key, value);
        }
        get { return PlayerPrefs.GetInt(YuZi_Buy_Key, 0); }
    }
    #endregion


    
    #endregion
}
