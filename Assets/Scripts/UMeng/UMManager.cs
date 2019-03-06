using System;
using Umeng;
using UnityEngine;

public class UMManager : MonoBehaviour
{
    void Awake()
    {
        if (Math.Abs(Time.time) < 1)
        {
            //Analytics.StartWithAppKeyAndChannelId("582aef1cf43e481705000800", "Google Play");
            //GameStart();
        }
    }

    /// <summary>
    /// 进入关卡，开始统计
    /// </summary>
    /// <param name="mission_Name"></param>
    public static void StartMission(string mission_Name)
    {
       // GA.StartLevel(mission_Name);
    }
    /// <summary>
    /// 完成关卡
    /// </summary>
    /// <param name="mission_Name"></param>
    public static void FinishMission(string mission_Name)
    {
       // GA.FinishLevel(mission_Name);
    }
    /// <summary>
    /// 闯关失败
    /// </summary>
    /// <param name="mission_Name"></param>
    public static void FailMission(string mission_Name)
    {
       // GA.FailLevel(mission_Name);
    }
    /// <summary>
    /// 购买道具
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    /// <param name="price"></param>
    public static void BuyItem(string item, int amount, double price)
    {
       // GA.Buy(item, amount, price);
    }
    /// <summary>
    /// 人物等级
    /// </summary>
    /// <param name="level"></param>
    public static void SetHeroLevel(int level)
    {
       // GA.SetUserLevel(level);
    }
    /// <summary>
    /// 自定义事件
    /// </summary>
    /// <param name="eventId"></param>
    public static void Event(string eventId)
    {
       // Analytics.Event(eventId);
    }
    /// <summary>
    /// 自定义事件
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="label"></param>
    public static void Event(string eventId, string label)
    {
       // Analytics.Event(eventId, label);
    }
    /// <summary>
    /// 游戏开始（计时）
    /// </summary>
    public static void GameStart()
    {
       // Analytics.EventBegin(EventID.Game);
    }
    /// <summary>
    /// 游戏结束（计时）
    /// </summary>
    public static void GameEnd()
    {
        //Analytics.EventEnd(EventID.Game);
    }
}
