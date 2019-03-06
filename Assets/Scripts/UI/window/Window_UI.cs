using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Window_UI : MonoBehaviour
{
    public Text[] _text_List = new Text[4];

    void OnEnable()
    {
        Initialization();
    }

    // Use this for initialization
    void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("UI"))
        {
            QualitySettings.vSyncCount = 2;
        }

        Initialization();
    }
    //初始化函数
    void Initialization()
    {
        MyKeys.Diamond_Text = _text_List[0];
        MyKeys.Gold_Text = _text_List[1];
        MyKeys.Physical_Power_Text = _text_List[2];
        TimeManager.Time_Text = _text_List[3];

        _text_List[0].text = MyKeys.Diamond_Value.ToString();
        _text_List[1].text = MyKeys.Gold_Value.ToString();
        _text_List[2].text = MyKeys.Physical_Power_Value.ToString();
        if (MyKeys.Physical_Power_Value == MyKeys.Power_Replay_Max)
        {
            _text_List[3].text = "已满";
        }
    }

   
}
