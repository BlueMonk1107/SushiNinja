using UnityEngine;
using System.Collections;

public class DarkGuidance : MonoBehaviour
{
    private string _dark_Guidance_Key = "DarkGuidance";
    //第一次进入黑暗模式关卡的标志位，0代表第一次

    private int Is_First
    {
        set
        {
            if (Is_First == 0)
            {
                PlayerPrefs.SetInt(_dark_Guidance_Key, value);
            }
        }
        get { return PlayerPrefs.GetInt(_dark_Guidance_Key, 0); }
    }

    void OnEnable()
    {
        int number = int.Parse(MyKeys.MissionName.ToString());
        if (number == 17 && Is_First == 0)
        {
            Is_First = 1;
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
        }
    }
}
