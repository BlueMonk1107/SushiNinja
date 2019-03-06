using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ChangeRotate : MonoBehaviour
{
    public Vector3 角度;
    public Transform 道具父物体;
    public Transform 导弹父物体;
    public enum MyEnum
    {
        空,
        无敌,
        双倍,
        冲刺,
        磁铁,
        护盾,
        导弹
    }

    public MyEnum 选择道具;
    public bool 开始调整角度;


    // Use this for initialization
    void Start () {
	
	}
    [ExecuteInEditMode]
    // Update is called once per frame
    void Update ()
    {
        if (开始调整角度)
        {
            string name = null;
            switch (选择道具)
            {
                case MyEnum.空:
                    Debug.Log("未选择道具");
                    break;
                case MyEnum.冲刺:
                    name = "item-dash";
                    break;
                case MyEnum.双倍:
                    name = "item-change";
                    break;
                case MyEnum.导弹:
                    name = "Missile";
                    break;
                case MyEnum.护盾:
                    name = "item-shield";
                    break;
                case MyEnum.无敌:
                    name = "item-burst";
                    break;
                case MyEnum.磁铁:
                    name = "item-magnet";
                    break;
            }
            List<Transform> list = FindObject(name, 选择道具);
            foreach (Transform item in list)
            {
                item.eulerAngles = 角度;
            }
            开始调整角度 = false;
        }
        
    }

    List<Transform> FindObject(string name,MyEnum temp)
    {
        List<Transform> list = new List<Transform>();
        Transform father = null;
        try
        {
            if (temp == MyEnum.导弹)
            {
                father = 导弹父物体;
            }
            else
            {
                father = 道具父物体;
            }

        }
        catch (Exception)
        {
            Debug.Log("没有找到道具父物体");
        }
        if (father != null)
        {
            foreach (Transform item in father)
            {
                if (item.name.Contains(name))
                {
                    list.Add(item);
                }
            }
        }
        return list;
    }
}
