using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FPS : MonoBehaviour
{
    private float fps;
    private Text _text;
	// Use this for initialization
	void Start ()
	{
	    _text = transform.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        try
        {
            fps = 1 / Time.smoothDeltaTime;
            int temp = (int) fps;
            _text.text = temp + "帧";
        }
        catch (Exception)
        {

        }

    }

}
