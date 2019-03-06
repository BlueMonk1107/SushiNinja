using UnityEngine;
using System.Collections;

/// <summary>
/// 选关类，子物体的顺序不能改变，第一个是关卡集合，第二个是摄像机，第三个是人物，关卡集合的第一个是开始标记，第二个是结束标记 
/// </summary>
public class ChoiceGamePoints : MonoBehaviour
{
    private Transform _select_Node;//选中的关卡节点
    private RectTransform _this_RectTransform;//当前物体的RectTransform组件
    private float _before_Y;
    private float _image_Start_Y;//在地图起始边界，地图的坐标
    private float _image_End_Y;//在地图结束边界，地图的坐标

    // Use this for initialization
    void Start()
    {
        _before_Y = 0;
        _this_RectTransform = transform.GetComponent<RectTransform>();
        _image_Start_Y = _this_RectTransform.sizeDelta.y / 2 - (transform.parent.GetComponent<RectTransform>().sizeDelta.y-800)/2;
        _image_End_Y = transform.parent.GetComponent<RectTransform>().sizeDelta.y - _this_RectTransform.sizeDelta.y;

        int now_Mission = GetMissionStand();
        
        //判断是否需要调整背景图片
        AdjustBGImagePosition(now_Mission);
        //设置波纹位置
        SetRipplePosition(now_Mission);
    }

    void AdjustBGImagePosition(int now_Mission)
    {
        //地图需要调整的起始坐标
        //float height = transform.parent.position.y;
        float start_Y = transform.parent.position.y;
        //float end_Y = _this_RectTransform.sizeDelta.y/2 - height;
        float node_Position_Y = transform.GetChild(0).GetChild(now_Mission - 1).position.y;

        if (node_Position_Y > start_Y)
        {
            transform.position += Vector3.down * (node_Position_Y - start_Y);
        }

        IfBeyondTheBorderAmendImage();

        //transform.GetChild(0).GetChild(now_Mission - 1).position = new Vector3(transform.GetChild(0).GetChild(now_Mission - 1).position.x,
        //    transform.parent.GetComponent<RectTransform>().sizeDelta.y/2,
        //    transform.GetChild(0).GetChild(now_Mission - 1).position.z);
        //if (node_Position_Y > start_Y && node_Position_Y < end_Y)
        //{
        //    transform.position = Vector3.right * Screen.width / 2 + Vector3.up * (_this_RectTransform.sizeDelta.y / 2 - (node_Position_Y - transform.parent.GetComponent<RectTransform>().sizeDelta.y / 2));
        //}
        //else if (node_Position_Y <= start_Y)
        //{
        //    transform.position = Vector3.right * Screen.width / 2 + Vector3.up * _image_Start_Y;
        //}
        //else
        //{
        //    transform.position = Vector3.right * Screen.width / 2 + Vector3.up * _image_End_Y;
        //}
    }
    // Update is called once per frame
    void Update()
    {
        if (StartGame.First_In)
            return;

        //若是导航界面，不允许拖动
        if (GuidanceBase.GuidanceMark < 3)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _before_Y = 0;
        }
        if (Input.GetMouseButton(0))
        {
            Move();//屏幕的移动
        }
        if (Input.GetMouseButtonUp(0))
        {
            _before_Y = 0;
            _select_Node = null;
        }

    }
    void SetRipplePosition(int mission)
    {
        Transform rippleTransform = transform.GetChild(2);
        rippleTransform.position = transform.GetChild(0).GetChild(mission - 1).position - Vector3.up * 0.1f;
        //调用动画函数
        rippleTransform.GetComponent<Ripple>().Animation(mission);
    }
    //获取特效应该存在的点
    int GetMissionStand()
    {
        if (MyKeys.NowPlayingMission == 26)
        {
            return MyKeys.PassMission + 1;
        }
        else
        {
            if (MyKeys.PassMission + 1 <= MyKeys.MissionMaxIndex)
            {
                if (StartGame.First_In)
                {
                    return MyKeys.PassMission + 1;
                }

                if (MyKeys.NowPlayingMission == MyKeys.PassMission || MyKeys.NowPlayingMission == 0)
                {
                    return MyKeys.NowPlayingMission + 1;
                }
                else
                {
                    return MyKeys.NowPlayingMission;
                }
            }
            else
            {
                if (MyKeys.NowPlayingMission == 0)
                {
                    return MyKeys.MissionMaxIndex;
                }
                else
                {
                    return MyKeys.NowPlayingMission;
                }
            }
        }


    }
    //屏幕的移动
    void Move()
    {
        if (MainUITool.Instance.TheActiveStack.Peek() != WindowID.WindowID_MainMenu)
            return;

        float temp = Input.GetAxis("Mouse Y");
        if (_before_Y.Equals(0))
        {
            temp = 0;
        }
        _before_Y = Input.mousePosition.x;

        //移动
        transform.Translate(0, temp * 8, 0);
        IfBeyondTheBorderAmendImage();
    }

    //超出边界，修正图片坐标
    void IfBeyondTheBorderAmendImage()
    {
        //到达边界时修正位置
        if (_this_RectTransform.offsetMin.y > 0)
        {
            _this_RectTransform.localPosition += Vector3.down*_this_RectTransform.offsetMin.y;
        }
        if (_this_RectTransform.offsetMax.y*-1 > 0)
        {
            _this_RectTransform.localPosition += Vector3.down * _this_RectTransform.offsetMax.y;
        }
    }
}
