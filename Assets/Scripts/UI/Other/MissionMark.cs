using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionMark : MonoBehaviour
{
    private GameObject _stars;
    private GameObject _number_GameObject;//显示关卡数字的预制体
    private bool _is_Lock;
    private Image _renderer;
    private const float _the_Max_Size_Heigh = 175;//第一个节点的高度，其他的关卡数字需要按照比例缩放
    private float _scale_Times;//这个节点相对最大节点的缩放倍数
    /// <summary>
    /// 关卡是否解锁标签
    /// </summary>
    public bool IsLock
    {
        get { return _is_Lock; }
    }

    public void Awake()
    {
        _scale_Times = transform.GetComponent<RectTransform>().sizeDelta.y/_the_Max_Size_Heigh;
    }

    void OnEnable()
    {
        _renderer = transform.GetComponent<Image>();
        ShowMissionNumber();
        //获取当前标记的关卡数
        int mark = MyKeys.GetNameToInt(transform.name);

        if (mark <= MyKeys.MissionMaxIndex)
        {
            //通过的关卡，修改shader
            if (mark <= MyKeys.PassMission + 1)
            {
                _is_Lock = true;
            }
            else
            {
                _is_Lock = false;
                SetMissionDark();
                return;
            }

            //星星实例设置部分
            SetStars();
        }
        else
        {
            _is_Lock = false;
            SetMissionDark();
        }

    }

    void SetMissionDark()
    {
        _renderer.color = Color.white * 0.35f;

        foreach (Image image in GetComponentsInChildren<Image>())
        {
            Image temp = image.GetComponent<Image>();
            if (temp)
            {
                temp.color = Color.white * 0.35f;
            }
        }
    }
    void SetStars()
    {
        _stars = Resources.Load(UIResourceDefine.MainUIPrefabPath + "Stars") as GameObject;//加载星星预制体
        GameObject copy = Instantiate(_stars);
        copy.transform.position = transform.position;
        copy.transform.SetParent(transform); 
        copy.transform.localScale = Vector3.one * _scale_Times;
        JudgeNumber(copy);//判断星级并显示
    }
    //判断星级并显示
    void JudgeNumber(GameObject stars)
    {
        for (int i = 0; i < MyKeys.GetStarsMax(transform.name); i++)
        {
            stars.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    //显示关卡数字
    void ShowMissionNumber()
    {
        Sprite decade_Texture = null;
        //计算十位和个位数字
        int number = Int32.Parse(transform.name);
        int decade = number / 10;
        int unit = number % 10;
        //根据名字，加载图片
        if (number > 9)
        {
            _number_GameObject = Resources.Load(UIResourceDefine.MainUIPrefabPath + "double-digit") as GameObject;
            decade_Texture = Resources.Load<Sprite>(UIResourceDefine.MainUIPrefabPath + "Number/" + decade);
        }
        else
        {
            _number_GameObject = Resources.Load(UIResourceDefine.MainUIPrefabPath + "one-digit") as GameObject;
        }

        Sprite unit_Texture = Resources.Load<Sprite>(UIResourceDefine.MainUIPrefabPath + "Number/" + unit);
        //创建实例，并设置参数
        GameObject temp = Instantiate(_number_GameObject);
        temp.transform.position = transform.position + Vector3.up*5*_scale_Times;
        temp.transform.SetParent(transform,true);
        temp.transform.localScale = Vector3.one*_scale_Times;
        
        if (number < 10)
        {
            temp.GetComponent<Image>().sprite = unit_Texture;
        }
        else
        {
            temp.transform.GetChild(0).GetComponent<Image>().sprite = unit_Texture;
            temp.transform.GetChild(1).GetComponent<Image>().sprite = decade_Texture;
        }

    }

    void Update()
    {
        if (StartGame.First_In)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            if (!_is_Lock)
                return;
            //若在新手引导界面，并且不再第一关，跳出函数
            if (GuidanceBase.GuidanceMark == 0 && !transform.name.Equals("1"))
                return;
            if (GuidanceBase.GuidanceMark == 1)
                return;
            if (GuidanceBase.GuidanceMark == 2 && !transform.name.Equals("2"))
                return;

            if (EventSystem.current.currentSelectedGameObject != null)
            {
                string name = EventSystem.current.currentSelectedGameObject.name;
                if (name.Contains("jueshe") || name.Contains("jia"))
                    return;
            }

            Vector3 temp = transform.position - Input.mousePosition;

            if (Mathf.Abs(temp.x) < 30 && Mathf.Abs(temp.y) < 30
                && MainUITool.Instance.TheActiveStack.Peek() == WindowID.WindowID_MainMenu)
            {
                UIManager.MissionName = transform.name;
                MyKeys.MissionName = new StringBuilder(transform.name);
                UIManager.ActiveWindow(WindowID.WindowID_MissionPreparation);
                MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);
            }
        }
    }

}
