using UnityEngine;
using UnityEditor;
using System.Xml;
using System.IO;
using UnityEditor.SceneManagement;

public class GetColliderData : Editor
{
    private static float StartPoint = -31.33f;
    private static int _mark;
    [MenuItem("Assets/Export Scene ColliderData To XML From Selection")]
    static void ExportXML()
    {
        string path = EditorUtility.SaveFilePanel("Save Resource", "Assets/StreamingAssets", "ColliderData", "xml");

        if (path.Length != 0)
        {
            Object[] selectedAssetList = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            
            //遍历所有的游戏对象
            foreach (Object selectObject in selectedAssetList)
            {
                // 场景名称
                string sceneName = selectObject.name;
                // 场景路径
                string scenePath = AssetDatabase.GetAssetPath(selectObject);
                // 场景文件
                //string xmlPath = path; //Application.dataPath + "/AssetBundles/Prefab/Scenes/" + sceneName + ".xml";
                // 如果存在场景文件，删除
                if (File.Exists(path)) File.Delete(path);
                // 打开这个关卡
                EditorSceneManager.OpenScene(scenePath);
                XmlDocument xmlDocument = new XmlDocument();
                // 创建XML属性
                XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDocument.AppendChild(xmlDeclaration);
                // 创建XML根标志
                XmlElement rootXmlElement = xmlDocument.CreateElement("root");
                // 创建场景标志
                XmlElement sceneXmlElement = xmlDocument.CreateElement("scene");
                sceneXmlElement.SetAttribute("sceneName", sceneName);

                //声明变量
                XmlElement oneXmlElement = xmlDocument.CreateElement("fragment");
                int mark = 0;
                
                foreach (GameObject sceneObject in FindObjectsOfType(typeof(GameObject)))
                {
                    mark = (int)((sceneObject.transform.position.y - StartPoint)/30);
                    oneXmlElement = xmlDocument.CreateElement("fragment" + mark);
                    //DebugMaxNumber(mark);//284

                    // 如果对象是激活状态
                    if (sceneObject.activeSelf)
                    {
                        // 判断父物体是否存在且是否为预制体，并判断此物体是否是预制体
                        if ((sceneObject.transform.parent == null||PrefabUtility.GetPrefabType(sceneObject.transform.parent) != PrefabType.PrefabInstance)
                            &&PrefabUtility.GetPrefabType(sceneObject) == PrefabType.PrefabInstance)
                        {
                            // 获取引用预设对象
                            Object prefabObject = PrefabUtility.GetPrefabParent(sceneObject);
                            if (prefabObject != null && prefabObject.name.Contains("WideWallCollider"))
                            {
                                XmlElement gameObjectXmlElement = xmlDocument.CreateElement("gameObject");
                                gameObjectXmlElement.SetAttribute("objectName", sceneObject.name);
                                gameObjectXmlElement.SetAttribute("objectAsset", prefabObject.name);

                                XmlElement transformXmlElement = xmlDocument.CreateElement("transform");
                                // 位置信息
                                XmlElement positionXmlElement = xmlDocument.CreateElement("position");
                                positionXmlElement.SetAttribute("x", sceneObject.transform.position.x.ToString());
                                positionXmlElement.SetAttribute("y", sceneObject.transform.position.y.ToString());
                                positionXmlElement.SetAttribute("z", sceneObject.transform.position.z.ToString());

                                // 旋转信息
                                XmlElement rotationXmlElement = xmlDocument.CreateElement("rotation");
                                rotationXmlElement.SetAttribute("x", sceneObject.transform.rotation.eulerAngles.x.ToString());
                                rotationXmlElement.SetAttribute("y", sceneObject.transform.rotation.eulerAngles.y.ToString());
                                rotationXmlElement.SetAttribute("z", sceneObject.transform.rotation.eulerAngles.z.ToString());

                                // 缩放信息
                                XmlElement scaleXmlElement = xmlDocument.CreateElement("scale");
                                scaleXmlElement.SetAttribute("x", sceneObject.transform.localScale.x.ToString());
                                scaleXmlElement.SetAttribute("y", sceneObject.transform.localScale.y.ToString());
                                scaleXmlElement.SetAttribute("z", sceneObject.transform.localScale.z.ToString());

                                //标签信息
                                XmlElement tagXmlElement = xmlDocument.CreateElement("Tag");
                                tagXmlElement.SetAttribute("tag", sceneObject.tag);

                                //层级信息
                                XmlElement layerXmlElement = xmlDocument.CreateElement("Layer");
                                layerXmlElement.SetAttribute("layer", sceneObject.layer.ToString());

                                //碰撞框的中心信息
                                BoxCollider temp = sceneObject.transform.GetComponent<BoxCollider>();

                                XmlElement colliderCenterElement = xmlDocument.CreateElement("colliderCenter");
                                colliderCenterElement.SetAttribute("x", temp.center.x.ToString());
                                colliderCenterElement.SetAttribute("y", temp.center.y.ToString());
                                colliderCenterElement.SetAttribute("z", temp.center.z.ToString());
                                
                                //碰撞框的大小信息
                                XmlElement colliderSizeElement = xmlDocument.CreateElement("colliderSize");
                                colliderSizeElement.SetAttribute("x", temp.size.x.ToString());
                                colliderSizeElement.SetAttribute("y", temp.size.y.ToString());
                                colliderSizeElement.SetAttribute("z", temp.size.z.ToString());

                                transformXmlElement.AppendChild(positionXmlElement);
                                transformXmlElement.AppendChild(rotationXmlElement);
                                transformXmlElement.AppendChild(scaleXmlElement);
                                transformXmlElement.AppendChild(tagXmlElement);
                                transformXmlElement.AppendChild(layerXmlElement);
                                transformXmlElement.AppendChild(colliderCenterElement);
                                transformXmlElement.AppendChild(colliderSizeElement);

                                gameObjectXmlElement.AppendChild(transformXmlElement);

                                oneXmlElement.AppendChild(gameObjectXmlElement);
                                sceneXmlElement.AppendChild(oneXmlElement);
                            }
                        }
                    }
                }
                
                rootXmlElement.AppendChild(sceneXmlElement);
                xmlDocument.AppendChild(rootXmlElement);
                // 保存场景数据
                xmlDocument.Save(path);
                // 刷新Project视图
                AssetDatabase.Refresh();
            }
        }
    }

    static void DebugMaxNumber(int num)
    {
        if (_mark < num)
        {
            _mark = num;
        }

        Debug.Log(_mark);
    }
}