using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GuidanceTwo : GuidanceBase
{

	// Use this for initialization
	void Start () {
        Initialization(transform);
    }
	

    /// <summary>
    /// 点击UI调用的方法
    /// </summary>
    /// <param name="x"></param>
    public void TheClicked(int x)
    {
        if (global::StartGame.First_In)
            return;

        MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);
        WindowID temp = (WindowID)x;
        MainUITool.Instance.SetTheActive(temp);
        ConvertGuidance(0,1,0.4f);
    }

    public void LevelUp()
    {
        UI_Guidance_List[1].SetActive(false);

        Window_ChooseHero window_ChooseHero = Transform.FindObjectOfType<Window_ChooseHero>();
        window_ChooseHero.LevelUp();
        
        ConvertGuidance(1,2,0f);
    }

    /// <summary>
    /// 返回按钮调用方法
    /// </summary>
    public void Return()
    {
        MyAudio.PlayAudio(StaticParameter.s_Cancel, false, StaticParameter.s_Cancel_Volume);

        MainUITool.Instance.ReturnPrevious();

        UI_Guidance_List[2].SetActive(false);

        StartCoroutine(Wait());

    }

    IEnumerator Wait()
    {
        GuidanceMark = 2;
        yield return new WaitForSeconds(0.4f);
        transform.parent.GetChild(2).gameObject.SetActive(true);
    }
}
