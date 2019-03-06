using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Window_Seconds : MonoBehaviour {
    public List<Image> _image_List;
    float _time;
    float _keep_Time;
	
	void OnEnable(){
        _time = 0;
        _keep_Time = 0;
        MyAudio.PlayAudio(StaticParameter.s_Countdown, false, StaticParameter.s_Countdown_Volume);
    }
    // Update is called once per frame
    void Update () {
        _time += Time.deltaTime;
        if (_time > 0 && _time < 0.6f)
        {
            Change(3);
        }
        else if(_time >=0.6f && _time < 1.2f)
        {
            Change(2);
        }
        else if(_time >= 1.2f && _time < 1.8f)
        {
            Change(1);
        }
        else
        {
            transform.parent.gameObject.SetActive(false);
            ResetGameObject();
            MyKeys.Pause_Game = false;
            if(HumanManager.Nature.HumanManager_Script.CurrentState == HumanState.Dead)
            {
                ResetHero();
            }
            GameUITool.Instance.Prefab_Dictionary[GameUI.GameUI_MyMask].SetActive(false);
        }
	}
    void Change(int mark)
    {
        _keep_Time += Time.deltaTime;
        if (_keep_Time >= 0.6f)
        {
            _keep_Time = 0;
            for (int i = 1; i < 4; i++)
            {
                if (i == mark)
                {
                    _image_List[i].gameObject.SetActive(true);
                }
                else
                {
                    _image_List[i].gameObject.SetActive(false);
                }
            }
        }
    }
    void ResetGameObject()
    {
        _image_List[1].gameObject.SetActive(false);
        _image_List[3].gameObject.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            _image_List[i].fillAmount = 1;
        }
    }
    void ResetHero()
    {
        HumanManager.Nature.Human.gameObject.SetActive(true);
        HumanManager.Nature.HumanManager_Script.CurrentState = HumanState.Stop;
        HumanManager.Nature.HumanManager_Script.ItemState = ItemState.Dash;
    }
}
