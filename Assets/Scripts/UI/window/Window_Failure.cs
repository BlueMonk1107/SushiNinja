using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Window_Failure : MonoBehaviour
{
    public Text _high_Score;
    public Text _now_Score;
	// Use this for initialization
	void Start ()
	{
        UMManager.FailMission(MyKeys.MissionName.ToString());
        Score();
	}
	
    void Score()
    {
        _now_Score.text = MyKeys.Game_Score.ToString();
        _high_Score.text = MyKeys.Top_Score.ToString();
        MyKeys.Gold_Value += MyKeys.Game_Score;

        if (SceneManager.GetActiveScene().name.Contains("Endless"))
        {
            //初始化最高分数属性的关键字
            MyKeys.Top_Score_Key = MyKeys.MissionName.ToString();
            //为最高分赋值，属性内会自己判断分数是否更新
            MyKeys.Top_Score = MyKeys.Game_Score;
        }
    }
}
