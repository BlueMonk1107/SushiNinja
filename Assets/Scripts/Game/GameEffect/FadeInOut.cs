using UnityEngine;
using System.Collections;
/// <summary>  
/// 淡入淡出脚本  
/// </summary>  
public class FadeInOut : MonoBehaviour
{
    public float lifeCycle;     //残影存在时间  


    float startTime;        //产生的时间  
    Material mat = null;    //材质球  
    Color32 _color;           //材质球颜色
    // Use this for initialization  
    void Start()
    {
        startTime = Time.time;


        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        //如果没有meshRenderer  或者  没有材质  禁用对象  
        if (!meshRenderer || !meshRenderer.material)
        {
            base.enabled = false;
        }
        else
        {
            mat = meshRenderer.material;
            //替换材质球  可不用  
            ReplaceSharder();
        }
        if (!ItemColliction.SuperMan.IsRun())
        {
            _color = mat.GetColor("_TintColor");
        }

        if (HumanManager.JumpMark == JumpMark.SpeedUp)
        {
            _color = new Color32(20, 249, 221, 255);
        }
        else
        {
            if (MyKeys.CurrentSelectedHero == MyKeys.CurrentHero.YuZi ||
                MyKeys.CurrentSelectedHero == MyKeys.CurrentHero.ShouSi)
            {
                _color = new Color32(50, 50, 50, 100);
            }
        }
    }

    // Update is called once per frame  
    void Update()
    {
        float time = Time.time - startTime;
        //存在时间到了就消灭掉  
        if (time > lifeCycle)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            //通过remainderTime来控制残影的透明度  
            float remainderTime = (lifeCycle - time)*255;
            //如果得到了材质球  
            if (mat)
            {
                //武器特效时使用  
                //if(mat.HasProperty("_Color"))  
                //控制sharder透明度  
                //Color col = mat.GetColor("_Color");
                //col.a = remainderTime;
                //mat.SetColor("_Color", col);

                if (!ItemColliction.SuperMan.IsRun())
                {
                    _color.a = (byte)remainderTime;
                    mat.SetColor("_TintColor", _color);

                    if (_color.a <= 0)
                    {
                        MyDestory();
                    }
                }
            }
        }
    }
    //替换材质球  
    private void ReplaceSharder()
    {
        if (mat.shader.name.Equals("Custom/NewSurfaceShader"))
        {
            mat.shader = Shader.Find("Particles/Additive");
        }
        // else if(mat.shader.name.Equals("Custom/Toon/Basic"))  
        // {  
        // mat.shader=Shader.Find("Custom/Toon/Basic Replace");  
        // }  
        // else  
        // {  
        // Debug.LogError("Can't find target sharder");  
        // }  
    }

    void MyDestory()
    {
        DestroyImmediate(gameObject,true);
    }
}