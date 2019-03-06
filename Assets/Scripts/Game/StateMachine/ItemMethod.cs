using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//冲锋
public class Dash : IStateMethod
{
    readonly Transform _human;
    readonly float _up_Speed;
    readonly Animator _Ani;
    private GameResource.HumanNature _nature;
    private Transform _dash_Mobel; //冲刺模型
    private Transform _dash_BG;    //冲刺背景
    private Transform _item_Colloder_Transform;//技能碰撞框所在物体
    private SphereCollider _item_Colloder;//技能碰撞框
    public Dash(GameResource.HumanNature nature)
    {
        _human = nature.Human;
        _up_Speed = nature.Run_Speed*1.5f;
        _Ani = nature.Human_Ani;
        _nature = HumanManager.Nature;
    }
    public void OnEnter()
    {
        _human.eulerAngles = Vector3.up * 180;

        //播放音效
        switch (MyKeys.CurrentSelectedHero)
        {
            case MyKeys.CurrentHero.GuiYuZi:
                MyAudio.PlayAudio(StaticParameter.s_GuiYuZi_Skill, false, StaticParameter.s_GuiYuZi_Skill_Volume);
                break;
            case MyKeys.CurrentHero.CiShen:
                MyAudio.PlayAudio(StaticParameter.s_CiShen_Skill, false, StaticParameter.s_CiShen_Skill_Volume);
                break;
            case MyKeys.CurrentHero.YuZi:
                MyAudio.PlayAudio(StaticParameter.s_YuZi_Skill, false, StaticParameter.s_YuZi_Skill_Volume);
                break;
        }
        MyAudio.PlayAudio(StaticParameter.s_Dash, false, StaticParameter.s_Dash_Volume);

        MoveToDashPosition(_nature);

        _Ani.SetInteger(StaticParameter.Ani_Key_HumanState, 6);
        
        //加载并实例化冲刺模型
        GameObject dash_Mobel = (GameObject)StaticParameter.LoadObject("Other", "Dash");
        GameObject temp = Object.Instantiate(dash_Mobel);
        _dash_Mobel = temp.transform.GetChild(0);
        _dash_BG = temp.transform.GetChild(1);
        //调整碰撞框大小及位置
        _item_Colloder_Transform = HumanColliderManager.Item_Collider.transform;
        _item_Colloder = _item_Colloder_Transform.GetComponent<SphereCollider>();
        //_item_Colloder_Transform.localPosition = Vector3.up * (-1.42f);
        _item_Colloder.radius = 3.38f;
        //特效位置位置初始化
        _dash_Mobel.position = _human.position - Vector3.up * 2;
        _dash_BG.position = Vector3.right * _dash_BG.position.x + Vector3.up * _human.position.y +
                            Vector3.forward * _human.position.z;
    }
    public void OnExecute()
    {
        //冲刺模型控制
        _dash_Mobel.position = _human.position - Vector3.up * 2;
        _dash_Mobel.transform.Rotate(0, 10, 0, Space.World);
        _dash_Mobel.GetChild(0).GetComponent<Renderer>().material.mainTextureOffset -= Vector2.right * 0.3f;
        //模型背景控制
        _dash_BG.position = Vector3.right * _dash_BG.position.x + Vector3.up * _human.position.y +
                            Vector3.forward * _human.position.z;
        _dash_BG.GetComponent<Renderer>().material.mainTextureScale += Vector2.right * 0.3f;
        //人物移动控制
        _human.Translate(0, _up_Speed, 0, Space.World);

    }
    public void OnExit()
    {
        //调整人物角度
        switch (HumanManager.WallMark)
        {
            case WallMark.Left:
                _human.eulerAngles = Vector3.zero;
                break;
            case WallMark.Right:
                _human.eulerAngles = Vector3.up * 180;
                break;
        }

        //销毁实例化的模型
        Object.Destroy(_dash_Mobel.parent.gameObject);
        _dash_Mobel = null;
        _dash_BG = null;
        Resources.UnloadUnusedAssets();

        //_item_Colloder_Transform.localPosition = Vector3.up * 0.32f;
        _item_Colloder.radius = 5f;

    }


    void MoveToDashPosition(GameResource.HumanNature nature)
    {
        Vector3 center = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth * 0.5f, Camera.main.pixelHeight * 0.25f,
                   nature.Human.position.z - Camera.main.transform.position.z));
        if (MyCamera.GameEnd)
        {
            nature.Human.position = new Vector3(center.x, nature.Human.position.y, nature.Human.position.z);
            _human.DOMoveY(_human.position.y + 12, 0.5f);
            _human.DOPlay();
        }
        else
        {
            float duration = 0.5f;
            int frams = (int)(60 * duration);
            
            Vector3 temp;
            if (BuyTheItems.start_Dash_Bool)
            {
                temp = Vector3.right * center.x + Vector3.up * (center.y + (frams - 6) * _up_Speed) +
                                         Vector3.forward * nature.Human.position.z;
            }
            else
            {
                if (MyKeys.Pause_Game && !GameUIManager.Is_Resurgence)
                {
                    temp = Vector3.right * center.x + Vector3.up * center.y +
                                        Vector3.forward * nature.Human.position.z;
                }
                else
                {
                    temp = Vector3.right * center.x + Vector3.up * (center.y + frams * _up_Speed) +
                                            Vector3.forward * nature.Human.position.z;
                }
            }


            nature.Human.DOMove(temp, duration).SetEase(Ease.Linear).SetId("Dash").OnComplete(()=> { DOTween.Kill("Dash"); });
            DOTween.Play("Dash");
        }
        
    }
}

//双倍
public class Double : IStateMethod
{
    Transform _human;
    private GameResource.HumanNature _nature;

    public Double(GameResource.HumanNature nature)
    {
        _nature = nature;
        _human = nature.Human;

    }
    public void OnEnter()
    {
        //播放音效
        MyAudio.PlayAudio( StaticParameter.s_Double, false, StaticParameter.s_Double_Volume);
    }
    public void OnExecute()
    {

    }
    public void OnExit()
    {
    }

}

//护盾
public class Protect : IStateMethod
{
    private GameResource.HumanNature _nature;
    private GameObject _copy;
    private Renderer _copy_Renderer;
    private Color _color;
    private float[] _time;//每一块起始颜色的数值
    public Protect(GameResource.HumanNature nature)
    {
        _nature = nature;
    }
    public void OnEnter()
    {
        //播放音效
        MyAudio.PlayAudio( StaticParameter.s_Sheld, false, StaticParameter.s_Sheld_Volume);
        //加载并实例化护盾模型
        GameObject protect_Mobel = (GameObject)StaticParameter.LoadObject("Other", "Protect");
        _copy = Object.Instantiate(protect_Mobel);
        _copy_Renderer = _copy.transform.GetChild(0).GetComponent<Renderer>();
        _color = Color.white;
        _time = new[]{1.25f,0.75f,0.25f,0.0f};

        GameObject collider = GameObject.FindGameObjectWithTag("ColliderManager");

        if (_nature.HumanManager_Script.CurrentState == HumanState.Run|| _nature.HumanManager_Script.CurrentState == HumanState.Revise)
        {
            if (HumanManager.WallMark == WallMark.Left)
            {
                _copy.transform.position = _nature.Human.position + Vector3.right * 0.86f;
            }
            else
            {
                _copy.transform.position = _nature.Human.position + Vector3.left * 0.86f;
            }
        }
        else
        {
            _copy.transform.position = _nature.Human.position + Vector3.up*0.72f;
        }
        
        _copy.transform.SetParent(_nature.Human);
    }
    public void OnExecute()
    {
        for (int i = 0;i<_time.Length;i++)
        {
            _time[i] += Time.fixedDeltaTime;
        }

        for (int i = 0;i<_copy.transform.childCount;i++)
        {
            _copy_Renderer = _copy.transform.GetChild(i).GetComponent<Renderer>();
            _color = Color.white * (Mathf.PingPong(_time[i], 1)-0.25f*i);
            _copy_Renderer.materials[0].SetColor("_TintColor", _color);
            _copy_Renderer.materials[1].SetColor("_TintColor", _color);
        }
       
    }
    public void OnExit()
    {
        Object.Destroy(_copy);
        StaticParameter.ClearReference(ref _copy);
    }

}

//无敌
public class SuperMan : IStateMethod
{
    Transform _human;
    private int _frames;//无敌总帧数
    private int _times;//执行帧数
    private GameResource.HumanNature _nature;
    private List<Renderer> temp;
    private GameObject _item_Time;

    private string normal_Shader_Path;
    private string flicker_Shader_Path;
    public SuperMan(GameResource.HumanNature nature)
    {
        _human = nature.Human;
        _nature = nature;
    }
    public void OnEnter()
    {
        if (ItemColliction.SuperMan.Time > 1)
        {
            //播放音效
            switch (MyKeys.CurrentSelectedHero)
            {
                case MyKeys.CurrentHero.GuiYuZi:
                    MyAudio.PlayAudio(StaticParameter.s_GuiYuZi_Skill, false, StaticParameter.s_GuiYuZi_Skill_Volume);
                    break;
                case MyKeys.CurrentHero.CiShen:
                    MyAudio.PlayAudio(StaticParameter.s_CiShen_Skill, false, StaticParameter.s_CiShen_Skill_Volume);
                    break;
                case MyKeys.CurrentHero.YuZi:
                    MyAudio.PlayAudio(StaticParameter.s_YuZi_Skill, false, StaticParameter.s_YuZi_Skill_Volume);
                    break;
            }
            //播放音效
            MyAudio.PlayAudio(StaticParameter.s_Atkup, false, StaticParameter.s_Atkup_Volume);
        }
        
        _human.localScale = Vector3.one * 1.2f;
        _frames = (int)_nature.Super_Man_Time * 60;
        temp = new List<Renderer>();
        foreach (Transform transform in _human)
        {
            Renderer renderer = _human.GetComponentInChildren<Renderer>();
            if (renderer)
            {
                temp.Add(renderer);
            }
        }

        foreach (Renderer renderer in temp)
        {
            renderer.material.color = Color.red;
        }
        //道具时间显示组件
        if (ItemColliction.SuperMan.Time > 1)
        {
            GameObject time_Temp = Resources.Load("GameEffect/ItemTime") as GameObject;
            _item_Time = Object.Instantiate(time_Temp);
            _item_Time.GetComponentInChildren<ItemTimeEffect>().Time = ItemColliction.SuperMan.Time;
        }
        normal_Shader_Path = "Legacy Shaders/Transparent/Diffuse";
        flicker_Shader_Path = "Legacy Shaders/Self-Illumin/Diffuse";
    }
    public void OnExecute()
    {
        _times++;
        if (_times < (_frames - 60))
        {
            if (_times % 30 == 27)
            {
                foreach (Renderer renderer in temp)
                {
                    renderer.material.shader = Shader.Find(normal_Shader_Path);
                }
            }
            if (_times%30 == 0)
            {
                foreach (Renderer renderer in temp)
                {
                    renderer.material.shader = Shader.Find(flicker_Shader_Path);
                    renderer.material.color = new Color32(255, 104, 88, 0);
                }
            }
        }
        else
        {
            if (_times % 12 == 9)
            {
                foreach (Renderer renderer in temp)
                {
                    renderer.material.shader = Shader.Find(normal_Shader_Path);
                }
            }
            if (_times % 12 == 0)
            {
                foreach (Renderer renderer in temp)
                {
                    renderer.material.shader = Shader.Find(flicker_Shader_Path);
                    renderer.material.color = new Color32(255, 104, 88, 0);
                }
            }
        }

    }
    public void OnExit()
    {
        foreach (Renderer renderer in temp)
        {
            renderer.material.shader = _nature.Human_Shader;
            renderer.material.color = new Color(1, 1, 1, 0);
        }
        _human.localScale = Vector3.one;

        _item_Time = null;
    }

}

//磁铁
public class Magnet : IStateMethod
{
    readonly Transform _human;
    private SphereCollider _item_Colloder;
    private Transform _item_Colloder_Transform;
    private GameResource.HumanNature _nature;
    public Magnet(GameResource.HumanNature nature)
    {
        _human = nature.Human;
        _nature = nature;
    }
    public void OnEnter()
    {
        //调整碰撞框大小及位置
        _item_Colloder_Transform = HumanColliderManager.Item_Collider.transform;
        _item_Colloder = _item_Colloder_Transform.GetComponent<SphereCollider>();
        _item_Colloder.radius = 6f;
        //播放音效
        MyAudio.PlayAudio( StaticParameter.s_Magent, false, StaticParameter.s_Magent_Volume);
    }
    public void OnExecute()
    {

    }
    public void OnExit()
    {
        _item_Colloder.radius = 5f;
    }
}
//空状态
public class Null : IStateMethod
{
    public Null(GameResource.HumanNature nature)
    {

    }
    public void OnEnter()
    {

    }
    public void OnExecute()
    {
    }
    public void OnExit()
    {

    }
}
