using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GuidanceThree : GuidanceBase
{
    // Use this for initialization
    void Start () {
        Initialization(transform);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            SelectedMission(2);
        }
    }
    /// <summary>
    /// 开始前购买道具
    /// </summary>
    /// <param name="x"></param>
    public void BuyItem()
    {
        MyAudio.PlayAudio(StaticParameter.s_Buy, false, StaticParameter.s_Buy_Volume);
        //加入购买的道具
        BuyTheItems.AddItems = Items.Protect;
        //购买后按键的变化
        MainUITool.Instance.ItemsButton(EventSystem.current.currentSelectedGameObject);
        GameObject list = FindObjectOfType<Window_PreparationList>().gameObject;
        MainUITool.Instance.ItemsButton(list.transform.GetChild(3).GetChild(1).gameObject);
        ConvertGuidance(1,2,0);
    }
}
