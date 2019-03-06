using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Window_Preparation : MonoBehaviour
{

    public Text _high_Score;
    public GameObject _mission_Name;//关卡名称
    private readonly int[] _dark_Mission_Index = {17, 18, 20,23,24,26};


    void OnEnable()
    {
        ChangeMissionName();
        //显示最高分
        if (_high_Score != null)
        {
            //初始化最高分数属性的关键字
            MyKeys.Top_Score_Key = MyKeys.MissionName.ToString();
            //显示最高分
            _high_Score.text = MyKeys.Top_Score.ToString();
        }
    }

    //修改关卡显示名称
    void ChangeMissionName()
    {
        int number = int.Parse(MyKeys.MissionName.ToString());
        if (number < 26)
        {
            _mission_Name.transform.GetChild(0).gameObject.SetActive(true);
            _mission_Name.transform.GetChild(1).gameObject.SetActive(false);
            int decade = number / 10;
            int unit = number % 10;


            //加载十位的数字
            Sprite decade_Texture = Resources.Load<Sprite>(UIResourceDefine.MainUIPrefabPath + "Number/" + decade);
            //加载个位的数字
            Sprite unit_Texture = Resources.Load<Sprite>(UIResourceDefine.MainUIPrefabPath + "Number/" + unit);

            if (number < 10)
            {
                _mission_Name.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                _mission_Name.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = unit_Texture;
                _mission_Name.transform.GetChild(0).GetChild(0).localPosition = Vector3.zero;
            }
            else
            {
                _mission_Name.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                _mission_Name.transform.GetChild(0).GetChild(0).localPosition = Vector3.right * 22;
                _mission_Name.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = unit_Texture;
                _mission_Name.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = decade_Texture;
            }
        }
        else
        {
            _mission_Name.transform.GetChild(0).gameObject.SetActive(false);
            _mission_Name.transform.GetChild(1).gameObject.SetActive(true);
        }
        
    }
}
