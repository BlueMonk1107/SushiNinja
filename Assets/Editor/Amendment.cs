using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class Amendment : Editor
{
    [MenuItem("Tools/Amendment &Q")]
    static void Change()
    {
        List<GameObject> shoulijian_List = new List<GameObject>();
        GameObject[] selectedObjects = Selection.gameObjects;
        foreach (GameObject selectedObject in selectedObjects)
        {
            if (selectedObject.name.Contains("shoulijian"))
            {
                shoulijian_List.Add(selectedObject);
            }
        }

        for (int i = 0; i < shoulijian_List.Count; i++)
        {
            for (int j = i; j < shoulijian_List.Count; j++)
            {
                if (shoulijian_List[i].transform.position.y > shoulijian_List[j].transform.position.y)
                {
                    var temp = shoulijian_List[i];
                    shoulijian_List[i] = shoulijian_List[j];
                    shoulijian_List[j] = temp;
                }
            }
        }
        if (shoulijian_List.Count > 2)
        {
            float different_X = shoulijian_List[1].transform.position.x - shoulijian_List[0].transform.position.x;
            float different_Y = shoulijian_List[1].transform.position.y - shoulijian_List[0].transform.position.y;

            for (int i = 2; i < shoulijian_List.Count; i++)
            {
                shoulijian_List[i].transform.position = shoulijian_List[i - 1].transform.position +
                                                        Vector3.right*different_X + Vector3.up*different_Y;
            }
        }
    }
}
