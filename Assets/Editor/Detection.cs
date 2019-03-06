using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class Detection : Editor {

    [MenuItem("Tools/Detection &E")]
    static void Change()
    {
        GameObject select_GameObject = null;
        if (Selection.gameObjects.Length == 1)
        {
            select_GameObject = Selection.gameObjects[0];
        }
        else
        {
            Debug.Log("请选中一个父物体，不要多选");
            return;
        }
        
        object[] gameObjects;
        List<object> selectedObjects = new List<object>();
        gameObjects = FindObjectsOfType(typeof(Transform));
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if(((Transform)gameObjects[i]).parent != select_GameObject.transform)
                continue;
            for (int j = i+1; j < gameObjects.Length; j++)
            {
                if (((Transform) gameObjects[i]).position == ((Transform) gameObjects[j]).position)
                {
                    if (((Transform)gameObjects[j]).parent != select_GameObject.transform)
                        continue;
                    selectedObjects.Add(gameObjects[j]);
                }
            }
        }

        if (selectedObjects.Count != 0)
        {
            GameObject[] temp = new GameObject[selectedObjects.Count];

            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = ((Transform)selectedObjects[i]).gameObject;
            }
            Selection.objects = temp;
        }
        else
        {
            Debug.Log("没有重合物体");
        }
       
    }
}
