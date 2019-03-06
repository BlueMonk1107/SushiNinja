using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameUITool : MonoBehaviour {
    #region 属性和字段

    /// <summary>
    /// 静态实例
    /// </summary>
    public static GameUITool Instance
    {
        get
        {
            return _instance;
        }
    }
    static GameUITool _instance;

    /// <summary>
    /// 界面预制体字典，保存界面复制体的引用
    /// </summary>
    public Dictionary<GameUI, GameObject> Prefab_Dictionary
    {
        get
        {
            return _prefab_Dictionary;
        }
    }
    static Dictionary<GameUI, GameObject> _prefab_Dictionary;//存放界面预制体的字典

    /// <summary>
    /// 存放当前显示界面的栈
    /// </summary>
    public Stack<GameUI> TheActiveStack
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
    static Stack<GameUI> _UI_Active_Stack; //存放UI场景当前显示界面的栈
    static Stack<GameUI> _game_Active_Stack; //存放游戏场景当前显示界面的栈

    #endregion

    void Awake()
    {
        _instance = this;
        _UI_Active_Stack = new Stack<GameUI>();
        _game_Active_Stack = new Stack<GameUI>();
        _prefab_Dictionary = new Dictionary<GameUI, GameObject>();
    }


    /// <summary>
    /// 设置界面显示
    /// </summary>
    /// <param name="id">界面id</param>
    public void SetTheActive(GameUI id)
    {
        SetTheActive(id, ref _prefab_Dictionary);

        if (TheActiveStack.Count > 0)
        {
            GameUI current_ID = TheActiveStack.Peek();
            if(UIResourceDefine.gameUILayer[id] == UIResourceDefine.gameUILayer[current_ID])
            {
                //隐藏当前界面
                EffectManager.Instance.WindowEffect(false, _prefab_Dictionary[current_ID]);
            }
            //开启遮罩
            SetTheActive(GameUI.GameUI_MyMask, ref _prefab_Dictionary);
        }
        //将显示界面加入栈中
        TheActiveStack.Push(id);
   
    }
    void SetTheActive(GameUI id, ref Dictionary<GameUI, GameObject> prefab_Dictionary)
    {
        if (prefab_Dictionary.ContainsKey(id))
        {
            prefab_Dictionary[id].SetActive(true);
        }
        else
        { 
            GameObject temp = null;
            if (id == GameUI.GameUI_ShopDiamond|| id == GameUI.GameUI_ShopGold)
            {
                temp =Resources.Load(UIResourceDefine.MainUIPrefabPath + UIResourceDefine.gameUIPrefabName[id]) as GameObject;
            }
            else
            {
                temp = Resources.Load(UIResourceDefine.GameUIPrefabPath + UIResourceDefine.gameUIPrefabName[id]) as GameObject;
            }
            GameObject copy = Instantiate(temp);
            copy.transform.GetComponent<Canvas>().sortingOrder = UIResourceDefine.gameUILayer[id];
            prefab_Dictionary.Add(id, copy);
            copy.SetActive(true);
        }
    }

    public void Return()
    {
        _prefab_Dictionary[TheActiveStack.Pop()].SetActive(false);
        _prefab_Dictionary[TheActiveStack.Peek()].SetActive(true);
    }
}
