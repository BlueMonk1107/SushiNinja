using System;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class Window_ChooseHero : MonoBehaviour
{
    public static Action UpdatePage;

    public List<RectTransform> _level_Image_List;//技能条图片父物体数组（一个小红块代表一级）
    private List<Text> _Level_Text_List;//显示等级的文本数组 
    public List<Text> _explain_Text_List;//说明文本的数组 
    public Text _Cost_Text;//显示升级所需花费的文本    
    public GameObject _red_Point;//提示升级的红点
    private Image _level_Up_Button;//升级按钮
    private Image _level_Up_Max_Button;//升满级按钮

    private int[] _skill_level_List;//第一项代表第一个技能的等级，以此类推
    private readonly int[] _level_Max_List = { 50, 50, 50, 50 }; //人物最大等级数组
    private Transform _item_List_Father;//技能栏UI的父物体
    private Transform _explain_List_Father;//技能栏UI的父物体

    private Transform[] _show_Hero_List;//展示人物的父物体数组

    //人物及其子物体的图片组件数组（组件的排列顺序不能变）
    private Image[] _guiYuZI_Images;
    private Image[] _ciShen_Images;
    private Image[] _yuZI_Images;
    private Image[] _shouSi_Images;

    private MyKeys.CurrentHero _the_Clicked_Hero;//当前点击的人物

    public void OnEnable()
    {
        _the_Clicked_Hero = (MyKeys.CurrentHero)100;

        //初始化引用
        InitializeReference();

        InitializeHeroImages();
        ChangeSelectedState((int)MyKeys.CurrentSelectedHero);

        _skill_level_List = new int[_level_Image_List.Count];
        _level_Up_Button = _Cost_Text.transform.parent.GetComponent<Image>();

        UpadateAllState();
    }

    public void Start()
    {
        FindText(_explain_List_Father,"cost");
    }
    private void FindText(Transform parent,string name)
    {
        Text[] temp = null;
        for (int i = 0; i < 3; i++)
        {
            temp = parent.GetChild(i).GetComponentsInChildren<Text>();
            for (int j = 0; j < temp.Length; j++)
            {
                if (temp[j].transform.name.Contains(name))
                {
                    switch (i)
                    {
                        case 0:
                            temp[j].text = MyKeys.CiShenPrice + "元";
                            break;
                        case 1:
                            temp[j].text = "礼包赠送";
                            break;
                        case 2:
                            temp[j].text = MyKeys.ShouSiPrice + "元";
                            break;
                    }
                }
            }
        }
    }

    void UpadateAllState()
    {
        //付费人物是否购买的展示状态
        HeroShowState();
        //更新文本对象的显示状态
        UpdateTextState();
        //更新列表
        UpdateList(MyKeys.CurrentSelectedHero);
        //显示下一级所需金币
        ChangeCost();

        JudgeMaxButtonState();
    }

    void HeroShowState()
    {
        ChangeShowState(MyKeys.CurrentHero.CiShen);
        ChangeShowState(MyKeys.CurrentHero.YuZi);
        ChangeShowState(MyKeys.CurrentHero.ShouSi);
    }

    void ChangeShowState(MyKeys.CurrentHero nowHero)
    {
        Image[] current_List = null;
        switch (nowHero)
        {
            case MyKeys.CurrentHero.CiShen:
                current_List = _ciShen_Images;
                break;
            case MyKeys.CurrentHero.YuZi:
                current_List = _yuZI_Images;
                break;
            case MyKeys.CurrentHero.ShouSi:
                current_List = _shouSi_Images;
                break;
        }

        if (MyKeys.JudgeWhetherBuy(nowHero))
        {
            WhiteImages(current_List);
        }
        else
        {
            GrayImages(current_List);
        }
    }
    //初始化引用
    void InitializeReference()
    {
        _item_List_Father = transform.GetChild(0).GetChild(2);//第三个组件
        _explain_List_Father = transform.GetChild(0).GetChild(6);//第七个组件
        _level_Up_Max_Button = _item_List_Father.GetChild(6).GetComponent<Image>();
        //人物展示栏的父物体
        _show_Hero_List = new Transform[4];
        _Level_Text_List = new List<Text>();

        Transform show_Hero_Father = transform.GetChild(0).GetChild(4);//第五个组件

        for (int i = 0; i < _show_Hero_List.Length; i++)
        {
            _show_Hero_List[i] = show_Hero_Father.GetChild(0).GetChild(i);
            //显示等级的文本数组 
            _Level_Text_List.Add(_show_Hero_List[i].GetChild(0).GetComponent<Text>());
        }

        _level_Up_Max_Button.GetComponentInChildren<Text>().text = MyKeys.Up_Max_Cost.ToString();
    }

    void InitializeHeroImages()
    {
        _guiYuZI_Images = new Image[3];
        _ciShen_Images = new Image[3];
        _yuZI_Images = new Image[3];
        _shouSi_Images = new Image[3];

        Transform father = transform.GetChild(0).GetChild(4).GetChild(0);
        //初始化父物体位置
        father.localPosition = Vector3.up * father.localPosition.y + Vector3.forward * father.localPosition.z;

        AddImages(father.GetChild(0), ref _guiYuZI_Images);
        AddImages(father.GetChild(1), ref _ciShen_Images);
        AddImages(father.GetChild(2), ref _yuZI_Images);
        AddImages(father.GetChild(3), ref _shouSi_Images);
    }

    void AddImages(Transform heroImage, ref Image[] list)
    {
        Image[] temp = heroImage.GetComponentsInChildren<Image>();
        list = temp;
    }
    #region 点击事件

    public void ReturnUpdate()
    {
        //付费人物是否购买的展示状态
        HeroShowState();
        if ((int)_the_Clicked_Hero == 100)
        {
            UpdateList(MyKeys.CurrentSelectedHero);

            ChangeSelectedState((int)MyKeys.CurrentSelectedHero);
        }
        else
        {
            UpdateList(_the_Clicked_Hero);

            ChangeSelectedState((int)_the_Clicked_Hero);
        }
        //更新红点状态
        UpdatePage();
        JudgeMaxButtonState();
    }
    //选中事件
    public void Choose(int mark)
    {
        if ((int)_the_Clicked_Hero == mark)
            return;
        //播放音效
        switch ((MyKeys.CurrentHero)mark)
        {
            case MyKeys.CurrentHero.GuiYuZi:
                MyAudio.PlayAudio(StaticParameter.s_GuiYuZi_Select, false, StaticParameter.s_GuiYuZi_Select_Volume);
                break;
            case MyKeys.CurrentHero.CiShen:
                MyAudio.PlayAudio(StaticParameter.s_CiShen_Select, false, StaticParameter.s_CiShen_Select_Volume);
                break;
            case MyKeys.CurrentHero.YuZi:
                MyAudio.PlayAudio(StaticParameter.s_YuZi_Select, false, StaticParameter.s_YuZi_Select_Volume);
                break;
        }
        MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);

        MyKeys.CurrentHero click = (MyKeys.CurrentHero)mark;
        _the_Clicked_Hero = click;

        if (MyKeys.JudgeWhetherBuy(click))
        {
            MyKeys.CurrentSelectedHero = click;
            UpdateList(click);
        }

        ChangeSelectedState(mark);
        //更新红点状态
        UpdatePage();
        JudgeMaxButtonState();
    }

    void ShowSelectImage(int hero_mark)
    {
        foreach (Transform tr in _show_Hero_List)
        {
            tr.GetChild(3).gameObject.SetActive(false);
            tr.GetComponent<Image>().color = Color.gray;
        }
        int mark = hero_mark;
        if (mark == 100)
        {
            mark = 0;
        }

        _show_Hero_List[mark].GetChild(3).gameObject.SetActive(true);
        _show_Hero_List[mark].GetComponent<Image>().color = Color.white;

        if (hero_mark == 3)
        {
            _show_Hero_List[mark].parent.GetComponent<RectTransform>().anchoredPosition = Vector2.left * 450;
        }
    }
    //升级按钮
    public void LevelUp()
    {
        int hero_Level = MyKeys.GetHeroLevel(MyKeys.CurrentSelectedHero);

        if (GuidanceBase.GuidanceMark == 1)
        {
            //升级
            SetHeroLevel(MyKeys.CurrentSelectedHero);
            ChangeLevelImage();
            return;
        }
        if (hero_Level < MyKeys.Level_Cost_Change_Point)
        {
            int pay_Gold = (hero_Level + 1) * 100;
            if (MyKeys.Gold_Value >= pay_Gold)
            {
                MyKeys.Gold_Value -= pay_Gold;

                //升级
                SetHeroLevel(MyKeys.CurrentSelectedHero);
                ChangeLevelImage();
                //升级提示
                _red_Point.GetComponent<RedPoint>().LevelUpHint();
            }
            else
            {
                //UIManager.ActiveWindow(WindowID.WindowID_ShopJinBi);
                SetButtonInactivity(_level_Up_Button);
            }
        }
        else
        {
            if (MyKeys.Diamond_Value >= MyKeys.Level_Cost_Diamond)
            {
                MyKeys.Diamond_Value -= MyKeys.Level_Cost_Diamond;

                //升级
                SetHeroLevel(MyKeys.CurrentSelectedHero);

                ChangeLevelImage();
            }
            else
            {
                //UIManager.ActiveWindow(WindowID.WindowID_ShopZuanShi);
                SetButtonInactivity(_level_Up_Button);
            }
        }

        ChangeCost();//显示下一级所需金币
        //更新红点状态
        UpdatePage();
    }
    //设置按钮不能点击，颜色变灰
    void SetButtonInactivity(Image image)
    {
        image.raycastTarget = false;
        image.color = Color.gray;
    }

    void SetButtonActivity(Image image)
    {
        image.raycastTarget = true;
        image.color = Color.white;
    }

    void ChangeSelectedState(int hero_mark)
    {
        //显示选中图标
        ShowSelectImage(hero_mark);

        MyKeys.CurrentHero click_Hero = (MyKeys.CurrentHero)hero_mark;

        switch (click_Hero)
        {
            case MyKeys.CurrentHero.GuiYuZi:
                ItemListState(-1, 1);
                break;
            case MyKeys.CurrentHero.CiShen:
                ItemListState(0, MyKeys.CiShen_Buy);
                break;
            case MyKeys.CurrentHero.YuZi:
                ItemListState(1, MyKeys.YuZi_Buy);
                break;
            case MyKeys.CurrentHero.ShouSi:
                ItemListState(2, MyKeys.ShouSi_Buy);
                break;
        }
    }

    void ItemListState(int hero_Mark, int buy_Mark)
    {
        foreach (Transform child in _explain_List_Father)
        {
            child.gameObject.SetActive(false);
        }

        if (buy_Mark > 0)
        {
            _item_List_Father.gameObject.SetActive(true);

        }
        else
        {
            _item_List_Father.gameObject.SetActive(false);
            _explain_List_Father.GetChild(hero_Mark).gameObject.SetActive(true);
        }
    }
    void GrayImages(Image[] images_List)
    {
        foreach (Image image in images_List)
        {
            image.color = Color.gray;
        }
    }

    void WhiteImages(Image[] images_List)
    {
        foreach (Image image in images_List)
        {
            image.color = Color.white;
        }
    }

    //修改等级点的状态
    void ChangeLevelImage()
    {
        FadeLevelImage();

        for (int i = 0; i < _level_Image_List.Count; i++)
        {
            for (int j = 0; j < _skill_level_List[i]; j++)
            {
                _level_Image_List[i].GetChild(j).gameObject.SetActive(true);
            }
        }
    }

    //先回复等级点的初始状态
    void FadeLevelImage()
    {
        for (int i = 0; i < _level_Image_List.Count; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                _level_Image_List[i].GetChild(j).gameObject.SetActive(false);
            }
        }
    }

    void ChangeCost()
    {
        ChangeCostTest();

        ChangeCostIcon();

    }

    void ChangeCostTest()
    {
        if (!MyKeys.JudgeLevelMax())
        {
            int hero_Level = MyKeys.GetHeroLevel(MyKeys.CurrentSelectedHero);
            if (hero_Level < MyKeys.Level_Cost_Change_Point)
            {
                int temp = (hero_Level + 1) * 100;
                _Cost_Text.text = temp.ToString();

                if (MyKeys.Gold_Value < temp)
                {
                    SetButtonInactivity(_level_Up_Button);
                }
                else
                {
                    SetButtonActivity(_level_Up_Button);
                }
            }
            else
            {
                _Cost_Text.text = MyKeys.Level_Cost_Diamond.ToString();
                if (MyKeys.Diamond_Value < MyKeys.Level_Cost_Diamond)
                {
                    SetButtonInactivity(_level_Up_Button);
                }
                else
                {
                    SetButtonActivity(_level_Up_Button);
                }
            }

        }
        else
        {
            _Cost_Text.text = "满级";
            SetButtonInactivity(_level_Up_Button);
            _red_Point.SetActive(false);
        }
    }
    void ChangeCostIcon()
    {
        Transform iconTransform = _item_List_Father.GetChild(5).GetChild(0);
        Transform goldIcon = iconTransform.GetChild(0);
        Transform diamondIcon = iconTransform.GetChild(1);

        if (!iconTransform.name.Contains("icon"))
        {
            Debug.LogError("升级按钮上的图标获取错误");
        }

        if (!MyKeys.JudgeLevelMax())
        {
            int hero_Level = MyKeys.GetHeroLevel(MyKeys.CurrentSelectedHero);
            
            if (hero_Level < MyKeys.Level_Cost_Change_Point)
            {
                goldIcon.gameObject.SetActive(true);
                diamondIcon.gameObject.SetActive(false);
            }
            else
            {
                goldIcon.gameObject.SetActive(false);
                diamondIcon.gameObject.SetActive(true);
            }
        }
        else
        {
            goldIcon.gameObject.SetActive(false);
            diamondIcon.gameObject.SetActive(false);
        }
    }
    //直升满级
    public void UpMax()
    {
        MyAudio.PlayAudio(StaticParameter.s_Levelup, false, StaticParameter.s_Levelup_Volume);

        //若英雄是最大等级跳出
        if (MyKeys.GetHeroLevel(MyKeys.CurrentSelectedHero) == _level_Max_List[(int)MyKeys.CurrentSelectedHero])
            return;
        if (MyKeys.Diamond_Value >= MyKeys.Up_Max_Cost)
        {
            EffectManager.Instance.LevelUpEffect();

            MyKeys.Diamond_Value -= MyKeys.Up_Max_Cost;
            SetHeroLevelMax(MyKeys.CurrentSelectedHero);
            UpdateList(MyKeys.CurrentSelectedHero);
            ChangeLevelImage();
            JudgeMaxButtonState();
            ChangeCost();
        }
        else
        {
            UIManager.ActiveWindow(WindowID.WindowID_ShopDiamond);
        }

        //更新红点状态
        UpdatePage();
    }

    void JudgeMaxButtonState()
    {
        //判断当前英雄是否是最高级
        if (MyKeys.JudgeLevelMax())
        {
            SetButtonInactivity(_level_Up_Button);
            SetButtonInactivity(_level_Up_Max_Button);
        }
        else
        {
            SetButtonActivity(_level_Up_Button);
            SetButtonActivity(_level_Up_Max_Button);
        }
    }
    //确认购买
    public void ConfirmBuyHero(int mark)
    {
        bool isBuy = false;
        MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);

        MyKeys.CurrentHero selectedHero = (MyKeys.CurrentHero)mark;
        switch (selectedHero)
        {
            case MyKeys.CurrentHero.CiShen:
                isBuy = PayRMB.Instance.PayRMBFun(MyKeys.CiShenPrice);
                if (isBuy)
                {
                    MyKeys.CiShen_Buy = 1;//表示此角色已经购买
                }
                break;
            case MyKeys.CurrentHero.YuZi:
                UIManager.ActiveWindow(WindowID.WindowID_SuperGift);
                break;
            case MyKeys.CurrentHero.ShouSi:
                isBuy = PayRMB.Instance.PayRMBFun(MyKeys.ShouSiPrice);
                if (isBuy)
                {
                    MyKeys.ShouSi_Buy = 1;//表示此角色已经购买
                }
                break;
        }
        ReturnUpdate();
    }
    #endregion
    //列表更新函数
    void UpdateList(MyKeys.CurrentHero current_Hero)
    {
        Clear();

        //获取人物等级
        int hero_Level = MyKeys.GetHeroLevel(current_Hero);

        //计算每种技能的等级
        for (int i = 0; i < hero_Level; i++)
        {
            SetItemLevel(i);
        }

        //更新文本
        UpdateText((int)MyKeys.CurrentSelectedHero);
        ChangeLevelImage();
        ChangeCost();
        ChangeSelectedState((int)MyKeys.CurrentSelectedHero);
    }
    //清空上次数据，重新计算
    void Clear()
    {
        for (int i = 0; i < _skill_level_List.Length; i++)
        {
            _skill_level_List[i] = 0;
        }
    }

    void UpdateText(int mark)
    {
        if (!MyKeys.JudgeWhetherBuy((MyKeys.CurrentHero)mark))
            return;
        UpdateLevelText();
        UpdateExplainText();
    }
    //更新等级文本
    void UpdateLevelText()
    {
        for (int i = 0; i < 4; i++)
        {
            if ((MyKeys.GetHeroLevel((MyKeys.CurrentHero)i) + 1) < 10)
            {
                _Level_Text_List[i].text = " " + (MyKeys.GetHeroLevel((MyKeys.CurrentHero)i) + 1);
            }
            else
            {
                _Level_Text_List[i].text = (MyKeys.GetHeroLevel((MyKeys.CurrentHero)i) + 1).ToString();
            }

        }

    }
    //更新说明文档
    void UpdateExplainText()
    {
        for (int i = 0; i < _explain_Text_List.Count; i++)
        {
            string a = "";
            switch (i)
            {
                case 0:
                    a = "盾 牌 时 间 增 加 ";
                    break;
                case 1:
                    a = "无 敌 时 间 增 加 ";
                    break;
                case 2:
                    a = "双 倍 时 间 增 加 ";
                    break;
                case 3:
                    a = "磁 铁 时 间 增 加 ";
                    break;
                case 4:
                    a = "冲 刺 时 间 增 加 ";
                    break;
                default:
                    Debug.Log("这里不对");
                    break;
            }
            _explain_Text_List[i].text = a + _skill_level_List[i] * 10 + "%";
        }
    }

    //更新文本对象的显示状态
    void UpdateTextState()
    {
        //for (int i = 1; i < _Level_Text_List.Count; i++)
        //{
        //    if (!MyKeys.JudgeWhetherBuy((MyKeys.CurrentHero)i))
        //    {
        //        Debug.Log(i);
        //        _Level_Text_List[i].gameObject.SetActive(false);
        //    }
        //    else
        //    {
        //        _Level_Text_List[1].gameObject.SetActive(true);
        //    }
        //}
    }


    //设置人物等级
    void SetHeroLevel(MyKeys.CurrentHero current_Hero)
    {
        //声音特效
        MyAudio.PlayAudio(StaticParameter.s_Levelup, false, StaticParameter.s_Levelup_Volume);
        EffectManager.Instance.LevelUpEffect();

        SetItemLevel(MyKeys.GetHeroLevel(current_Hero));

        switch (current_Hero)
        {
            case MyKeys.CurrentHero.GuiYuZi:
                MyKeys.GuiYuZi_Level_Value += 1;
                break;
            case MyKeys.CurrentHero.CiShen:
                MyKeys.CiShen_Level_Value += 1;
                break;
            case MyKeys.CurrentHero.YuZi:
                MyKeys.YuZi_Level_Value += 1;
                break;
            case MyKeys.CurrentHero.ShouSi:
                MyKeys.ShouSi_Level_Value += 1;
                break;
        }

        UpdateText((int)current_Hero);

        Debug.Log("关于等级的友盟设置还没有修改");
        //UMManager.SetHeroLevel(MyKeys.HeroOne_Level_Value);
        //UMManager.Event(EventID.Boy_Level, MyKeys.HeroOne_Level_Value.ToString());

        JudgeMaxButtonState();
    }
    //设置人物技能为满级
    void SetHeroLevelMax(MyKeys.CurrentHero current_Hero)
    {
        switch (current_Hero)
        {
            case MyKeys.CurrentHero.GuiYuZi:
                MyKeys.GuiYuZi_Level_Value = _level_Max_List[(int)current_Hero];
                break;
            case MyKeys.CurrentHero.CiShen:
                MyKeys.CiShen_Level_Value = _level_Max_List[(int)current_Hero];
                break;
            case MyKeys.CurrentHero.YuZi:
                MyKeys.YuZi_Level_Value = _level_Max_List[(int)current_Hero];
                break;
            case MyKeys.CurrentHero.ShouSi:
                MyKeys.ShouSi_Level_Value = _level_Max_List[(int)current_Hero];
                break;
        }
        MyAudio.PlayAudio(StaticParameter.s_Levelup, false, StaticParameter.s_Levelup_Volume);

        JudgeMaxButtonState();

    }
    //设置道具等级
    void SetItemLevel(int i)
    {
        var index = i % 5;
        _skill_level_List[index] += 1;
    }
}
