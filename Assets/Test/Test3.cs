using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

public class Test3 : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    Load();
	}

    void Load()
    {
        string filepath = Application.dataPath + "/StreamingAssets" + "/MyXML.xml";

        if (File.Exists(filepath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filepath);
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("root").SelectSingleNode("scene").SelectNodes("fragment");

            foreach (XmlElement gameobjects in nodeList)
            {
                string asset = "Prefab/" + gameobjects.SelectSingleNode("gameObject").Attributes[1].InnerText;
                Debug.Log(asset);

                Vector3 pos = Vector3.zero;
                Vector3 rot = Vector3.zero;
                Vector3 sca = Vector3.zero;
                string Tag = "";
                int Layer = 0;
                foreach (XmlElement myTransform in gameobjects.SelectSingleNode("gameObject").SelectSingleNode("transform").ChildNodes)
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
                    #endregion
                }
                Debug.Log(pos+" "+rot+" "+sca+" "+Tag+" "+Layer);
                //拿到 旋转 缩放 平移 以后克隆新游戏对象
                //GameObject ob = (GameObject)Instantiate(Resources.Load(""), pos, Quaternion.Euler(rot));
                //ob.transform.localScale = sca;
                //ob.tag = Tag;
                //ob.layer = Layer;
            }
        }
    }
}
