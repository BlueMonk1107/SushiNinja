using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Phantom : MonoBehaviour
{


    private float interval = 0.03f;   //每隔多久生成一个残影  
    private float lifeCycle = 0.4f;    //残影存在时间   
    private float lastCombinedTime = 0.0f;    //上一次组合的时间 
    private string normal_Shader_Path;


    MeshFilter[] meshFilters = null;    //存贮对象包含的MeshFilter组件  


    MeshRenderer[] meshRenderers = null;   //存贮对象包含的MeshRenderer组件  


    SkinnedMeshRenderer[] skinnedMeshRenderers = null; //存贮对象包含的skinnedMeshRenderers组件  


    List<GameObject> _phantomsList;    //存储残影  

    // Use this for initialization  
    void Start()
    {
        meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();
        skinnedMeshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        normal_Shader_Path = "Particles/Additive";
    }

    void OnEnable()
    {
       _phantomsList = new List<GameObject>();
    }
    void OnDisable()
    {
        if(_phantomsList == null)
            return;

        _phantomsList.Clear();
        _phantomsList = null;
    }
    // Update is called once per frame  
    void Update()
    {


        if (Time.time - lastCombinedTime > interval)
        {
            if ((HumanManager.WallMark == WallMark.Right && HumanManager.Nature.Human.position.x > HumanManager.Nature.Right_Wall_Inside)
                ||(HumanManager.WallMark == WallMark.Left && HumanManager.Nature.Human.position.x < HumanManager.Nature.Left_Wall_Inside))
            {
                return;
            }
            else
            {
                lastCombinedTime = Time.time;
                //控制skinnedMeshRenderers  
                for (int i = 0; skinnedMeshRenderers != null && i < skinnedMeshRenderers.Length; ++i)
                {
                    Mesh mesh = new Mesh();
                    //skinnedMeshRenderers.BakeMesh取出的mesh是原始mesh不会随动画改变  
                    skinnedMeshRenderers[i].BakeMesh(mesh);
                    GameObject phantom = new GameObject();
                    //设置layer层（可自行修改或去掉）  
                    phantom.layer = 21;
                    //物体是否被隐藏、保存在场景中或被用户修改？  
                    //HideFlags.HideAndDontSave; 不能显示在层级面板并且不能保存到场景。  
                    phantom.hideFlags = HideFlags.HideAndDontSave;
                    //将mesh赋给残影  
                    MeshFilter meshFilter = phantom.AddComponent<MeshFilter>();
                    meshFilter.mesh = mesh;
                    //将材质球赋给残影  
                    MeshRenderer meshRenderer = phantom.AddComponent<MeshRenderer>();
                    meshRenderer.material.shader = Shader.Find(normal_Shader_Path);
                    //残影的淡入淡出  
                    InitFadeInObj(phantom, skinnedMeshRenderers[i].transform.position
                            , skinnedMeshRenderers[i].transform.rotation, lifeCycle);
                }
                //控制meshFilters  
                for (int i = 0; meshFilters != null && i < meshFilters.Length; ++i)
                {
                    if (!meshFilters[i].Equals(null))
                    {
                        GameObject go = Instantiate(meshFilters[i].gameObject) as GameObject;
                        InitFadeInObj(go, meshFilters[i].transform.position
                                     , meshFilters[i].transform.rotation, lifeCycle);
                    }

                }
            }
            
        }
    }
    //残影的淡入淡出  对象、位置、旋转、存在时间  
    private void InitFadeInObj(GameObject phantom, Vector3 position, Quaternion rotation, float lifeCycle)
    {
        phantom.hideFlags = HideFlags.HideAndDontSave;
        phantom.transform.position = position;
        phantom.transform.rotation = rotation;


        FadeInOut fi = phantom.AddComponent<FadeInOut>();
        fi.lifeCycle = lifeCycle;

        //加入列表  
        _phantomsList.Add(phantom);
    }
}