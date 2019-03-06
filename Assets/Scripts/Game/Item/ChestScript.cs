using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChestScript : MonoBehaviour {

    Camera _my_Camera;  //主摄像机
    GameObject _text; //切水果后的分数预制
    GameObject _canvas; //游戏界面的画布

    // Use this for initialization
    void Start () {
        _my_Camera = Camera.main;
        _text = (GameObject)StaticParameter.LoadObject("Other", "Text");
        _canvas = GameObject.FindWithTag("Canvas");
        //判断变量是否为空
        StaticParameter.JudgeNull("_canvas", _canvas);
    }
	
    public void Excute()
    {
        Excute(transform, _my_Camera, _text, _canvas);
    }

    void Excute(Transform chest, Camera camera, GameObject text, GameObject canvas)
    {
        Vector3 score_Position = chest.position + Vector3.up * (chest.transform.localScale.y * 0.5f);
        Vector2 position = camera.WorldToScreenPoint(score_Position);

        GameObject text_Copy = Instantiate(text);
        text_Copy.transform.SetParent(canvas.transform, true);
        text_Copy.transform.position = position;
        Destroy(text_Copy, 0.2f);

        MyKeys.Game_Score += 10;
        try
        {
            Window_Ingame.Instance.ShowGold(MyKeys.Game_Score.ToString());
        }
        catch (Exception)
        {
            throw;
        }

        Text text_Component = text_Copy.transform.GetComponent<Text>();
        TextEffect(text_Component);
    }

    void TextEffect(Text text_Com)
    {
        text_Com.text = "+10";
        text_Com.color -= Color.black * 0.05f;
        text_Com.transform.position += Vector3.up * 2f;
    }
}
