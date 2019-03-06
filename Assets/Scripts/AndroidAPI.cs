using UnityEngine;
using System.Collections;

public class AndroidAPI : MonoBehaviour
{

    void Start()
    {
        //设置当前游戏体的名字，在Android中我们将使用这个名字
        this.name = "Ninja";
    }

    void OnGUI()
    {
        //通过API调用对话框
        if (GUILayout.Button("调用Android API显示对话框", GUILayout.Height(45)))
        {
            //获取Android的Java接口
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            //构造参数
            string[] mObject = new string[2];
            mObject[0] = "Unity3D";
            mObject[1] = "Unity3D成功调用Android API";
            //调用方法
            jo.Call("ShowDialog", mObject);
        }

    }
}