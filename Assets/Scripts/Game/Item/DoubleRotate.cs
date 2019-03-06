using UnityEngine;
using System.Collections;

public class DoubleRotate : MonoBehaviour
{
    private Transform _red;
    private Transform _blue;
    private Transform _ball;
    private Vector3 _red_Vector3;
    private Vector3 _blue_Vector3;
    private GameObject _copy_Effect;//光圈特效的副本
    // Use this for initialization
    //void Start()
    //{
    //    _red = transform.GetChild(0);
    //    _blue = transform.GetChild(1);
    //    _ball = transform.GetChild(2);
    //    _red_Vector3 = new Vector3(-1f, 1, 0);
    //    _blue_Vector3 = new Vector3(1f, 1, 0);
    //    _copy_Effect = StaticParameter.ItemEffect(transform);
    //}

    public void OnEnable()
    {
        _red = transform.GetChild(0);
        _blue = transform.GetChild(1);
        _ball = transform.GetChild(2);
        _red_Vector3 = new Vector3(-1f, 1, 0);
        _blue_Vector3 = new Vector3(1f, 1, 0);
        _copy_Effect = StaticParameter.ItemEffect(transform);
    }


    // Update is called once per frame
    void Update()
    {
        if (MyKeys.Pause_Game)
            return;
        _red.RotateAround(_ball.position, _red_Vector3, -3);
        _blue.RotateAround(_ball.position, _blue_Vector3, 3);
        _ball.Rotate(0, 0, -3, Space.World);
    }

    void OnDestroy()
    {
        Destroy(_copy_Effect);
        StaticParameter.ClearReference(ref _copy_Effect);
    }

    public void OnDisable()
    {
        Destroy(_copy_Effect);
        StaticParameter.ClearReference(ref _copy_Effect);
    }
}
