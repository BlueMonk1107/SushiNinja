using UnityEngine;
using UnityEngine.UI;

public class Window_GoldShop : MonoBehaviour {
    
    // Use this for initialization 
    void Start()
    {
        ShopData.FindText(transform.GetChild(0), "cost", ShopData.GoldShopCost,null);
        ShopData.FindText(transform.GetChild(0), "shoulijian_Num", ShopData.GoldShopGet,null);
    }


    

}
