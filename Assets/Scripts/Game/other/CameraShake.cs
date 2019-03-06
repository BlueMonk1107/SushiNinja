using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
    Transform _main_Camera;
    //每半个周期的帧数
    private int _once_Frames;
    //每帧移动的距离
    private float _once_Distance;
    //帧数计数器
    int _times;
    /// <summary>
    /// 相机震动开关
    /// </summary>
    static bool _is_Shake_Camera;

    /// <summary>
    /// 相机震动开关
    /// </summary>
    public static bool Is_Shake_Camera
    {
        set
        {
            _wait_Bool = true;
            _is_Shake_Camera = value;
        }
    }
    static bool _wait_Bool;

    // Use this for initialization
    void Start () {
        _wait_Bool = false;
        _times = 0;
        _main_Camera = this.transform;
        _is_Shake_Camera = false;
        _once_Frames = 2;
        _once_Distance = 0.1f;
    }
	
	// Update is called once per frame
	void Update () {
        //if (_wait_Bool)
        //{
        //    StartCoroutine(Wait());
        //    _wait_Bool = false;
        //}

        if (_is_Shake_Camera)
        {
            Move( 0, _once_Distance, 0, _main_Camera, _once_Frames,ref _times, ref _is_Shake_Camera);
        }
	}
    IEnumerator Wait()
    {
        MyKeys.Pause_Game = true;
        yield return new WaitForSeconds(0.05f);
        MyKeys.Pause_Game = false;
    }

    void Move(float x,float y,float z, Transform camera,int frames,ref int times,ref bool is_Shake)
    {
        if (times < frames)
        {
            times++;
            camera.Translate(-x, -y, -z); 
          
        }
        else if (times >= frames && times < 3 * frames)
        {
            times++;
            camera.Translate(x, y, z);
        }
        else if (times >= 3 * frames && times < 5 * frames)
        {
            times++;
            camera.Translate(-x, -y, -z);
        }
        else if (times >= 5 * frames && times < 6 * frames)
        {
            times++;
            camera.Translate(x, y, z);
        }
        else
        {
            is_Shake = false;
            times = 0;
        }
        
    }
}
