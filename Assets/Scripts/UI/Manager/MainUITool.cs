using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUITool : MonoBehaviour {
    #region 属性和字段

    /// <summary>
    /// 静态实例
    /// </summary>
    public static MainUITool Instance
    {
        get
        {
            return _instance;
        }
    }
    static MainUITool _instance;

    /// <summary>
    /// 界面预制体字典，保存界面复制体的引用
    /// </summary>
    public Dictionary<WindowID, GameObject> Prefab_Dictionary
    {
        get
        {
            return _prefab_Dictionary;
        }
    }
    static Dictionary<WindowID, GameObject> _prefab_Dictionary;//存放界面预制体的字典

    /// <summary>
    /// 存放当前显示界面的栈
    /// </summary>
    public Stack<WindowID> TheActiveStack
    {
        get
        {
            if (SceneManager.GetActiveScene().name.Equals("UI"))
            {
                return _UI_Active_Stack;
            }
            else
            {
                return _game_Active_Stack;
            }
        }
    }
    static Stack<WindowID> _UI_Active_Stack; //存放UI场景当前显示界面的栈
    static Stack<WindowID> _game_Active_Stack; //存放游戏场景当前显示界面的栈

    #endregion

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _UI_Active_Stack = new Stack<WindowID>();
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        _game_Active_Stack = new Stack<WindowID>();
        _prefab_Dictionary = new Dictionary<WindowID, GameObject>();
    }

    /// <summary>
    /// 设置界面显示
    /// </summary>
    /// <param name="id">界面id</param>
	public void SetTheActive(WindowID id)
    {
        //显示界面
        SetTheActive(id, ref _prefab_Dictionary, true);

        if (TheActiveStack.Count > 0)
        {
          

            WindowID current_ID = TheActiveStack.Peek();
            //若打开界面与前一界面层级相同，关闭前一界面
            if (UIResourceDefine.windowLayer[current_ID] == UIResourceDefine.windowLayer[id])
            {
                if (current_ID != id)
                {
                    SetTheActive(current_ID, ref _prefab_Dictionary, false);
                }
            }
            else
            {
                //层级不同，则开启遮罩层
                SetTheActive(WindowID.WindowID_MyMask, ref _prefab_Dictionary, true);
            }
        }

        if (!TheActiveStack.Contains(id))
        {
            //将显示界面加入栈中
            TheActiveStack.Push(id);
        }
    }

    //设置界面的显示状态
    void SetTheActive(WindowID id, ref Dictionary<WindowID,GameObject> prefab_Dictionary,bool active_Bool)
    {

        if (!prefab_Dictionary.ContainsKey(id))
        {
            GameObject temp = Resources.Load(UIResourceDefine.MainUIPrefabPath + UIResourceDefine.windowPrefabName[id]) as GameObject;
            GameObject copy = Instantiate(temp);
            copy.transform.GetComponent<Canvas>().sortingOrder = UIResourceDefine.windowLayer[id];
            prefab_Dictionary.Add(id, copy);
        }
        EffectManager.Instance.WindowEffect(active_Bool, prefab_Dictionary[id]);
    }
    /// <summary>
    /// 关闭当前界面，返回上一界面
    /// </summary>
    public void ReturnPrevious()
    {
        if (SceneManager.GetActiveScene().name.Equals("UI"))
        {
            ReturnPrevious(ref _UI_Active_Stack, _prefab_Dictionary);
        }
        else
        {
            GameUITool.Instance.SetTheActive(GameUI.GameUI_Seconds);
        }
        
    }
    void ReturnPrevious(ref Stack<WindowID> active_Stack, Dictionary<WindowID, GameObject> prefab_Dictionary)
    {
        WindowID remove_ID = active_Stack.Pop();

        SetTheActive(remove_ID,           ref _prefab_Dictionary,false);
        //若返回界面是3级界面，直接跳过
        if (UIResourceDefine.windowLayer[active_Stack.Peek()] == 3)
        {
            active_Stack.Pop();
        }
        SetTheActive(active_Stack.Peek(), ref _prefab_Dictionary,true );

        //遮罩层的关闭
        if(prefab_Dictionary.ContainsKey(WindowID.WindowID_MyMask)&&prefab_Dictionary[WindowID.WindowID_MyMask].activeSelf == true)
        {
            SetTheActive(WindowID.WindowID_MyMask, ref _prefab_Dictionary, false);
        }

        if (active_Stack.Peek() == WindowID.WindowID_SelectHero)
        {
            _prefab_Dictionary[WindowID.WindowID_SelectHero].GetComponentInChildren<Window_ChooseHero>().ReturnUpdate();
        }
    }
    //购买之后，实现购买键的变化
    public void ItemsButton(GameObject button)
    {
        Transform tiao = button.transform.parent;
        Image[] images = tiao.GetComponentsInChildren<Image>();
        foreach (var image in images)
        {
            image.raycastTarget = false;
            image.color = Color.gray;
        }

        Text[] texts = tiao.GetComponentsInChildren<Text>();
        foreach (var text in texts)
        {
            text.raycastTarget = false;
            text.color = Color.gray;
        }

        //已生效显示正常
        tiao.GetChild(0).GetChild(0).gameObject.SetActive(true);
        
    }
    /// <summary>
    /// 交易函数，0：购买钻石，1：购买金币，2：购买体力
    /// </summary>
    /// <param name="x"></param>
    public void Business(int x)
    {
        switch (x)
        {
            case 0:
                break;
            case 1://使用钻石购买金币
                if (MyKeys.Diamond_Value > 6)
                {
                    MyKeys.Diamond_Value -= 6;
                    MyKeys.Gold_Value += 60;
                }
                else
                {
                    UIManager.ActiveWindow(WindowID.WindowID_ShopDiamond);
                }
                break;
            case 2://使用钻石购买体力
                if (MyKeys.Diamond_Value >= MyKeys.BuyPhysicalPowerCost)
                {
                    MyKeys.Diamond_Value -= MyKeys.BuyPhysicalPowerCost;
                    MyKeys.Physical_Power_Value += 1;
                    UMManager.Event(EventID.Buy_Power);
                }
                else
                {
                    //UIManager.ActiveWindow(WindowID.WindowID_ShopZuanShi);
                    MyAudio.PlayAudio(StaticParameter.s_No, false, StaticParameter.s_No_Volume);
                }
                break;
            default:
                Debug.Log("购买行为标记错误");
                break;
        }
    }

    public void SetMapActive()
    {
        _UI_Active_Stack.Pop();
    }
}
