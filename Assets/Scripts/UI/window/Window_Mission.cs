using System;
using UnityEngine;
using System.Collections;

public class Window_Mission : MonoBehaviour
{
    private GameObject _mission_Camera;
    private GameObject _main_Camera;
	// Use this for initialization
    void Awake()
    {
        _main_Camera = Camera.main.gameObject;
    }
	void Start ()
	{
	   
	}

    void OnEnable()
    {
        try
        {
           
            if (_mission_Camera == null)
            {
                GameObject temp = Resources.Load(UIResourceDefine.MainUIPrefabPath + "TheMission") as GameObject;
                _mission_Camera = Instantiate(temp);
            }
            else
            {
                _mission_Camera.SetActive(true);
            }
            _main_Camera.SetActive(false);
        }
        catch (Exception)
        {
            
           Debug.Log("有问题");
        }
        

       
    }

    void OnDisable()
    {
        try
        {
            _main_Camera.SetActive(true);
            _mission_Camera.SetActive(false);
        }
        catch (Exception)
        {
            
            Debug.Log("有问题");
        }
        
    }

}
