using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    /// <summary>
    /// 是否复活的标志位
    /// </summary>
    public static bool Is_Resurgence;
    // Use this for initialization
    void Start()
    {
        if (GuidanceBase.GuidanceMark > 0)
        {
            MyKeys.Pause_Game = false;
        }
        if (!SceneManager.GetActiveScene().name.Equals("UI"))
        {
            TheClicked(GameUI.GameUI_MainUI);
        }
    }
    public void TheClicked(int x)
    {
        if (x != 3)
        {
            MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);
        }
        else
        {
            MyAudio.PlayAudio(StaticParameter.s_UI_Pause, false, StaticParameter.s_UI_Pause_Volume);
        }
        GameUI temp = (GameUI)x;
        GameUITool.Instance.SetTheActive(temp);
    }
    public void TheClicked(GameUI x)
    {
        GameUITool.Instance.SetTheActive(x);
    }

    /// <summary>
    /// 复活
    /// </summary>
    public void Resurgence()
    {
        if (MyKeys.Diamond_Value > MyKeys.ResurgenceCost)
        {
            Is_Resurgence = true;
            MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);
            MyKeys.Diamond_Value -= MyKeys.ResurgenceCost;
            GameUITool.Instance.SetTheActive(GameUI.GameUI_Seconds);
            UMManager.Event(EventID.State_Resurgence,MyKeys.MissionName.ToString());
        }
        HumanManager.Nature.ColliderManager.SetActive(true);
    }

    /// <summary>
    /// 购买冲刺
    /// </summary>
    public void BuyDash()
    {
        MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);

        if (MyKeys.Diamond_Value > MyKeys.BuyDashCost)
        {
            MyKeys.Diamond_Value -= MyKeys.BuyDashCost;
            HumanManager.Nature.HumanManager_Script.ItemState = ItemState.Dash;
            UMManager.Event(EventID.InGame_Dash);
        }
        else
        {
            MyKeys.Pause_Game = true;
            GameUITool.Instance.SetTheActive(GameUI.GameUI_ShopDiamond);
            //EventSystem.current.currentSelectedGameObject.GetComponent<Image>().raycastTarget = false;
            MyAudio.PlayAudio(StaticParameter.s_Buy, false, StaticParameter.s_Buy_Volume);
        }
    }

    /// <summary>
    /// 购买护盾
    /// </summary>
    public void BuyProtect()
    {
        MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);

        if (MyKeys.Diamond_Value > MyKeys.BuyProtectCost)
        {
            MyKeys.Diamond_Value -= MyKeys.BuyProtectCost;
            HumanManager.Nature.HumanManager_Script.ItemState = ItemState.Protect;
            UMManager.Event(EventID.InGame_Protect);
        }
        else
        {
            MyKeys.Pause_Game = true;
            GameUITool.Instance.SetTheActive(GameUI.GameUI_ShopDiamond);
            //EventSystem.current.currentSelectedGameObject.GetComponent<Image>().raycastTarget = false;
            MyAudio.PlayAudio( StaticParameter.s_Buy, false, StaticParameter.s_Buy_Volume);
        }
    }

    //在游戏界面中显示商店
    public static GameObject ShowShop()
    {
        GameObject temp = Resources.Load(UIResourceDefine.GameUIPrefabPath + UIResourceDefine.gameUIPrefabName[GameUI.GameUI_ShowShop]) as GameObject;
        GameObject copy = Instantiate(temp);
        copy.GetComponent<Canvas>().sortingOrder = 100;
        return copy;
    }
}
