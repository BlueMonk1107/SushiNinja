using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;

public class SavePrefabName : Editor
{

    private static List<string> _name_List;

    [MenuItem("Tools/XML  Export Prefab Name To XML From XML")]
    static void ExportName()
    {

        _name_List = new List<string>();
        string filepath = Application.dataPath + "/StreamingAssets" + "/EndlessMode.xml";
        string temp = "";


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
                    temp = gameobjects.SelectSingleNode("gameObject").Attributes[1].InnerText;
                    if (!_name_List.Contains(temp))
                    {
                        _name_List.Add(temp);
                    }

                }
            }
        }

        string path = EditorUtility.SaveFilePanel("Save Resource", "Assets/StreamingAssets", "Name", "xml");

        if (path.Length != 0)
        {
            // 如果存在场景文件，删除
            if (File.Exists(path)) File.Delete(path);
            // 打开这个关卡
            XmlDocument xmlDocument = new XmlDocument();
            // 创建XML属性
            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDocument.AppendChild(xmlDeclaration);
            XmlElement rootXmlElement = xmlDocument.CreateElement("root");

            foreach (string name in _name_List)
            {
                // 创建XML根标志
                XmlElement XmlElement = xmlDocument.CreateElement(name);
                rootXmlElement.AppendChild(XmlElement);
            }
            xmlDocument.AppendChild(rootXmlElement);
            xmlDocument.Save(path);
        }
    }
}
