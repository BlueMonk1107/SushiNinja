using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Window_DiamondShop : MonoBehaviour
{
    // Use this for initialization 
    void Start ()
    {
        ShopData.FindText(transform.GetChild(0), "cost",null,GetStringArray(ShopData.DiamondShopCost));
        ShopData.FindText(transform.GetChild(0), "get",ShopData.DiamondShopGet,null);
        ShopData.FindText(transform.GetChild(0), "present",ShopData.DiamondShopPresent,null);
    }

    string[] GetStringArray(int[] data)
    {
        string[] temp = new string[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            temp[i] = "￥ " + data[i] + " 获得";
        }
        return temp;
    }
}
