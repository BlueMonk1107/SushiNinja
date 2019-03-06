using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;

public class ChangeNoteName : Editor {

    private static string[] _name_List;

    [MenuItem("Tools/XML  ChangeNoteName")]
    static void Change()
    {
        //_name_List = new string[]{ "boluo","chengzi","huolongguo", "li", "mangguo", "mihoutao", "pingguo", "shuimitao", "xigua", "yezi" };
        //_name_List = new string[] { "item-shield1" };
        _name_List = new [] { "Obstacle_zhangai03" };

        #region 修改EndlessMode文件
        string filepath = Application.dataPath + "/StreamingAssets" + "/EndlessMode.xml";
        string temp = "";

        //如果文件存在话开始解析
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

                    temp = gameobjects.SelectSingleNode("gameObject").Attributes[1].InnerText;
                    foreach (string name in _name_List)
                    {
                        if (temp == name)
                        {
                            gameobjects.SelectSingleNode("gameObject").Attributes[1].InnerText = "Obstacle_zhangai01";
                        }
                    }
                }
            }

            xmlDoc.Save(filepath);
        }
        #endregion



        Debug.Log("End");
    }
}
