using UnityEngine;
using System.Collections;

public static class EventID
{
    public static string Game = "Game";
    //人物状态统计
    public static string Boy_Level = "Boy_Level";
    public static string State_Dead = "State_Dead";              //死亡
    public static string UnitOne_StateDead = "UnitOne_StateDead";//第一单元死亡统计
    public static string State_Resurgence = "State_Resurgence"; //复活
    public static string Buy_Power = "Buy_Power";               //购买体力
    public static string BuyItem = "BuyItem";                   //购买道具
    //虚拟货币统计
    public static string Gold_Now = "Gold_Now";                 //当前金币数
    public static string Diamond_Now = "Diamond_Now";           //当前钻石数
    public static string Gold_Cost = "Gold_Cost";               //金币总量
    public static string Diamond_Cost = "Diamond_Cost";         //钻石总量
    //道具的统计
    public static string InGame_Dash = "InGame_Dash";           //游戏中购买的冲刺道具
    public static string InGame_Protect = "InGame_Protect";     //游戏中购买的护盾道具
}
