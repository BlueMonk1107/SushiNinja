using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SuccessEffect : MonoBehaviour
{
    private static SuccessEffect _instances;
    public static SuccessEffect Instances
    {
        get
        {
            if (_instances == null)
            {
                _instances = FindObjectOfType<SuccessEffect>();
            }
            return _instances;
        }
    }
    public Text _now_Score;                 //显示当前关卡分数组件
    public Text _diamond;                   //显示钻石数量组件
    public GameObject _new_Title;           //新纪录组件

    private List<Transform> _stars_List;    //星星组件数组
    private int _times;                     //星星动画执行次数
    private int _score;                     //分数计数器
    private int _add_Once;                  //计数每次加的字数
    private int _add_Times;                 //增加次数
    private bool _auto;                     //自动播放开关
    private bool _start_Effect;             //特效开关
    private int _star_Number;               //星星数量
    private int _diamond_Number;            //钻石计数器
    private int _add_Diamond_Number;        //钻石数
    private bool _diamond_Bool;             //控制增加钻石为零时只执行一次的开关
    void Update()
    {
        if(!_start_Effect)
            return;

        ScoreEffect();

        ShowDiamond(_star_Number);

        KillEffect();
    }
    //初始化函数
    void Initialization()
    {
        _stars_List = new List<Transform>();
        _stars_List.Add(transform.GetChild(0));
        _stars_List.Add(transform.GetChild(1));
        _stars_List.Add(transform.GetChild(2));
        _auto = true;
        _diamond_Bool = false;
        _score = 0;
        _diamond_Number = 0;
        _add_Once = 3;
        _add_Times = MyKeys.Game_Score/_add_Once;
        if (_star_Number - MyKeys.PassStarsMax > 0)
        {
            _add_Diamond_Number = (_star_Number - MyKeys.PassStarsMax) * 2;
        }
        else
        {
            _add_Diamond_Number = 0;
        }
    }

    public void StartSuccessEffect(int starNumber)
    {
        _star_Number = starNumber;
        Initialization();
        _start_Effect = true;
    }


    //分数动效
    void ScoreEffect()
    {
        if (_auto && _score < MyKeys.Game_Score)
        {
            int temp = _score / _add_Once;
            if (temp < _add_Times)
            {
                _score += _add_Once;
            }
            else
            {
                _score++;
            }
            _now_Score.text = _score.ToString();
            MyAudio.PlayAudio(StaticParameter.s_Count, false, StaticParameter.s_Count_Volume);
        }
        
    }
    //钻石动效
    void ShowDiamond(int stars)
    {
        if (_score == MyKeys.Game_Score)
        {
            //得到的钻石数>0，执行钻石动效
            if (_diamond_Number < _add_Diamond_Number)
            {
                _diamond_Number ++;
                _diamond.text = _diamond_Number.ToString();
                MyAudio.PlayAudio(StaticParameter.s_Count, false, StaticParameter.s_Count_Volume);

                if (_diamond_Number == _add_Diamond_Number)
                {
                    StarsEffect(stars);
                }
            }
            //得到的钻石数为0
            if (_add_Diamond_Number == 0&& !_diamond_Bool)
            {
                _diamond_Bool = true;
                _diamond.text = _add_Diamond_Number.ToString();
                StarsEffect(stars);
                if (stars == 0)
                {
                    NewScoreEffect();
                }
            }
        }
    }
    //星星动效
    void StarsEffect(int number)
    {
        if (number > 0)
        {
            _stars_List[_times].gameObject.SetActive(true);
            _stars_List[_times].DOScale(2, 0.5f);
            _stars_List[_times].DOPlay();
            MyAudio.PlayAudio(StaticParameter.s_Star, false, StaticParameter.s_Star_Volume);
            StartCoroutine(Next(number));
        }
        else
        {
            NewScoreEffect();
        }
    }

    IEnumerator Next(int number)
    {
        yield return new WaitForSeconds(1);
        _times++;
        if (_auto)
        {
            if (_times < number)
            {
                StarsEffect(number);
            }
            else
            {
                _times = 0;
                NewScoreEffect();
            }
        }

    }

    //新纪录动效
    void NewScoreEffect()
    {
        MyAudio.PlayAudio(StaticParameter.s_New_record, false, StaticParameter.s_New_record_Volume);

        if (MyKeys.Game_Score == MyKeys.Top_Score)
        {
            _new_Title.SetActive(true);
            _new_Title.transform.DOScale(1, 0.5f);
            _new_Title.transform.DOPlay();
        }
    }

    //跳过动效
    void KillEffect()
    {
        if (Input.GetMouseButtonDown(0) && _auto)
        {
            _auto = false;
            //显示分数
            _now_Score.text = MyKeys.Game_Score.ToString();
            //显示钻石
            if (_add_Diamond_Number>0)
            {
                _diamond.text = _add_Diamond_Number.ToString();
            }
            //显示新纪录
            if (MyKeys.Game_Score == MyKeys.Top_Score)
            {
                _new_Title.gameObject.SetActive(true);
                _new_Title.transform.DOKill();
                _new_Title.transform.localScale = Vector3.one;
            }
            //显示星级
            for (int i = 0; i < _star_Number; i++)
            {
                _stars_List[i].gameObject.SetActive(true);
                _stars_List[i].localScale = Vector3.one*2;
                _stars_List[i].DOKill();
            }
        }
    }

}
