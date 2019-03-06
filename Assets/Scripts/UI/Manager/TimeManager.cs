using System;
using UnityEngine;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    #region 属性和字段
    public static Text Time_Text { get; set; }//显示时间的文本
    private bool _add_Bool { get; set; }//回复体力的开关
    private static int _start_Time { get; set; }//在满体力下，消耗第一点体力时，记录时间

    private int _pauseTime;//游戏到后台以后，程序暂停会执行两次，这个变量是为了计数

    //写在update中的变量（为了避免频繁创建字段）
    private int _minute_Total;//回复需要的总分钟
    private int _minute;//显示不足五分钟的部分
    private int _second;//显示不足一分钟的秒数
    private int _times;//按帧率读秒

    private static string _total_Key = "Total_Time";
    /// <summary>
    /// 剩余没有回复的时间
    /// </summary>
    public static int NotReplayTotalTime
    {
        set
        {
            if (value > 0)
            {
                PlayerPrefs.SetInt(_total_Key, value);
            }
            else
            {
                PlayerPrefs.SetInt(_total_Key, 0);
            }
        }
        get
        {
            return PlayerPrefs.GetInt(_total_Key, 0);
        }
    }

    #endregion

    // Use this for initialization
    void Start () {
        _add_Bool = false;

        if (global::StartGame.First_In)
        {
            //若体力小于最大值，显示回复时间
            if (MyKeys.Physical_Power_Value < MyKeys.Power_Replay_Max)
            {
                RecoverPower();
            }
        }
        else
        {
            if (SceneManager.GetActiveScene().name.Equals("UI"))
            {
                ReplayPower(NotReplayTotalTime);
                ShowTime();
            }
            
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (MyKeys.Physical_Power_Value == MyKeys.Power_Replay_Max)
            return;

        //读秒计数器
        _times++;

        if (_times%(60/QualitySettings.vSyncCount) != 0)
	        return;

            _times = 0;
            //体力的改变和时间的显示
            if (MyKeys.Physical_Power_Value < MyKeys.Power_Replay_Max)
            {
                ShowTime();
            }
            else
            {
                if (Time_Text)
                {
                    Time_Text.text = "已满";
                    NotReplayTotalTime = 0;
                }

            }
            
    }

    //显示回复时间
    void ShowTime()
    {
        //为显示时间做准备
        GetReadyForTime();

        //显示时间
        ShowInText();

        //回复一点体力
        if (_minute == 0 && _second == 0 && _add_Bool)
        {
            _add_Bool = false;
            MyKeys.Physical_Power_Value++;
            if (SceneManager.GetActiveScene().name.Equals("UI"))
            {
                MyKeys.Physical_Power_Text.text = MyKeys.Physical_Power_Value.ToString();
            }
            
        }
        //回复增长体力的开关
        if (_second != 0 && !_add_Bool)
        {
            _add_Bool = true;
        }

    }
    //为所需时间变量赋值
    void GetReadyForTime()
    {
        //回复需要的总时间，单位：秒
        NotReplayTotalTime = NotReplayTotalTime - 1;
        //回复需要的总分钟
        _minute_Total = NotReplayTotalTime / 60;
        //显示不足五分钟的部分
        _minute = _minute_Total % MyKeys.Power_Replay_Time;
        //显示不足一分钟的秒数
        _second = NotReplayTotalTime % 60;
    }
    //显示在文本中
    void ShowInText()
    {
        if(!Time_Text)
            return;
        if (_second > 9)
        {
            Time_Text.text = _minute + ":" + _second;
        }
        else
        {
            Time_Text.text = _minute + ":0" + _second;
        }
    }

    //回复体力
    void RecoverPower()
    {
        if(MyKeys.Physical_Power_Value == MyKeys.Power_Replay_Max)
            return;

        //对比时间，返回值是两次游戏间隔多久。单位：秒
        int difference = ContrastTime();

        //与所剩回复时间做对比，若大于等于，则回满体力，小于则，回复已回体力，并给回复总时间赋值
        if (difference >= NotReplayTotalTime)
        {
            MyKeys.Physical_Power_Value = MyKeys.Power_Replay_Max;
        }
        else
        {
            NotReplayTotalTime = NotReplayTotalTime - difference;
            ReplayPower(NotReplayTotalTime);
        }

    }

    //对比时间，返回值是两次游戏间隔多久。单位：秒
    int ContrastTime()
    {
        if (MyKeys.Time_Day_Value == 0 && MyKeys.Time_Hour_Value == 0 && MyKeys.Time_Minute_Value == 0 &&
            MyKeys.Time_Second_Value == 0)
        {
            MyKeys.Time_Day_Value = System.DateTime.Now.Day;
            MyKeys.Time_Hour_Value = System.DateTime.Now.Hour;
            MyKeys.Time_Minute_Value = System.DateTime.Now.Minute;
            MyKeys.Time_Second_Value = System.DateTime.Now.Second;
        }

        int now_Seconds;
        int before_Seconds;
        //以上一次退出游戏时，当天的0点为基准，做时间比较
        //从基准0点到当前时间所用的秒数
        now_Seconds = ((System.DateTime.Now.Day - MyKeys.Time_Day_Value) * 24 + System.DateTime.Now.Hour) * 3600 + System.DateTime.Now.Minute * 60 + System.DateTime.Now.Second;
        //基准0点到记录时间所用的秒数
        before_Seconds = MyKeys.Time_Hour_Value * 3600 + MyKeys.Time_Minute_Value * 60 + MyKeys.Time_Second_Value;
        //秒数的差值就是在这两次进行游戏之间过了多久
        int difference = now_Seconds - before_Seconds;

        return difference;

    }
    //在无法回满的情况下回复体力
    void ReplayPower(int total_Seconds)
    {
        int not_Replay_Integer = total_Seconds / (MyKeys.Power_Replay_Time * 60);
        int not_Replay_Remainder = total_Seconds % (MyKeys.Power_Replay_Time * 60);

        //还未回复的体力数值
        int number = 0;

        if (not_Replay_Remainder == 0)
        {
            number = not_Replay_Integer;
        }
        else
        {
            number = not_Replay_Integer + 1;
        }

        //回复后的体力总数
        MyKeys.Physical_Power_Value = MyKeys.Power_Replay_Max - number;
    }
    /// <summary>
    /// 减少一点体力
    /// </summary>
    public static void ReducePhysicalPower()
    {
        if (MyKeys.Physical_Power_Value > 0)
        {
            MyKeys.Physical_Power_Value--;

            NotReplayTotalTime = NotReplayTotalTime + MyKeys.Power_Replay_Time * 60;

            //在满体力下，消耗第一点体力时，记录时间
            if (MyKeys.Physical_Power_Value == (MyKeys.Power_Replay_Max - 1))
            {
                _start_Time = (int)Time.time;
            }

        }

    }
    /// <summary>
    /// 通过关卡，则减掉扣体力增加的时间
    /// </summary>
    public static void ResetTime()
    {
        NotReplayTotalTime = NotReplayTotalTime - MyKeys.Power_Replay_Time * 60;
    }

    void OnApplicationPause()
    {
        //第一次进游戏时会执行一次这个函数，排除这次计数
        if (global::StartGame.First_In)
            return;

        _pauseTime++;
        if (_pauseTime == 1)
        {
            _start_Time = (int)Time.time;
        }
        //在程序焦点回到游戏后，回复时间
        if (_pauseTime == 2)
        {
            //若体力小于最大值，显示回复时间
            if (MyKeys.Physical_Power_Value < MyKeys.Power_Replay_Max)
            {
                RecoverPower();
            }
            _pauseTime = 0;
        }
    }
    #region 管理时间的静态方法
    /// <summary>
    /// DateTime时间格式转换为Unix时间戳格式
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static int ConvertDateTimeInt(DateTime time)
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        return (int)(time - startTime).TotalSeconds;
    }


    /// <summary>
    /// 时间戳转为C#格式时间
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public static DateTime GetTime(string timeStamp)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }
    public static string GetBeiJingTime()
    {
        bool isget = false;
        string result = string.Empty;
        try
        {
            string time = TimeManager.GetWebClient("http://www.hko.gov.hk/cgi-bin/gts/time5a.pr?a=2");
            Regex regex = new Regex(@"0=(?<timestamp>\d{10})\d+");

            Match match = regex.Match(time);
            result = match.Groups["timestamp"].Value;
        }
        catch (Exception)
        {
        }
        finally
        {
            if (!isget) result = "0";//如果没有网络就返回默认
        }
        return result;
    }

    #endregion

    private static string GetWebClient(string url)
    {
        string strHTML = "";
        WebClient myWebClient = new WebClient();
        Stream myStream = myWebClient.OpenRead(url);
        StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
        strHTML = sr.ReadToEnd();
        myStream.Close();
        return strHTML;
    }

}
