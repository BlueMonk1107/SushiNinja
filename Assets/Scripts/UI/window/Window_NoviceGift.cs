using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Window_NoviceGift : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    FindTextToAssignValue();
	}

    void FindTextToAssignValue()
    {
        Text[] all = transform.GetComponentsInChildren<Text>();
        string name = null;
        string temp = null;
        foreach (Text text in all)
        {
            name = text.transform.parent.name;
            if (name.Contains("gold"))
            {
                temp = ShopData.NoviceGiftGet[0].ToString();
            }
            else if (name.Contains("diamond"))
            {
                temp = ShopData.NoviceGiftGet[1].ToString();
            }
            else if (name.Contains("buy"))
            {
                temp = "￥ " + ShopData.NoviceGiftCost;
            }
            else
            {
                Debug.LogError("新手礼包中:"+name+"组件的名字不合格规定");
            }

            text.text = temp;
        }
    }
}
