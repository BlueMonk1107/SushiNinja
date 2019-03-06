using System;
using UnityEngine;
using System.Collections;

public class RedPoint : MonoBehaviour
{
    // Use this for initialization
    void OnEnable()
    {
        Window_ChooseHero.UpdatePage += JudgeActive;
        JudgeActive();
    }
    //判断显示
    void JudgeActive()
    {
        if (!MyKeys.JudgeLevelMax())
        {
            int hero_Level = MyKeys.GetHeroLevel(MyKeys.CurrentSelectedHero);
            int pay_Gold = (hero_Level + 1) * 100;

            if (MyKeys.Gold_Value > pay_Gold)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
        
    }

    public void LevelUpHint()
    {
        JudgeActive();
    }

}
