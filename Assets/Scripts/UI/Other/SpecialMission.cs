using System;
using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpecialMission : MonoBehaviour {
    
    private bool _is_Lock;
    private Image _renderer;
    /// <summary>
    /// 关卡是否解锁标签
    /// </summary>
    public bool IsLock
    {
        get { return _is_Lock; }
    }

    void OnEnable()
    {
        _renderer = transform.GetComponent<Image>();
        //获取当前标记的关卡数
        int mark = MyKeys.PassMission;

        if (mark < 6)
        {
            _is_Lock = false;
            float times = 0.5f;
            _renderer.color = new Color(times, times, times,1);
            return;

        }
        else
        {
            _is_Lock = true;
            _renderer.color = Color.white;
        }

    }


    void Update()
    {
        if (StartGame.First_In)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            if (!_is_Lock)
                return;

            //若在新手引导界面，并且不再第一关，跳出函数
            if (GuidanceBase.GuidanceMark == 0 && !transform.name.Equals("1"))
                return;

            if (EventSystem.current.currentSelectedGameObject != null)
            {
                string name = EventSystem.current.currentSelectedGameObject.name;
                if (name.Contains("jueshe") || name.Contains("jia"))
                    return;
            }

            Vector3 temp = transform.position - Input.mousePosition;

            if (Mathf.Abs(temp.x) < 30 && Mathf.Abs(temp.y) < 30
                && MainUITool.Instance.TheActiveStack.Peek() == WindowID.WindowID_MainMenu)
            {
                UIManager.MissionName = transform.name;
                MyKeys.MissionName = new StringBuilder(transform.name);
                UIManager.ActiveWindow(WindowID.WindowID_MissionPreparation);
                MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);
            }
        }
    }
}
