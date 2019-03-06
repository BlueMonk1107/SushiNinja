using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_PreparationList : MonoBehaviour {
    public int _count_Items;
    RectTransform _this_Rect;
    GridLayoutGroup _layout;
	// Use this for initialization
	void Start ()
	{
	    InitializationText();
        _layout = transform.GetComponent<GridLayoutGroup>();
        _this_Rect = this.transform.GetComponent<RectTransform>();
        _this_Rect.sizeDelta = new Vector2(_this_Rect.sizeDelta.x, _layout.cellSize.y * _count_Items + _layout.spacing.y * (_count_Items - 1));
    }

    void OnEnable()
    {
        gameObject.transform.parent.GetComponentInChildren<Scrollbar>().value = 1;
    }

    void InitializationText()
    {
        ShopData.FindText(transform,"cost", GetCostArray(ShopData.ItemShop),null);
        ShopData.FindText(transform, "tip", null, ShopData.ItemTips);
    }

    int[] GetCostArray(Dictionary<Items, int[]> itemShop)
    {
        Items item;
        int[] temp = new int[itemShop.Count];
        for (int i = 0; i < itemShop.Count; i++)
        {
            item = (Items) i;
            if (itemShop[item][0] > 0)
            {
                temp[i] = itemShop[item][0];
            }
            else if (itemShop[item][1] > 0)
            {
                temp[i] = itemShop[item][1];
            }
            else
            {
                Debug.LogError("道具支付数据错误");
            }
        }
        return temp;
    }

}
