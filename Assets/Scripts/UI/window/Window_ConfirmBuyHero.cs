using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Window_ConfirmBuyHero : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    Text text = transform.GetComponentInChildren<Text>();
	    text.text = MyKeys.CiShenPrice.ToString();
	}

    public void ConfirmBuyHero()
    {
        if (MyKeys.Gold_Value >= MyKeys.CiShenPrice)
        {
            MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);
            MyKeys.CiShen_Buy = 1;//表示此角色已经购买
            MainUITool.Instance.ReturnPrevious();
        }
        else
        {
            UIManager.ActiveWindow(WindowID.WindowID_ShopGold);
        }
    }
}
