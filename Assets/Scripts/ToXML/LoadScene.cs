using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using PathologicalGames;

public class LoadScene : MonoBehaviour
{

    #region 变量，属性
    private static LoadScene _instance;

    public static LoadScene Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LoadScene>();
            }
            return _instance;
        }
    }

    private int _spaceBetweenLoadMark;
    //两个加载标记间的距离
    public int SpaceBetweenLoadMark
    {
        get { return _spaceBetweenLoadMark; }
    }

    /// <summary>
    /// 加载下一段场景
    /// </summary>
    public void LoadNextScene()
    {
        _now_Scene_Mark_Index ++ ;
        CreatScene(_now_Scene_Mark_Index);
    }

    /// <summary>
    /// 将不用的场景片段隐藏留待复用
    /// </summary>
    public void InactiveScene()
    {
        SetInactive();
    }

    public ObjectPool EndlessPool { get { return _spawnPool; } }

    private ObjectPool _spawnPool;//内存池对象
    private List<Transform> _now_List;
    private List<Transform> _middle_List; 
    private List<Transform> _last_List;
    private SceneData[] _sceneDatas;
    private GameObject[] _prefabs;//预制体数组
    private int _now_Scene_Mark_Index;
    #endregion
    // Use this for initialization
    void Start()
    {
        //加载背景
        GameObject BG = Resources.Load("Prefab/prefab_New/BackGround/BG_Endless") as GameObject;
        GameObject BG_Copy = Instantiate(BG);
        BG_Copy.transform.position = new Vector3(-0.42f,-30f,1.17f);

        BG = null;
        BG_Copy = null;
        //两个加载标记间的距离
        _spaceBetweenLoadMark = 30;//一个场景片段的长度是30

        _prefabs = Resources.LoadAll<GameObject>("Prefab");

        _spawnPool = PoolManager.Pools["EndlessMode"];
        _now_List = new List<Transform>();
        _middle_List = new List<Transform>();
        _last_List = new List<Transform>();
        _sceneDatas = new SceneData[285];

        FromXmlLoadScene("Name");

    }

    private void FromXmlLoadScene(string xmlName)
    {
        string filepath = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            filepath = Application.streamingAssetsPath + "/"+xmlName+".xml";
        }
        else
        {
            filepath = "file://" + Application.streamingAssetsPath + "/" + xmlName + ".xml";
        }

        StartCoroutine(LoadXml(filepath,xmlName));
    }

    IEnumerator LoadXml(string path, string xmlName)
    {
        int index = 0;
        if (!xmlName.Contains("Name"))
        {
            index = int.Parse(xmlName);
        }

        if (index == 4)
        {
            StratToCreatScene();
        }

        WWW www = new WWW(path);
        yield return www;

        if (xmlName.Contains("Name"))
        {
            InitializePool(www);
            //从xml加载数据
            FromXmlLoadScene(index.ToString());
        }
        else if (index < 285)
        {
            ReadXMLAndSaveToClass(index,www);
            FromXmlLoadScene((index+1).ToString());
        }
        else
        {
           
        }
        
    }
    void InitializePool(WWW www)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(www.text);
        XmlNodeList nodeList = xmlDoc.SelectSingleNode("root").ChildNodes;
        foreach (XmlElement xmlelement in nodeList)
        {
            foreach (GameObject prefab in _prefabs)
            {
                if (prefab.name == xmlelement.Name)
                {
                    MyPool.InitializePool(/*内存池对象*/_spawnPool,/*预制体文件*/prefab,
                                  /*默认初始化Prefab数量*/4, /*实例化限制*/ false,  /*无限取Prefab*/true,  /*限制池子里最大的Prefab数量*/4,
                                  /*自动清理池子*/true,  /*最终保留*/4,  /*多久清理一次*/1, /*每次清理几个*/1);
                }

            }

        }
    }

    //创建标记实例，设置好第一个标记，撞到第一个标记，就把实例挪到下一个标记的位置
    void CreatMark()
    {
        float StartPoint = -31.33f;
        float mark_Y = StartPoint + 60;
        GameObject instance = StaticParameter.LoadObject("Other", "EndlessMark") as GameObject;
        GameObject copy = Instantiate(instance);
        copy.transform.position = instance.transform.position.x * Vector3.right + mark_Y * Vector3.up +
                                      instance.transform.position.z * Vector3.forward;
    }
    void StratToCreatScene()
    {
        //创建第一段场景
        CreatScene(0);
        CreatScene(1);
        CreatScene(2);
        _now_Scene_Mark_Index = 2;
        //创建加载下段场景的标记
        CreatMark();
    }

    void ReadXMLAndSaveToClass(int index, WWW www)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(www.text);

        SceneData sceneData = new SceneData();

        XmlNodeList nodeList =
            xmlDoc.SelectSingleNode("root").ChildNodes;

        foreach (XmlElement gameobjects in nodeList)
        {
            Vector3 pos = Vector3.zero;
            Vector3 rot = Vector3.zero;
            Vector3 sca = Vector3.zero;
            Vector3 colliderCenter = Vector3.zero;
            Vector3 colliderSize = Vector3.zero;
            string Tag = "";
            int Layer = 0;
            
            foreach (XmlElement myTransform in gameobjects.SelectSingleNode("transform").ChildNodes)
            {
                #region 根据数据赋值参数

                if (myTransform.Name == "position")
                {
                    foreach (XmlAttribute position in myTransform.Attributes)
                    {
                        switch (position.Name)
                        {
                            case "x":
                                pos.x = float.Parse(position.InnerText);
                                break;
                            case "y":
                                pos.y = float.Parse(position.InnerText);
                                break;
                            case "z":
                                pos.z = float.Parse(position.InnerText);
                                break;
                        }
                    }
                }
                else if (myTransform.Name == "rotation")
                {
                    foreach (XmlAttribute rotation in myTransform.Attributes)
                    {
                        switch (rotation.Name)
                        {
                            case "x":
                                rot.x = float.Parse(rotation.InnerText);
                                break;
                            case "y":
                                rot.y = float.Parse(rotation.InnerText);
                                break;
                            case "z":
                                rot.z = float.Parse(rotation.InnerText);
                                break;
                        }
                    }
                }
                else if (myTransform.Name == "scale")
                {
                    foreach (XmlAttribute scale in myTransform.Attributes)
                    {
                        switch (scale.Name)
                        {
                            case "x":
                                sca.x = float.Parse(scale.InnerText);
                                break;
                            case "y":
                                sca.y = float.Parse(scale.InnerText);
                                break;
                            case "z":
                                sca.z = float.Parse(scale.InnerText);
                                break;
                        }
                    }
                }
                else if (myTransform.Name == "Tag")
                {
                    Tag = myTransform.Attributes[0].InnerText;
                }
                else if (myTransform.Name == "Layer")
                {
                    Layer = int.Parse(myTransform.Attributes[0].InnerText);
                }

                if (gameobjects.Attributes[1].InnerText == "WideWallCollider")
                {
                    if (myTransform.Name == "colliderCenter")
                    {
                        foreach (XmlAttribute center in myTransform.Attributes)
                        {
                            switch (center.Name)
                            {
                                case "x":
                                    colliderCenter.x = float.Parse(center.InnerText);
                                    break;
                                case "y":
                                    colliderCenter.y = float.Parse(center.InnerText);
                                    break;
                                case "z":
                                    colliderCenter.z = float.Parse(center.InnerText);
                                    break;
                            }
                        }
                    }
                    else if (myTransform.Name == "colliderSize")
                    {
                        foreach (XmlAttribute size in myTransform.Attributes)
                        {
                            switch (size.Name)
                            {
                                case "x":
                                    colliderSize.x = float.Parse(size.InnerText);
                                    break;
                                case "y":
                                    colliderSize.y = float.Parse(size.InnerText);
                                    break;
                                case "z":
                                    colliderSize.z = float.Parse(size.InnerText);
                                    break;
                            }
                        }
                    }
                }

                #endregion
            }

            if (gameobjects.Attributes[1].InnerText == "WideWallCollider")
            {
                sceneData.SaveData(gameobjects.Attributes[1].InnerText, pos, rot, sca,
                Layer, Tag, colliderCenter, colliderSize);
            }
            else
            {
                sceneData.SaveData(gameobjects.Attributes[1].InnerText, pos, rot, sca,
                Layer, Tag);
            }

        }
        _sceneDatas[index] = sceneData;

    }

    void CreatScene(int mark)
    {
        GameObjectData gameObjectData;
        for (int i = 0; i < _sceneDatas[mark].Count; i++)
        {
            gameObjectData = _sceneDatas[mark].GetData(i);
            GameObject ob = CreatInstance(mark,gameObjectData._name, gameObjectData._position, Quaternion.Euler(gameObjectData._rotation)).gameObject;
            ob.transform.localScale = gameObjectData._scale;
            ob.tag = gameObjectData._tag;
            ob.layer = gameObjectData._layer;

            if (gameObjectData._name == "WideWallCollider")
            {
                BoxCollider collider = ob.GetComponent<BoxCollider>();
                collider.center = gameObjectData._collider_Center;
                collider.size = gameObjectData._collider_Size;
            }
            //StartCoroutine(Load(_sceneDatas[mark].GetData(i)));
        }

    }

    //创建实例
    Transform CreatInstance(int mark,string prefabName, Vector3 pos, Quaternion rot)
    {
        bool isContains = false;
        //检测内存池内是否含有指定预制体
        for (int i = 0; i < _spawnPool._perPrefabPoolOptions.Count; i++)
        {
            if (_spawnPool._perPrefabPoolOptions[i].prefab.name.Contains(prefabName))
            {
                isContains = true;
                break;
            }
        }

        if (!isContains)
        {
            MyPool.InitializePool(/*内存池对象*/_spawnPool,/*文件夹名称*/"Prefab", /*预制体名称*/prefabName,
           /*默认初始化Prefab数量*/2, /*实例化限制*/ false,  /*无限取Prefab*/true,  /*限制池子里最大的Prefab数量*/4,
           /*自动清理池子*/true,  /*最终保留*/3,  /*多久清理一次*/1, /*每次清理几个*/1);
        }

        Transform temp = _spawnPool.Active(prefabName, pos, rot);

        if (mark == 0)
        {
            _now_List.Add(temp);
        }
        else if(mark == 1)
        {
            _middle_List.Add(temp);
        }
        else
        {
            _last_List.Add(temp);
        }

        return temp;

    }

    void SetInactive()
    {
        for (int i = 0; i < _now_List.Count; i++)
        {
            if (_now_List[i] != null)
            {
                _spawnPool.Inactive(_now_List[i]);
            }
        }

        ChangeList();

    }

    void ChangeList()
    {
        _now_List.Clear();

        foreach (Transform tr in _middle_List)
        {
            _now_List.Add(tr);
        }

        _middle_List.Clear();

        foreach (Transform tr in _last_List)
        {
            _middle_List.Add(tr);
        }

        _last_List.Clear();
    }
}
