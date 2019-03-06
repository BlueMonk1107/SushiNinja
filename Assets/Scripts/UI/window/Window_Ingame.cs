using System;
using UnityEngine;
using UnityEngine.UI;

public class Window_Ingame : MonoBehaviour
{
    private static Window_Ingame _instance;
    private Text _move_Meter;
    private Text _gold;
    private float _start_Y;//人物开始移动的位置
    private float _end_Y;//截至计算的位置
    private float _move_Unit;//比例尺,一步是场景内的几个单位
    private Transform _human;


    private static int _steps;

    public static int Steps
    {
        get { return _steps; }
    }

    private int _fruit_Number_Total;
    //本关水果总数
    public int Fruit_Number_Total
    {
        get { return _fruit_Number_Total; }
    }
    public static Window_Ingame Instance 
    {
        get
        {
            return _instance;
        }
    }
    // Use this for initialization
    void Start ()
    {
        MyAudio.ChangeBGM();
        QualitySettings.vSyncCount = 1;
        MyKeys.Game_Score = 0;
        Window_Succeed.Cut_Number = 0;
        try
        {
            UMManager.StartMission(MyKeys.MissionName.ToString());
        }
        catch (Exception)
        {
            
            Debug.Log("关卡调试");
        }

        try
        {
            _fruit_Number_Total = GameObject.FindGameObjectWithTag("FruitManager").transform.childCount;
        }
        catch
        {
            Debug.Log("没有找到FruitManager");
        }
        
        _move_Unit = 0.1748f;
        _human = HumanManager.Nature.Human;
        _start_Y = _human.position.y;
        _instance = this;
        _move_Meter = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        _gold = transform.GetChild(1).GetChild(0).GetComponent<Text>();
        Transform end_Point = GameObject.Find("EndPoint").transform;
        _end_Y = end_Point.transform.position.y;
    }

    void Update()
    {
        ShowStep();
    }

    public void ShowGold(string number)
    {
        _gold.text = number;
    }

    void ShowStep()
    {
        if (_human.position.y < _end_Y && HumanManager.Nature.HumanManager_Script.CurrentState != HumanState.Dead)
        {
            _steps = (int)((_human.position.y - _start_Y) / _move_Unit);
            _move_Meter.text = _steps + "米";
            if (ItemColliction.StartDash.IsRun())
            {
                ItemColliction.StartDash.WhetherExit(_steps);
            }
        }
    }
}
