using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Window_SuperGift : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    FindTextToAssignValue();

	}

    void FindTextToAssignValue()
    {
        Text[] all = transform.GetComponentsInChildren<Text>();
        string name = null;
        foreach (Text text in all)
        {
            name = text.transform.parent.name;
            if (name.Contains("gold"))
            {
                text.text = ShopData.SuperGiftGet[0].ToString();
            }
            else if (name.Contains("diamond"))
            {
                text.text = ShopData.SuperGiftGet[1].ToString();
            }
            else if (name.Contains("buy"))
            {
                text.text = "￥ " + ShopData.SuperGiftCost;
            }
            else
            {
                Debug.LogError("新手礼包中:" + name + "组件的名字不合格规定");
            }
        }
    }
}
