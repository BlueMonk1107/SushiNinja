using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameGuidance : MonoBehaviour
{
    //private Transform _welcome_Text;//开场文本
    private Text _text;//提示文本组件
    private GameObject _jump_Guidance_Image;//跳跃提示图片
    private GameObject _double_Jump_Guidance_Image;//跳跃提示图片
    private GameObject _touch_GameObject;//点击图片
    private GameObject _text_Father;
    private Transform _image_Father;//引导图标的父物体
    
    private bool _touch_OnOff;//显示点击的总开关
    private bool _have_Double_Jump;//是否有二段跳的标记

    private static GameGuidance _instance;
    public static GameGuidance Instance
    {
        get { return _instance; }
    }

    #region 道具引导的标识
    private string _guidance_Dash = "GuidanceDash";
    public int GuidanceDash
    {
        get
        {
            return PlayerPrefs.GetInt(_guidance_Dash, 0);
        }
        set
        {
            if (GuidanceDash < 1)
            {
                Excute(ItemState.Dash);
                PlayerPrefs.SetInt(_guidance_Dash, value);
            }
        }
    }

    private string _guidance_Double = "GuidanceDouble";
    public int GuidanceDouble
    {
        get
        {
            return PlayerPrefs.GetInt(_guidance_Double, 0);
        }
        set
        {
            if (GuidanceDouble < 1)
            {
                Excute(ItemState.Double);
                PlayerPrefs.SetInt(_guidance_Double, value);
            }
        }
    }

    private string _guidance_Magnet = "GuidanceMagnet";
    public int GuidanceMagnet
    {
        get
        {
            return PlayerPrefs.GetInt(_guidance_Magnet, 0);
        }
        set
        {
            if (GuidanceMagnet < 1)
            {
                Excute(ItemState.Magnet);
                PlayerPrefs.SetInt(_guidance_Magnet, value);
            }
        }
    }

    private string _guidance_Protect = "GuidanceProtect";
    public int GuidanceProtect
    {
        get
        {
            return PlayerPrefs.GetInt(_guidance_Protect, 0);
        }
        set
        {
            if (GuidanceProtect < 1)
            {
                Excute(ItemState.Protect);
                PlayerPrefs.SetInt(_guidance_Protect, value);
            }
        }
    }

    private string _guidance_SuperMan = "GuidanceSuperMan";
    public int GuidanceSuperMan
    {
        get
        {
            return PlayerPrefs.GetInt(_guidance_SuperMan, 0);
        }
        set
        {
            if (GuidanceSuperMan < 1)
            {
                Excute(ItemState.SuperMan);
                PlayerPrefs.SetInt(_guidance_SuperMan, value);
            }
        }
    }
    #endregion 道具的标识

    private string _double_Jump = "DoubleJump";
    public int DoubleJump
    {
        get
        {
            return PlayerPrefs.GetInt(_double_Jump, 0);
        }
        set
        {
            if (DoubleJump < 1)
            {
                PlayerPrefs.SetInt(_double_Jump, value);
            }
        }
    }

    // Use this for initialization
    void Start ()
    {
        _instance = this;
        _have_Double_Jump = false;
        foreach (HumanSkill i in HumanManager.Nature.Skill_List)
        {
            if (i == HumanSkill.DoubleJump)
            {
                _have_Double_Jump = true;
            }
        }
        
        _text = transform.GetChild(0).GetComponentInChildren<Text>();
        _jump_Guidance_Image = transform.GetChild(1).gameObject;
        _double_Jump_Guidance_Image = transform.GetChild(2).gameObject;
        _touch_GameObject = transform.GetChild(3).gameObject;
       
        _image_Father = transform.GetChild(0).GetChild(2);

        _text_Father = _text.transform.parent.gameObject;
        _text_Father.SetActive(false);
        _touch_GameObject.SetActive(false);
        _double_Jump_Guidance_Image.SetActive(false);
        //判断条件，是否关闭游戏内新手引导
        if (JudgeCloseGuidance())
        {
            gameObject.SetActive(false);
            return;
        }
        //判断是否开启跳跃提示
        if (GuidanceBase.GuidanceMark == 0)
        {
            //tips声音
            MyAudio.PlayAudio(StaticParameter.s_Tips, false, StaticParameter.s_Tips_Volume);

            MyKeys.Pause_Game = true;
            HumanManager.Nature.Human_Ani.speed = 0;
            StartCoroutine(Wait());
        }
        else
        {
            _jump_Guidance_Image.SetActive(false);
        }


        // 第一次出现二段跳角色，弹出二段跳提示
        if (DoubleJump == 0&& _have_Double_Jump)
        {
            StartCoroutine(WaitPause());
        }
        
    }
    IEnumerator WaitPause()
    {
        yield return new WaitForSeconds(1f);

        MyKeys.Pause_Game = true;
        //tips声音
        MyAudio.PlayAudio(StaticParameter.s_Tips, false, StaticParameter.s_Tips_Volume);

        _double_Jump_Guidance_Image.gameObject.SetActive(true);
        StartCoroutine(Wait());
    }

    bool JudgeCloseGuidance()
    {
        return GuidanceBase.GuidanceMark > 0
               && GuidanceDash > 0
               && GuidanceDouble > 0
               && GuidanceMagnet > 0
               && GuidanceProtect > 0
               && GuidanceSuperMan > 0
               && DoubleJump > 0;
    }

    // Update is called once per frame
    void Update () {
	    if (MyKeys.Pause_Game && HumanManager.Nature.HumanManager_Script.CurrentState != HumanState.Dead)
	    {
	        if (Input.GetMouseButtonDown(0) )
	        {
	            Click();
	        }
        }
	}

    bool JumpCondition()
    {
        if (EventSystem.current.currentSelectedGameObject)
            return false;
        //if (_guidance_Number < 2)
        //    return false;
        return (Input.GetMouseButtonDown(0)/*点击*/
            && ItemColliction.Dash.IsRun() == false/*当前道具状态不是冲刺*/
            && HumanManager.Nature.HumanManager_Script.CurrentState != HumanState.Dead/*人物没有死亡*/
            && ItemColliction.StartDash.IsRun() == false && ItemColliction.DeadDash.IsRun() == false);
    }

    void Click()
    {
        if (_touch_OnOff)
        {
            if (!ItemColliction.Dash.IsRun())
            {
                Jump();
            }
            
            _touch_GameObject.SetActive(false);
            _touch_OnOff = false;
            MyKeys.Pause_Game = false;
            _text_Father.SetActive(false);
            _jump_Guidance_Image.SetActive(false);
            _double_Jump_Guidance_Image.gameObject.SetActive(false);
        }
    }
    public void Excute(ItemState state)
    {
        MyKeys.Pause_Game = true;
        _text_Father.SetActive(true);


        switch (state)
        {
            case ItemState.Dash:
                _text.text = "冲刺：使角色进入冲刺状态";
                SetIconShow(0);
                break;
            case ItemState.Double:
                _text.text = "双倍：可获得双倍积分效果";
                SetIconShow(1);
                break;
            case ItemState.Protect:
                _text.text = "盾牌：使角色抵挡一次任意伤害";
                SetIconShow(2);
                break;
            case ItemState.SuperMan:
                _text.text = "无敌：使角色进入加速状态并附加无敌效果";
                SetIconShow(3);
                break;
            case ItemState.Magnet:
                _text.text = "磁铁：可自动吸收附近的金币";
                SetIconShow(4);
                break;
        }

        StartCoroutine(Wait());
        
    }
    //设置显示道具的图标
    void SetIconShow(int mark)
    {
        //tips声音
        MyAudio.PlayAudio(StaticParameter.s_Tips, false, StaticParameter.s_Tips_Volume);

        foreach (Transform child in _image_Father)
        {
            child.gameObject.SetActive(false);
        }
        _image_Father.GetChild(mark).gameObject.SetActive(true);
    }

    void Jump()
    {
        if (_have_Double_Jump && DoubleJump == 0)
        {
            DoubleJump++;
            return;
        }
        if(HumanManager.Nature.HumanManager_Script.CurrentState==HumanState.Jump && !_have_Double_Jump)
            return;

        switch (HumanManager.WallMark)
        {
            case WallMark.Left:
                HumanManager.WallMark = WallMark.Right;
                break;
            case WallMark.Right:
                HumanManager.WallMark = WallMark.Left;
                break;
        }

        HumanManager.Nature.HumanManager_Script.CurrentState = HumanState.Jump;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);

        _touch_GameObject.transform.GetChild(0).gameObject.SetActive(false);
        _touch_OnOff = true;
        _touch_GameObject.SetActive(true);
    }
}
