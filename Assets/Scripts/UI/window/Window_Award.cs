using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Window_Award : MonoBehaviour
{
    private List<Image> _award_Images;
    private string _logInDays_Mark = "LogInTimeMark";
    private int LogInTimeMark
    {
        set
        {
            PlayerPrefs.SetInt(_logInDays_Mark, value);
        }
        get
        {
            return PlayerPrefs.GetInt(_logInDays_Mark, 0);
        }
    }
    
    private string _log_In_Time = "logInTime";
    //登陆时间戳
    private int LogInTime
    {
        set
        {
            PlayerPrefs.SetInt(_log_In_Time, value);
        }
        get
        {
            return PlayerPrefs.GetInt(_log_In_Time, 0);
        }
    }

    private static string _first_Log_In = DateTime.Today.ToString();
    //本日登陆标记
    public static int FirstLogIn
    {
        private set
        {
            PlayerPrefs.SetInt(_first_Log_In, value);
        }
        get
        {
            return PlayerPrefs.GetInt(_first_Log_In, 0);
        }
    }

    //Use this for initialization
    void Start()
    {
        if (FirstLogIn > 0 || GuidanceBase.GuidanceMark < 3)
        {
            gameObject.SetActive(false);
            return;
        }

        _award_Images = new List<Image>();

        Transform list = transform.GetChild(1).GetChild(0);
        for (int i = 0; i < list.childCount; i++)
        {
            _award_Images.Add(list.GetChild(i).GetComponent<Image>());
            _award_Images[i].color = Color.gray;
            _award_Images[i].raycastTarget = false;
        }

        JudgeDays();

        _award_Images[LogInTimeMark].color = Color.white;
        _award_Images[LogInTimeMark].raycastTarget = true;
    }
  //判断是第几天登陆
    void JudgeDays()
    {
        if (LogInTime == 0) 
        {
            LogInTimeMark = 0;
        }
        else
        {
            ContrastTime();
        }
    }

    void ContrastTime()
    {
        DateTime now_Time = DateTime.Now;
        
        DateTime last_Time = TimeManager.GetTime(LogInTime.ToString());
        
        int days = (now_Time - last_Time.Date).Days;
        
        if (days == 1)
        {
            LogInTimeMark++;
        }
        else if (days > 1)
        {
            LogInTimeMark = 0;
        }
    }

    public void Click()
    {
        MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);

        switch (LogInTimeMark)
        {
            case 0:
                MyKeys.Gold_Value += 500;
                break;
            case 1:
                MyKeys.Diamond_Value += 10;
                break;
            case 2:
                MyKeys.Gold_Value += 1000;
                break;
            case 3:
                MyKeys.Diamond_Value += 15;
                break;
            case 4:
                MyKeys.Diamond_Value += 20;
                break;
            case 5:
                MyKeys.Gold_Value += 1500;
                break;
            case 6:
                MyKeys.Diamond_Value += 30;
                break;
        }

        FirstLogIn ++;
        LogInTime = TimeManager.ConvertDateTimeInt(DateTime.Now);
        _award_Images[LogInTimeMark].color = Color.gray;
        _award_Images[LogInTimeMark].raycastTarget = false;
        gameObject.SetActive(false);
    }
}
