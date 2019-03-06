using System.Collections;
using UnityEngine;
using PathologicalGames;

public class MyPool : MonoBehaviour
{
    private static MyPool _instance;

    public static MyPool Instance
    {
        get { return _instance; }
    }
    private ObjectPool _spawnPool;//内存池对象
	// Use this for initialization
	void Start ()
	{
	    _instance = this;
        _spawnPool = PoolManager.Pools["MyPool"];
                    
        InitializePool(/*内存池对象*/_spawnPool,/*文件夹名称*/StaticParameter.s_Folder_GameEffect, /*预制体名称*/StaticParameter.s_Prefab_SwordLight, 
            /*默认初始化Prefab数量*/4, /*实例化限制*/ true,  /*无限取Prefab*/true,  /*限制池子里最大的Prefab数量*/4,
            /*自动清理池子*/false,  /*最终保留*/3,  /*多久清理一次*/1, /*每次清理几个*/1);
        InitializePool(/*内存池对象*/_spawnPool,/*文件夹名称*/StaticParameter.s_Folder_GameEffect, /*预制体名称*/StaticParameter.s_Prefab_Juice,
            /*默认初始化Prefab数量*/6, /*实例化限制*/ true,  /*无限取Prefab*/true,  /*限制池子里最大的Prefab数量*/6,
            /*自动清理池子*/false,  /*最终保留*/4,  /*多久清理一次*/1, /*每次清理几个*/2);
        InitializePool(/*内存池对象*/_spawnPool,/*文件夹名称*/StaticParameter.s_Folder_GameEffect, /*预制体名称*/StaticParameter.s_Prefab_Line,
            /*默认初始化Prefab数量*/3, /*实例化限制*/ false,  /*无限取Prefab*/true,  /*限制池子里最大的Prefab数量*/6,
            /*自动清理池子*/true,  /*最终保留*/2,  /*多久清理一次*/5, /*每次清理几个*/2);
        InitializePool(/*内存池对象*/_spawnPool,/*文件夹名称*/StaticParameter.s_Folder_GameEffect, /*预制体名称*/StaticParameter.s_Prefab_Fire,
            /*默认初始化Prefab数量*/3, /*实例化限制*/ false,  /*无限取Prefab*/true,  /*限制池子里最大的Prefab数量*/6,
            /*自动清理池子*/true,  /*最终保留*/2,  /*多久清理一次*/5, /*每次清理几个*/2);
    }

    public static void InitializePool(ObjectPool spawnPool,string folderName,string prefabName, 
        int preloadAmount,bool limitInstances,bool limitFIFO,int limitAmount,
        bool cullDespawned,int cullAbove,int cullDelay,int cullMaxPerPass)
    {
        //加载资源
        Transform temp = ((GameObject)StaticParameter.LoadObject(folderName, prefabName)).transform;

        PrefabPool refabPool = new PrefabPool(temp)
        {
            preloadAmount = preloadAmount,      //默认初始化两个Prefab
            limitInstances = limitInstances,    //开启限制
            limitFIFO = limitFIFO,              //关闭无限取Prefab
            limitAmount = limitAmount,          //限制池子里最大的Prefab数量
            AutoDestroy = cullDespawned,        //开启自动清理池子
            DestroyExceptTheNumber = cullAbove,       //最终保留
            DestroyEachTime = cullDelay,              //多久清理一次
            DestroyNumberEverytime = cullMaxPerPass   //每次清理几个
        };
        
        //初始化内存池
        spawnPool._perPrefabPoolOptions.Add(refabPool);
        spawnPool.CreatePrefabPool(spawnPool._perPrefabPoolOptions[spawnPool._perPrefabPoolOptions.Count-1]);
    }
    public static void InitializePool(ObjectPool spawnPool, GameObject prefab,
      int preloadAmount, bool limitInstances, bool limitFIFO, int limitAmount,
      bool cullDespawned, int cullAbove, int cullDelay, int cullMaxPerPass)
    {
        //加载资源
        Transform temp = prefab.transform;

        PrefabPool refabPool = new PrefabPool(temp)
        {
            preloadAmount = preloadAmount,      //默认初始化两个Prefab
            limitInstances = limitInstances,    //开启限制
            limitFIFO = limitFIFO,              //关闭无限取Prefab
            limitAmount = limitAmount,          //限制池子里最大的Prefab数量
            AutoDestroy = cullDespawned,        //开启自动清理池子
            DestroyExceptTheNumber = cullAbove,       //最终保留
            DestroyEachTime = cullDelay,              //多久清理一次
            DestroyNumberEverytime = cullMaxPerPass   //每次清理几个
        };

        //初始化内存池
        spawnPool._perPrefabPoolOptions.Add(refabPool);
        spawnPool.CreatePrefabPool(spawnPool._perPrefabPoolOptions[spawnPool._perPrefabPoolOptions.Count - 1]);
    }

    /// <summary>
    /// 调用内存池内的实例
    /// </summary>
    /// <param name="prefabName"></param>
    public void Spawn(string prefabName,Vector3 position,Quaternion rotation)
    {
        float time = 0;
        switch (prefabName)
        {
            case "SwordLight":
                time = 0.2f;
                break;
            case "juice":
                time = 0.3f;
                break;
            case "line":
                time = 0.9f;
                break;
            default:
                Debug.Log("内存池内无此预制体");
                break;
        }
        if (time > 0)
        {
            Transform clone = _spawnPool.Active(prefabName, position,rotation);
            _spawnPool.Inactive(clone, time);
        }
        
    }

    public Transform Spawn(string prefabName)
    {
        return _spawnPool.Active(prefabName);
    }

    public void Despawn(Transform clone)
    {
        _spawnPool.Inactive(clone);
    }


}
