using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Xml;

public class ChangeObjectData : Editor
{
    [MenuItem("Tools/XML  ChangeObjectData")]
    private static void ChangeData()
    {
        string filepath = Application.dataPath + "/StreamingAssets" + "/EndlessMode.xml";

        //如果文件存在话开始解析。
        if (File.Exists(filepath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filepath);
            for (int i = 0; i < 285; i++)
            {
                XmlNodeList nodeList =
                    xmlDoc.SelectSingleNode("root").SelectSingleNode("scene").SelectNodes("fragment" + i);

                foreach (XmlElement gameobjects in nodeList)
                {
                    if (gameobjects.SelectSingleNode("gameObject") == null)
                        continue;
                    //if (gameobjects.SelectSingleNode("gameObject").Attributes[1].InnerText == "Obj_shugan")
                    //{
                    //    if (gameobjects.SelectSingleNode("gameObject")
                    //              .SelectSingleNode("transform")
                    //              .SelectSingleNode("Tag")
                    //              .Attributes[0].InnerText == "Left")
                    //    {
                    //        gameobjects.SelectSingleNode("gameObject")
                    //              .SelectSingleNode("transform")
                    //              .SelectSingleNode("position")
                    //              .Attributes[0].InnerText = "-3";
                    //    }

                    //    //    if (gameobjects.SelectSingleNode("gameObject")
                    //    //              .SelectSingleNode("transform")
                    //    //              .SelectSingleNode("Tag")
                    //    //              .Attributes[0].InnerText == "Right")
                    //    //    {
                    //    //        gameobjects.SelectSingleNode("gameObject")
                    //    //              .SelectSingleNode("transform")
                    //    //              .SelectSingleNode("position")
                    //    //              .Attributes[0].InnerText = "3.084427";
                    //    //    }
                    //}

                    foreach (XmlElement myTransform in
                            gameobjects.SelectSingleNode("gameObject").SelectSingleNode("transform").ChildNodes)
                    {
                        if (gameobjects.SelectSingleNode("gameObject").Attributes[1].InnerText == "obj_zhangaira_wujin")
                        {
                            //gameobjects.SelectSingleNode("gameObject").Attributes[1].InnerText = "obj_zhangaira_wujin";

                            //if (myTransform.Name == "position")
                            //{
                            //    foreach (XmlAttribute position in myTransform.Attributes)
                            //    {
                            //        switch (position.Name)
                            //        {
                            //            case "x":
                            //                position.InnerText = (-3.256914f).ToString();
                            //                break;
                            //            case "y":
                            //                break;
                            //            case "z":
                            //                //position.InnerText = "1.278822";
                            //                break;
                            //        }
                            //    }
                            //}
                            //if (myTransform.Name == "rotation")
                            //{
                            //    foreach (XmlAttribute rotation in myTransform.Attributes)
                            //    {
                            //        switch (rotation.Name)
                            //        {
                            //            case "x":
                            //                rotation.InnerText = "0";
                            //                break;
                            //            case "y":
                            //                rotation.InnerText = "-90";
                            //                break;
                            //            case "z":
                            //                rotation.InnerText = "0";
                            //                break;
                            //        }
                            //    }
                            //}
                            //if (myTransform.Name == "scale")
                            //{
                            //    foreach (XmlAttribute rotation in myTransform.Attributes)
                            //    {
                            //        switch (rotation.Name)
                            //        {
                            //            case "x":
                            //                rotation.InnerText = "1";
                            //                break;
                            //            case "y":
                            //                rotation.InnerText = "1";
                            //                break;
                            //            case "z":
                            //                rotation.InnerText = "1";
                            //                break;
                            //        }
                            //    }
                            //}
                            //if (myTransform.Name == "Tag")
                            //{
                            //    if (myTransform.Attributes[0].InnerText == "Left")
                            //    {
                            //        gameobjects.SelectSingleNode("gameObject").Attributes[1].InnerText = "obj_zhangaila_wujin";
                            //    }
                            //    else
                            //    {
                            //        gameobjects.SelectSingleNode("gameObject").Attributes[1].InnerText = "obj_zhangaira_wujin";
                            //    }
                            //}
                        }

                    }

                }
                
            }

            xmlDoc.Save(filepath);
            Debug.Log("End");
        }
    }
}
