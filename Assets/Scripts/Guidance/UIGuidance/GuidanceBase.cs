using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.SceneManagement;

public class GuidanceBase : MonoBehaviour {
    /// <summary>
    /// UI导航界面的数组
    /// </summary>
    protected List<GameObject> UI_Guidance_List { get; set; }
    
    private Transform _select_Node;//选中的关卡节点
    private static string _guidance = "Guidance";
    /// <summary>
    /// 导航标志位，导航分成两部分
    /// 0是第一次进入游戏
    /// 1代表通过了第一步导航（通关第一关，完成）
    /// 2代表完成第二步导航（人物升了一级，完成）
    /// 3代表通过第三步导航（通过导航，进入第二关，完成）
    /// </summary>
    public static int GuidanceMark
    {
        get
        {
            return PlayerPrefs.GetInt(_guidance, 4);
        }
        set
        {
            PlayerPrefs.SetInt(_guidance, 4);
            //if (GuidanceMark < 4)
            //{
            //    PlayerPrefs.SetInt(_guidance, value);
            //}
        }
    }

    //导航界面的初始化函数
    protected void Initialization(Transform guidance_Transform)
    {
        UI_Guidance_List = new List<GameObject>();

        for (int i = 0; i < guidance_Transform.childCount; i++)
        {
            UI_Guidance_List.Add(guidance_Transform.GetChild(i).gameObject);
        }

        UI_Guidance_List[0].SetActive(true);
        //判断当前导航的显示状态
        ShowCurrentGuidance(guidance_Transform);
    }

    /// <summary>
    /// 显示当前未通过的导航,并隐藏其他导航组件
    /// </summary>
    /// <param name="guidance_Transform"></param>
    private void ShowCurrentGuidance(Transform guidance_Transform)
    {
        int index = MyKeys.GetNameToInt(guidance_Transform.name);

        if (GuidanceMark + 1 == index)
        {
            guidance_Transform.gameObject.SetActive(true);
        }
        else
        {
            guidance_Transform.gameObject.SetActive(false);
        }
    }

    #region 导航里开始游戏的点击事件
    public void StartGame(int mission)
    {
        MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);

        UI_Guidance_List[UI_Guidance_List.Count-1].SetActive(false);

        if (mission == 2)
        {
            GuidanceMark = 3;
        }

        //体力大于0，直接开始加载场景
        //开始游戏减去一点体力，通过游戏会加一点体力
        TimeManager.ReducePhysicalPower();
        MyKeys.NowPlayingMission = mission;
        int index = mission + 1;

        //显示加载界面
        Loading(index);

    }
    private void Loading(int index)
    {
        GameObject temp = Resources.Load(UIResourceDefine.MainUIPrefabPath + "Loading") as GameObject;
        GameObject copy = Instantiate(temp);
        copy.GetComponent<Loading>().SceneIndex = index;
        if (SceneManager.GetActiveScene().name.Equals("UI"))
        {
            MainUITool.Instance.Prefab_Dictionary[MainUITool.Instance.TheActiveStack.Peek()].SetActive(false);
        }
        else
        {
            GameUITool.Instance.Prefab_Dictionary[GameUITool.Instance.TheActiveStack.Peek()].SetActive(false);
        }
    }
    #endregion
    #region 导航内，步骤间的切换

    /// <summary>
    /// 导航步骤间的转换
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="timeToChange"></param>
    protected void ConvertGuidance(int from,int to,float timeToChange)
    {
        if (GuidanceBase.GuidanceMark == 1 && from == 1)
        {
            
        }
        else
        {
            if (!UI_Guidance_List[from].activeSelf)
                return;
        }

        //tips声音
        MyAudio.PlayAudio(StaticParameter.s_Tips, false, StaticParameter.s_Tips_Volume);
       
        UI_Guidance_List[from].SetActive(false);
        StartCoroutine(Wait(to,timeToChange));
    }

    IEnumerator Wait(int to, float timeToChange)
    {
        yield return new WaitForSeconds(timeToChange);
        UI_Guidance_List[to].SetActive(true);
    }
    #endregion

    protected void SelectedMission(int mission_Index)
    {
        if (global::StartGame.First_In)
            return;
        
        ChangeWindow(Camera.main, mission_Index);
    }

    //射线函数，返回碰撞到的物体，没有返回null
    Transform MyRay(Camera camera, int mission_Index)
    {
        //当前界面不是主界面，返回
        if (MainUITool.Instance.TheActiveStack.Peek() != WindowID.WindowID_MainMenu)
            return null;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Mission")))
        {
            //判断当前是否是解锁关卡
            MissionMark current_Mission = hit.transform.GetComponent<MissionMark>();
            if (!current_Mission.IsLock)
                return null;
            //点击当前导航关卡，才会加载界面
            if (hit.transform.name == mission_Index.ToString())
            {
                UIManager.MissionName = hit.transform.name;
                MyKeys.MissionName = new StringBuilder(hit.transform.name);
                return hit.transform;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    //切换UI界面
    void ChangeWindow(Camera camera,int mission_Index)
    {
        if(MyKeys.MissionName.ToString().Equals(""))
            return;
        if (mission_Index == int.Parse(MyKeys.MissionName.ToString()))
        {
            UIManager.ActiveWindow(WindowID.WindowID_MissionPreparation);
            MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);
            //若是导航界面，弹出准备准备界面导航
            if (GuidanceMark == 0 || GuidanceMark == 2)
            {
                ConvertGuidance(0,1,0.4f);
            }
        }
    }
}
