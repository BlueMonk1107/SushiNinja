using System;
using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEditor;

public class ApplyAllPrefabs : ScriptableWizard
{

    [UnityEditor.MenuItem("Tools/Change Prefabs")]
    static void UpdatePrefabs()
    {
        ScriptableWizard.DisplayWizard<ApplyAllPrefabs>("Change all prefabs", "NOW");
    }

    void OnWizardCreate()
    {
        //GameObject[] ObstacleManagers = GameObject.FindGameObjectsWithTag("ObstacleManager");

        //foreach (GameObject child in ObstacleManagers)
        //{
        //    foreach (Transform transform in child.transform)
        //    {

        //        //修正障碍
        //        if (transform.gameObject.layer == MyTags.Obstacle_Layer)
        //        {
        //            if (transform.position.x < 0)
        //            {
        //                transform.tag = MyTags.Left_Tag;
        //                transform.localScale = Vector3.one * 10;
        //                transform.localEulerAngles = new Vector3(90, 180, -180);
        //                transform.position = new Vector3(-2.3f, transform.position.y, transform.position.z);
        //            }
        //            else if (transform.position.x > 0)
        //            {
        //                transform.tag = MyTags.Right_Tag;
        //                transform.localScale = Vector3.one * 10;
        //                transform.localEulerAngles = new Vector3(-90, 180, 0);
        //                transform.position = new Vector3(2.4f, transform.position.y, transform.position.z);
        //            }
        //            else
        //            {
        //                Debug.Log("没有标签");
        //            }
        //        }
        //        //修正移动障碍
        //        if (transform.gameObject.layer == MyTags.Sword_Layer)
        //        {
        //            if (transform.tag == MyTags.Left_Tag)
        //            {
        //                transform.localScale = Vector3.one;
        //                transform.localEulerAngles = Vector3.zero;
        //                transform.position = new Vector3(-4.3f, transform.position.y, transform.position.z);
        //            }
        //            else if (transform.tag == MyTags.Right_Tag)
        //            {
        //                transform.localScale = Vector3.one;
        //                transform.localEulerAngles = new Vector3(0, 180, 0);
        //                transform.position = new Vector3(4.3f, transform.position.y, transform.position.z);
        //            }
        //            else
        //            {
        //                Debug.Log("没有标签");
        //            }
        //        }

        //    }
        //}

        GameObject[] fruitManagers = GameObject.FindGameObjectsWithTag("FruitManager");
        GameObject shoulijian = StaticParameter.LoadObject("Prefab/New", "shoulijian") as GameObject;

        foreach (GameObject i in fruitManagers)
        {
            foreach (Transform tr in i.transform)
            {
                GameObject temp = Instantiate(shoulijian);
                temp.transform.position = tr.position;
                temp.transform.parent = tr.parent;
            }
        }
    }

}
