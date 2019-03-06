using UnityEngine;
using DG.Tweening;

public class ScarfScript : MonoBehaviour {

    static TrailRenderer _scarf;


    float _distance;
    float _move_Time;
    float _speed;
    float _time;
    int _go_Mark;//1是起点，2是终点
    HumanManager _human;
    Vector3 _start_Position;
    int _base_Num;

    public int Base_Num
    {
        get
        {
            return _base_Num;
        }

        set
        {
            _base_Num = value;
            if (_base_Num == 1)
            {
                transform.localPosition = _start_Position;
            }
        }
    }

    static ScarfScript _instance;
    public static ScarfScript Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<ScarfScript>();
            }
            return _instance;
        }
    }

    public static TrailRenderer Scarf
    {
        get
        {
            return _scarf;
        }
    }

    // Use this for initialization
    void Start () {
        _scarf = transform.GetComponent<TrailRenderer>();
        StaticParameter.JudgeNull("围巾", _scarf);

        _distance = 0.4f;
        _move_Time = 0.08f;

        _time = 0;
        _go_Mark = 2;
        
        _start_Position = transform.localPosition;
        Base_Num = 1;
    }

    float GetSpeed(float time,float distance)
    {
        float temp = Random.Range(0, time*2);
        float y_Temp = (time + temp) / Time.fixedDeltaTime;
        return (distance / y_Temp);
    }

	// Update is called once per frame
	void Update () {


        if (_time < _move_Time)
        {
            _time += Time.fixedDeltaTime;
        }
        else {
            _time = 0;
            if (_go_Mark == 1)
            {
                _speed = GetSpeed(_move_Time, _distance);
                _go_Mark = 2;
                transform.Translate(0, 0.02f, 0);
            }
            else {
                _go_Mark = 1;
                transform.Translate(0, -0.02f, 0);
            }
        }

        //负责围巾的波动，人物状态只有在jump时，Base_Num会变大
        if (_go_Mark == 2)
        {
            transform.Translate(0, _speed * Base_Num, 0);
        }
        else {
            transform.Translate(0, -_speed * Base_Num, 0);
        }


    }
}
