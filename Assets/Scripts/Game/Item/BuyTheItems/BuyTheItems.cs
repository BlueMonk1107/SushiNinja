using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BuyTheItems {

    static List<Items> _buy_Items_List = new List<Items>();
    public static bool dead_Dash_Bool = false;
    public static bool start_Dash_Bool = false;

    /// <summary>
    /// 加入已经购买的道具
    /// </summary>
    public static Items AddItems
    {
        set
        {
            if (value == Items.StartDash600|| value == Items.StartDash1200)
            {
                start_Dash_Bool = true;
            }
            Buy_Items_List.Add(value);
        }
    }

    /// <summary>
    /// 已经购买的道具的数组
    /// </summary>
    public static List<Items> Buy_Items_List
    {
        get
        {
            return _buy_Items_List;
        }
    }

    /// <summary>
    /// 设置开始前购买的道具效果
    /// </summary>
    public static void SetTheItems()
    {
        for (int i = 0; i < Buy_Items_List.Count; i++)
        {
            SetItem(Buy_Items_List[i]);
        }
       
    }

    static void SetItem(Items items)
    {
        switch (items)
        {
            case Items.Blessing:
                HumanManager.Nature.HumanManager_Script.ItemState = Blessing();
                break;
            case Items.DestroyObstacle:
                DestroySomething("ObstacleManager");
                break;
            case Items.DestroyMissile:
                DestroySomething("MissileManager");
                break;
            case Items.Protect:
                HumanManager.Nature.HumanManager_Script.ItemState = ItemState.Protect;
                break;
            case Items.SuperMan:
                HumanManager.Nature.HumanManager_Script.ItemState = ItemState.SuperMan;
                break;
            case Items.StartDash600:
                ItemColliction.StartDash.Steps += 600;
                HumanManager.Nature.HumanManager_Script.ItemState = ItemState.StartDash;
                break;
            case Items.StartDash1200:
                ItemColliction.StartDash.Steps += 1200;
                HumanManager.Nature.HumanManager_Script.ItemState = ItemState.StartDash;
                break;
            case Items.AfterDeadDash:
                dead_Dash_Bool = true;
                break;
        }
    }

    static ItemState Blessing()
    {
        int temp = Random.Range(1, 6);
        return (ItemState)temp;
    }

    static void BuySomething()
    {
        
    }
    static void DestroySomething(string name)
    {
        GameObject temp = GameObject.FindGameObjectWithTag(name);
        try
        {
            temp.SetActive(false);
        }
        catch (NullReferenceException)
        {

            Debug.Log(name+"为空");
        }
    }
}
