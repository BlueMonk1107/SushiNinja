using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FruitScript))]
public class NewBehaviourScript : Editor
{
    // Use this for initialization
    void Start()
    {
        //EditorUtility.SetDirty(one);
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //var temp = target as FruitScript;
        //string path = "Fruit";
        //GameObject[] temp_List = Resources.LoadAll<GameObject>(path);

        //for (int i = 0; i < temp_List.Length; i++)
        //{
        //    if (temp.name.Contains(temp_List[i].name))
        //    {
        //        PrefabUtility.ReplacePrefab(temp.gameObject, PrefabUtility.GetPrefabParent(temp.gameObject), ReplacePrefabOptions.ConnectToPrefab);
        //    }

        //}


    }
}

