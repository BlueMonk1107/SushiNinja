using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpdateSceneToPrefabs : ScriptableWizard
{

    [UnityEditor.MenuItem("Tools/Update Prefabs")]
    static void UpdatePrefabs()
    {
        Debug.Log("UpdatePrefabs");
        ScriptableWizard.DisplayWizard<UpdateSceneToPrefabs>("Update (apply) all prefabs", "NOW");
    }

    private Dictionary<string, GameObject> prefabItems;
    private Dictionary<string, GameObject> sceneItems;
    private int foundToChange;
    private int changeSuccess;
    private int changeFailed;

    void OnWizardCreate()
    {
        Debug.Log("Replace all selected scene prefabs to library prefabs.");
        foundToChange = changeSuccess = changeFailed = 0;
        bool doContinue = loadLibraryAssets();
        if (doContinue) doContinue = loadSceneAsset();
        if (doContinue)
        {
            foreach (KeyValuePair<string, GameObject> pair in sceneItems)
            {
                GameObject sceneItem = pair.Value;
                if (prefabItems.ContainsKey(pair.Key))
                {
                    changeSuccess++;
                    var libraryPrefab = prefabItems[pair.Key];
                    PrefabUtility.ReplacePrefab(sceneItem, libraryPrefab);
                }
                else
                {
                    changeFailed++;
                    Debug.LogWarning("Prefab Warning: Scene item '" + pair.Key + "' has no prefab");
                }
            }
        }

        EditorUtility.DisplayDialog("Completed", "Replaced '" + changeSuccess + "' prefabs from selected scene items.", "Close");
        Debug.Log("Looked thru '" + foundToChange + "', replaced '" + changeSuccess + "', errors '" + changeFailed + "'");
    }
    //查找场景中的预制体源文件
    private bool loadLibraryAssets()
    {
        prefabItems = new Dictionary<string, GameObject>();
        //string[] list = AssetDatabase.GetAllAssetPaths();
        string path = "Fruit";
        GameObject[] temp_List = Resources.LoadAll<GameObject>(path);
        foreach (GameObject item in temp_List)
        {

            //bool isPrefab = (((path.Length > 6) ? path.Substring(path.Length - 7, 7) : "") == ".prefab");
            //if (isPrefab)
            //{
            //    Debug.Log(path);
            //    GameObject item = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            //    string name = item.name;
            //    if (prefabItems.ContainsKey(name))
            //    {
            //        Debug.LogWarning("Asset already exist, duplicate?: " + name);
            //        if (EditorUtility.DisplayDialog("Warning", "Prefab '" + name + "' is already saved, must be unique names!", "Continue", "Abort"))
            //        {
            //            return false;
            //        }
            //    }
            //    prefabItems[name] = item;
            //}

            string name = item.name;
            if (prefabItems.ContainsKey(name))
            {
                Debug.LogWarning("Asset already exist, duplicate?: " + name);
                if (EditorUtility.DisplayDialog("Warning", "Prefab '" + name + "' is already saved, must be unique names!", "Continue", "Abort"))
                {
                    return false;
                }
            }
            prefabItems[name] = item;
        }
        return true;
    }

    private bool loadSceneAsset()
    {
        sceneItems = new Dictionary<string, GameObject>();
        GameObject[] list = Selection.gameObjects;

        if (list.Length == 0)
        {
            EditorUtility.DisplayDialog("Warning", "You must select Prefab connected GameObjects in Hierachy window!", "Abort");
            return false;
        }

        foreach (GameObject obj in list)
        {
            // make sure we have root
            GameObject item = PrefabUtility.FindPrefabRoot(obj);

            string name = item.name;
            if (!sceneItems.ContainsKey(name))
            {
                foundToChange++;
                sceneItems[name] = obj;
            }
        }
        return true;
    }

}