using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Window_Succeed : MonoBehaviour
{
    public Text _high_Score;
    
    
    public static int Cut_Number { get; set; }//切了水果的数量
    // Use this for initialization
    void Start ()
    {
        //播放音效
        switch (MyKeys.CurrentSelectedHero)
        {
            case MyKeys.CurrentHero.GuiYuZi:
                MyAudio.PlayAudio(StaticParameter.s_GuiYuZi_Win, false, StaticParameter.s_GuiYuZi_Win_Volume);
                break;
            case MyKeys.CurrentHero.CiShen:
                MyAudio.PlayAudio(StaticParameter.s_CiShen_Win, false, StaticParameter.s_CiShen_Win_Volume);
                break;
            case MyKeys.CurrentHero.YuZi:
                MyAudio.PlayAudio(StaticParameter.s_YuZi_Win, false, StaticParameter.s_YuZi_Win_Volume);
                break;
        }
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return 0;

        //初始化最高分数属性的关键字
        MyKeys.Top_Score_Key = MyKeys.MissionName.ToString();
        //为最高分赋值，属性内会自己判断分数是否更新
        MyKeys.Top_Score = MyKeys.Game_Score;
        //检测是否为导航关卡
        if (GuidanceBase.GuidanceMark == 0)
        {
            GuidanceBase.GuidanceMark = 1;
        }

        //通过关卡加一点体力
        TimeManager.ResetTime();
        //增加金币
        MyKeys.Gold_Value += MyKeys.Game_Score;
        //开始播放动效
        int stars = StartEffect();
        //增加钻石
        AddDiamond(stars);
        //保存过关最大星数
        MyKeys.PassStarsMax = stars;
        //保存此关卡通过状态
        MyKeys.PassMission = MyKeys.GetNameToInt(MyKeys.MissionName.ToString());
        //初始化属性
        Cut_Number = 0;
        //显示分数
        Score();
        UMManager.FinishMission(MyKeys.MissionName.ToString());
    }
    void Score()
    {
        _high_Score.text = MyKeys.Top_Score.ToString();
    }

    //计算星级
    int StartEffect()
    {
        //评定星级
        int stars = Calculate();
        //开始播放动效
        SuccessEffect.Instances.StartSuccessEffect(stars);
        return stars;
    }

    void AddDiamond(int stars)
    {
        if (stars > MyKeys.PassStarsMax)
        {
            MyKeys.Diamond_Value += (stars - MyKeys.PassStarsMax)*2;
        }
    }
    private int Calculate()
    {
        if (SceneManager.GetActiveScene().name.Contains("Endless"))
        {
            return 3;
        }
        else
        {
            int total = Window_Ingame.Instance.Fruit_Number_Total;

            if (total == 0)
                return 0;
            float temp = Cut_Number / (float)total;
            if (temp >= 0.95f)
            {
                return 3;
            }
            else if (temp >= 0.8f)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
            
    }
}
