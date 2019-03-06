using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Xml;

public class InsideOfData : Editor {

    [MenuItem("Tools/XML  InsideOfData")]
    private static void ChangeData()
    {
        string sourcePath = Application.dataPath + "/StreamingAssets" + "/EndlessMode_Base.xml";
        string filePath = Application.dataPath + "/StreamingAssets" + "/WideWallData.xml";

        //如果文件存在话开始解析。
        if (File.Exists(filePath) && File.Exists(sourcePath))
        {
            ////读取源文档的数据
            //SceneData sceneData = new SceneData();

            //XmlDocument xmlDoc_Base = new XmlDocument();
            //xmlDoc_Base.Load(sourcePath);
            //for (int i = 0; i < 285; i++)
            //{
            //    XmlNodeList nodeList_Base =
            //        xmlDoc_Base.SelectSingleNode("root").SelectSingleNode("scene").SelectNodes("fragment" + i);
            //    //    XmlNodeList nodeList_Base =
            //    //xmlDoc_Base.SelectSingleNode("root").ChildNodes;

            //    foreach (XmlElement gameobjects in nodeList_Base)
            //    {
            //        Vector3 pos = Vector3.zero;
            //        Vector3 rot = Vector3.zero;
            //        Vector3 sca = Vector3.zero;
            //        string Tag = "";
            //        int Layer = 0;

            //        foreach (XmlElement myTransform in gameobjects.SelectSingleNode("gameObject").SelectSingleNode("transform").ChildNodes)
            //        {
            //            #region 根据数据赋值参数

            //            if (gameobjects.SelectSingleNode("gameObject").Attributes[1].InnerText == "Obj_zhangai02")
            //            {
            //                if (myTransform.Name == "position")
            //                {
            //                    foreach (XmlAttribute position in myTransform.Attributes)
            //                    {
            //                        switch (position.Name)
            //                        {
            //                            case "x":
            //                                pos.x = float.Parse(position.InnerText);
            //                                break;
            //                            case "y":
            //                                pos.y = float.Parse(position.InnerText);
            //                                break;
            //                            case "z":
            //                                pos.z = float.Parse(position.InnerText);
            //                                break;
            //                        }
            //                    }
            //                }
            //                else if (myTransform.Name == "rotation")
            //                {
            //                    foreach (XmlAttribute rotation in myTransform.Attributes)
            //                    {
            //                        switch (rotation.Name)
            //                        {
            //                            case "x":
            //                                rot.x = float.Parse(rotation.InnerText);
            //                                break;
            //                            case "y":
            //                                rot.y = float.Parse(rotation.InnerText);
            //                                break;
            //                            case "z":
            //                                rot.z = float.Parse(rotation.InnerText);
            //                                break;
            //                        }
            //                    }
            //                }
            //                else if (myTransform.Name == "scale")
            //                {
            //                    foreach (XmlAttribute scale in myTransform.Attributes)
            //                    {
            //                        switch (scale.Name)
            //                        {
            //                            case "x":
            //                                sca.x = float.Parse(scale.InnerText);
            //                                break;
            //                            case "y":
            //                                sca.y = float.Parse(scale.InnerText);
            //                                break;
            //                            case "z":
            //                                sca.z = float.Parse(scale.InnerText);
            //                                break;
            //                        }
            //                    }
            //                }
            //                else if (myTransform.Name == "Tag")
            //                {
            //                    Tag = myTransform.Attributes[0].InnerText;
            //                }
            //                else if (myTransform.Name == "Layer")
            //                {
            //                    Layer = int.Parse(myTransform.Attributes[0].InnerText);
            //                }

            //                if (Tag == "Left")
            //                {
            //                    sceneData.SaveData(gameobjects.SelectSingleNode("gameObject").Attributes[1].InnerText, pos, rot, sca,
            //           Layer, Tag);
            //                }
            //            }

            //            #endregion
            //        }
            //    }
            //}

            //Debug.Log(sceneData.GetData(0)._scale);
            //修改目标文档的数据
            int mark = 0;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            for (int i = 0; i < 285; i++)
            {
                XmlNodeList nodeList =
                    xmlDoc.SelectSingleNode("root").SelectSingleNode("scene").SelectNodes("fragment" + i);

                foreach (XmlElement gameobjects in nodeList)
                {
                    if (gameobjects.SelectSingleNode("gameObject") == null)
                    {
                        continue;
                    }

                    //删除节点
                    //if (gameobjects.SelectSingleNode("gameObject").SelectSingleNode("transform").SelectSingleNode("Tag").Attributes[0].InnerText == "Right")
                    //{
                    //    gameobjects.RemoveAll();
                    //}
                    //改名
                    //gameobjects.SelectSingleNode("gameObject").Attributes[1].InnerText = "obj_zhangaila_wujin";


                    foreach (XmlElement myTransform in
                        gameobjects.SelectSingleNode("gameObject").SelectSingleNode("transform").ChildNodes)
                    {
                        //    if (gameobjects.SelectSingleNode("gameObject").Attributes[1].InnerText == "obj_zhangaila_wujin")
                        //    {
                        //if (myTransform.Name == "scale")
                        //{
                        //    if (gameobjects.SelectSingleNode("gameObject").SelectSingleNode("transform").SelectSingleNode("position").Attributes[1].InnerText ==
                        //    sceneData.GetData(mark)._position.y.ToString())
                        //    {
                        //        foreach (XmlAttribute scale in myTransform.Attributes)
                        //        {
                        //            switch (scale.Name)
                        //            {
                        //                case "x":
                        //                    scale.InnerText = sceneData.GetData(mark)._scale.x.ToString();
                        //                    break;
                        //                case "y":
                        //                    scale.InnerText = sceneData.GetData(mark)._scale.y.ToString();
                        //                    break;
                        //                case "z":
                        //                    scale.InnerText = sceneData.GetData(mark)._scale.z.ToString();
                        //                    break;
                        //            }
                        //        }
                        //        mark++;
                        //    }
                        //}
                        //gameobjects.RemoveAll();
                        //if (myTransform.Name == "position")
                        //{
                        //    foreach (XmlAttribute position in myTransform.Attributes)
                        //    {
                        //        switch (position.Name)
                        //        {
                        //            case "x":
                        //                //position.InnerText = sceneData.GetData(mark)._position.x.ToString();
                        //                break;
                        //            case "y":
                        //                position.InnerText = sceneData.GetData(mark)._position.y.ToString();
                        //                break;
                        //            case "z":
                        //                //position.InnerText = sceneData.GetData(mark)._position.z.ToString();
                        //                break;
                        //        }
                        //    }
                        //}
                        if (myTransform.Name == "rotation")
                    {
                        foreach (XmlAttribute rotation in myTransform.Attributes)
                        {
                            switch (rotation.Name)
                            {
                                case "x":
                                    rotation.InnerText = "0";
                                    break;
                                case "y":
                                    rotation.InnerText = "-90";
                                    break;
                                case "z":
                                    rotation.InnerText = "0";
                                    break;
                            }
                        }
                    }

                    //if (myTransform.Name == "Tag")
                    //{
                    //    if (myTransform.Attributes[0].InnerText == "Left")
                    //    {
                    //        gameobjects.SelectSingleNode("gameObject").Attributes[1].InnerText = "Obstacle_zhangai";
                    //    }
                    //    else
                    //    {
                    //        gameobjects.SelectSingleNode("gameObject").Attributes[1].InnerText = "Obstacle_zhangai 1";
                    //    }
                    //}
                    //    }

                    }

                }
            }

            xmlDoc.Save(filePath);
            Debug.Log("End");
        }
        else
        {
            Debug.Log("文件不存在");
        }
    }

    //static void ChangeXML()
    //{
    //    string sourcePath = Application.dataPath + "/StreamingAssets" + "/WideWallData.xml";
    //    string filePath = Application.dataPath + "/StreamingAssets" + "/EndlessMode.xml";

    //    if (File.Exists(filePath) && File.Exists(sourcePath))
    //    {
    //        XmlDocument baseDocument = new XmlDocument();
    //        baseDocument.Load(sourcePath);


    //        XmlDocument childDocument = new XmlDocument();
    //        XmlDeclaration xmlDeclaration = childDocument.CreateXmlDeclaration("1.0", "us-ascii", null);
    //        childDocument.AppendChild(xmlDeclaration);
    //        XmlElement rootXmlElement = childDocument.CreateElement("root");

    //        XmlNodeList nodeList =
    //            baseDocument.SelectSingleNode("root").SelectSingleNode("scene").SelectNodes("fragment" + i);
    //        for (int j = 0; j < nodeList.Count; j++)
    //        {
    //            if (nodeList[j].SelectSingleNode("gameObject") == null)
    //                continue;
    //            rootXmlElement.AppendChild(childDocument.ImportNode(nodeList[j].SelectSingleNode("gameObject"), true));
    //        }

    //        childDocument.AppendChild(rootXmlElement);
    //        childDocument.Save(path);


    //    }
    //}
}
