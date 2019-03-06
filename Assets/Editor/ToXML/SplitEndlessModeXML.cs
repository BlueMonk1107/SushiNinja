using System;
using UnityEngine;
using UnityEditor;
using System.Xml;
using System.IO;
using UnityEditor.SceneManagement;

public class SplitEndlessModeXML : Editor
{
    private static float StartPoint = -31.33f;
    private static int _mark;

    [MenuItem("Custom/Split EndlessMode XML")]
    static void ExportXML()
    {
        string filepath = Application.dataPath + "/StreamingAssets" + "/EndlessMode.xml";

        if (File.Exists(filepath))
        {
            XmlDocument baseDocument = new XmlDocument();
            baseDocument.Load(filepath);
            for (int i = 0; i < 285; i++)
            {
                string path = Application.dataPath + "/StreamingAssets" + "/"+i+".xml";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                XmlDocument childDocument = new XmlDocument();
                XmlDeclaration xmlDeclaration = childDocument.CreateXmlDeclaration("1.0", "us-ascii", null);
                childDocument.AppendChild(xmlDeclaration);
                XmlElement rootXmlElement = childDocument.CreateElement("root");

                XmlNodeList nodeList =
                    baseDocument.SelectSingleNode("root").SelectSingleNode("scene").SelectNodes("fragment"+i);
                for (int j = 0; j < nodeList.Count; j++)
                {
                    if (nodeList[j].SelectSingleNode("gameObject") == null)
                        continue;
                    rootXmlElement.AppendChild(childDocument.ImportNode(nodeList[j].SelectSingleNode("gameObject"), true));
                }

                childDocument.AppendChild(rootXmlElement);
                childDocument.Save(path);
            }
           
        }
    }
}