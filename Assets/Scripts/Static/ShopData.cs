using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public static class ShopData
{

    private static readonly Dictionary<int, int> _goldShop = new Dictionary<int, int>();
    /// <summary>
    /// 钻石商店花费数组
    /// </summary>
    public static readonly int[] GoldShopCost = {20, 40, 80, 160, 320};
    public static readonly int[] GoldShopGet = { 2000, 4500, 12000, 28000, 66666 };
    /// <summary>
    /// 金币商店数据，第一个参数是钻石数，第二个参数是获取金币数
    /// </summary>
    public static Dictionary<int, int> GoldShop
    {
        get
        {
            if (_goldShop.Count == 0)
            {
                if (GoldShopCost.Length == GoldShopGet.Length)
                {
                    for (int i = 0; i < GoldShopCost.Length; i++)
                    {
                        _goldShop.Add(GoldShopCost[i], GoldShopGet[i]);
                    }
                }
                else
                {
                    Debug.LogError("金币商店数据错误");
                }
            }
            return _goldShop;
        }
    }

    private static readonly Dictionary<int, int> _diamondShop = new Dictionary<int, int>();
    public static readonly int[] DiamondShopCost = { 29, 20, 9, 6, 2 };
    public static readonly int[] DiamondShopGet = {300, 200, 100, 60, 18};
    public static readonly int[] DiamondShopPresent = {300, 100, 20, 0, 0};
    /// <summary>
    /// 钻石商店，第一个参数是花费人民币数，第二个参数是获得钻石数
    /// </summary>
    public static Dictionary<int, int> DiamondShop
    {
        get
        {
            if (_diamondShop.Count == 0)
            {
                if (DiamondShopCost.Length == DiamondShopGet.Length
                    && DiamondShopCost.Length == DiamondShopPresent.Length
                    && DiamondShopGet.Length == DiamondShopPresent.Length)
                {
                    for (int i = 0; i < DiamondShopCost.Length; i++)
                    {
                        _diamondShop.Add(DiamondShopCost[i], DiamondShopGet[i] + DiamondShopPresent[i]);
                    }
                }
                else
                {
                    Debug.LogError("钻石商店数据错误");
                }
            }
            return _diamondShop;
        }
    }

    private static readonly Dictionary<Items, int[]> _itemShop = new Dictionary<Items, int[]>();
    /// <summary>
    /// 准备界面道具花费数据，数组第一个元素是花费的金币数，第二个元素是花费的钻石数
    /// </summary>
    public static Dictionary<Items, int[]> ItemShop
    {
        get
        {
            if (_itemShop.Count == 0)
            {
                _itemShop.Add(Items.Blessing, new []{ 300,0});
                _itemShop.Add(Items.DestroyObstacle, new[] { 0, 18});
                _itemShop.Add(Items.DestroyMissile, new[] { 0, 18});
                _itemShop.Add(Items.Protect, new[] { 400, 0});
                _itemShop.Add(Items.SuperMan, new[] { 0, 48});
                _itemShop.Add(Items.StartDash600, new[] { 1000, 0});
                _itemShop.Add(Items.StartDash1200, new[] { 0, 18});
                _itemShop.Add(Items.AfterDeadDash, new[] { 700, 0});
            }
            return _itemShop;
        }
    }

    private static string[] _itemTips =
    {
        "本关随机获得一个效果",
        "本次进行游戏的关卡将没有障碍",
        "本次进行游戏的关卡将没有导弹",
        "开场自带护盾",
        "开场自带无敌",
        "游戏开始时冲刺 600米",
        "游戏开始时冲刺 1200米",
        "死亡后可继续冲刺一段距离"
    };
    /// <summary>
    /// 道具使用提示
    /// </summary>
    public static string[] ItemTips
    {
        get { return _itemTips; }
    }

    /// <summary>
    /// 查找文本组件通用函数
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <param name="array"></param>
    /// <param name="data"></param>
    public static void FindText(Transform parent, string name, int[] data,string[] dataStrings)
    {
        int arraySize = 0;
        if (data != null)
        {
            arraySize = data.Length;
        }
        else if (dataStrings != null)
        {
            arraySize = dataStrings.Length;
        }
        else
        {
            Debug.LogError("商店数据不能都为空");
            return;
        }

        Text[] testArray = new Text[arraySize];
        Text[] temp = null;
        for (int i = 0; i < arraySize; i++)
        {
            temp = parent.GetChild(i).GetComponentsInChildren<Text>();
            for (int j = 0; j < temp.Length; j++)
            {
                if (temp[j].transform.name.Contains(name))
                {
                    testArray[i] = temp[j];
                    if (data != null)
                    {
                        testArray[i].text = data[i].ToString();
                    }
                    else
                    {
                        testArray[i].text = dataStrings[i];
                    }
                }
            }
        }
    }

    /// <summary>
    /// 新手礼包需要支付的费用，单位：RMB
    /// </summary>
    public static float NoviceGiftCost
    {
        get { return 0.1f; }
    }

    private static int[] _novice_Data = { 1000, 30 };
    /// <summary>
    /// 新手礼包获得资源，第一项是金币，第二项是钻石
    /// </summary>
    public static int[] NoviceGiftGet
    {
        get { return _novice_Data; }
    }

    private static int[] _super_Data = { 10000, 100 };
    /// <summary>
    /// 超值礼包获得资源，第一项是金币，第二项是钻石
    /// </summary>
    public static int[] SuperGiftGet
    {
        get { return _super_Data; }
    }
    /// <summary>
    /// 超值礼包需要支付的费用，单位：RMB
    /// </summary>
    public static int SuperGiftCost
    {
        get { return 10; }
    }
}
