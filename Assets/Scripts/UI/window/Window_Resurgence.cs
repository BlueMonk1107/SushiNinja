using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Window_Resurgence : MonoBehaviour
{
    private GameObject _shop_Copy;
    public Image OK;
    void OnEnable()
    {
        MyKeys.Pause_Game = true;
        //根据钻石数判定按钮状态
        if (MyKeys.Diamond_Value > MyKeys.ResurgenceCost)
        {
            OK.raycastTarget = true;
            OK.color = Color.white;
        }
        else
        {
            OK.raycastTarget = false;
            OK.color = Color.gray;
        }

        _shop_Copy = GameUIManager.ShowShop();
    }

    public void OnDisable()
    {
        EffectManager.Instance.WindowEffect(false,_shop_Copy);
    }
}
