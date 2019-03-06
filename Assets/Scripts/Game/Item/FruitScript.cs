using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class FruitScript : MonoBehaviour
{
    private Material _double_Material; //双倍分数的材质
    private GameObject _canvas; //游戏界面的画布
    private Material _default_Material;//默认材质

    private SkinnedMeshRenderer _target; //磁铁状态下的目标
    private float _fly_Speed;//磁铁状态下的飞行速度
    private bool _rotate_Bool;//水果自旋转的开关

    //此水果是否双倍分数的标记
    public bool Is_Double_Bool { get; set; }
    //双倍材质
    public Material DoubleMaterial
    {
        get
        {
            return _double_Material;
        }
    }

    public SkinnedMeshRenderer Target//磁铁状态下的目标
    {
        set
        {
            _target = value;
            StartCoroutine(Move());
        }
    }

    public void OnEnable()
    {
        if (_default_Material != null)
        {
            transform.GetComponent<MeshRenderer>().material = _default_Material;
        }
    }

    // Use this for initialization
    void Start()
    {
        _default_Material = transform.GetComponent<MeshRenderer>().material;

        //for temp(O' 170224)
        if (transform.parent.name.IndexOf("shoulijian") != -1)
        {
            transform.parent.Rotate(180, 0, 0, Space.World);
            transform.parent.localScale = Vector3.one;
        }
        //从resources文件夹读取资源
        _double_Material = (Material)StaticParameter.LoadObject("Materials", "Golden");

        //判断人物自身技能（这里只判断人物双倍分数技能）
        JudgeSkill(HumanManager.Nature, _double_Material);

        _fly_Speed = 15.0f;

        _rotate_Bool = true;

        

    }

    //判断人物自身技能（这里只判断人物双倍分数技能）
    void JudgeSkill(GameResource.HumanNature nature, Material double_Material)
    {

        foreach (HumanSkill i in nature.Skill_List)
        {
            if (i == HumanSkill.DoubleGold)
            {
                MeshRenderer render = transform.GetComponent<MeshRenderer>();
                if (double_Material)
                {
                    render.material = _double_Material;
                }
                else {
                    Debug.Log(name + "少个材质");
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (StaticParameter.ComparePosition(transform, HumanManager.Nature.Human) > 80.0f)
            return;
        if (_rotate_Bool)
        {
            transform.Rotate(0, -2, 0, Space.World);
        }
    }

    public void CutFruit()
    {
        if (ItemColliction.Dash.IsRun())
        {
            //播放音效
            MyAudio.PlayAudio(StaticParameter.s_Score, false, StaticParameter.s_Score_Volume * 0.5f);
        }
        else {
            //播放音效
            MyAudio.PlayAudio(StaticParameter.s_Score, false, StaticParameter.s_Score_Volume);
            //刀光
            //SwordLight(this.transform);
        }
        //计算并显示分数
        Score();

        //喷溅果汁
        Juice(this.transform);

        transform.GetComponent<SphereCollider>().enabled = false;

        //RandonVelocity();//被切后的速度生成函数

        _target = null; //水果被切后磁铁目标修改为空

        ClearData();

        GoldAnimation();
    }
    //清空数据
    void ClearData()
    {
        _rotate_Bool = false;
        Destroy(this.gameObject, 2.0f);
        _canvas = null;
    }
    //吃了磁铁后，水果的移动
    IEnumerator Move()
    {
        yield return new WaitUntil(() =>
        {
            if (_target)
            {
                Vector3 temp = _target.bounds.center;
                Vector3 direction = temp - this.transform.position;
                this.transform.Translate(direction * Time.deltaTime * _fly_Speed, Space.World);
            }
            else
            {
                GoldAnimation();
            }
            return _target == null;
        });
    }

    //吃金币时金币动画
    void GoldAnimation()
    {
        Vector3 scale = transform.localScale * 1.6f;
        transform.DOScale(scale, 0.1f).OnComplete(() =>
        {
            Destroy(transform.parent.gameObject);
        });
        transform.DOPlay();
    }
    //显示分数
    void Score()
    {
        if (Is_Double_Bool)
        {
            MyKeys.Game_Score += 2;//加分静态变量
        }
        else
        {
            MyKeys.Game_Score++;
        }

        Window_Succeed.Cut_Number++;//切到水果的数量

        try
        {
            Window_Ingame.Instance.ShowGold(MyKeys.Game_Score.ToString());
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刀光
    void SwordLight(Transform fruit)
    {
        MyPool.Instance.Spawn(StaticParameter.s_Prefab_SwordLight, fruit.position + Vector3.up * fruit.localScale.y / 2, Quaternion.identity);
    }

    //喷溅果汁
    void Juice(Transform fruit)
    {
        MyPool.Instance.Spawn(StaticParameter.s_Prefab_Juice, fruit.position, Quaternion.identity);
    }

    //水果解体后的随机速度生成函数
    void RandonVelocity()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Contains("A"))
            {
                float x_A = Random.Range(1, 3);
                float y_A = Random.Range(2, 5);

                Vector3 target_A = Vector3.right * (-x_A) + Vector3.up * (y_A);

                child.DOBlendableMoveBy(target_A, 1f).SetEase(Ease.OutExpo);

                child.DOBlendableMoveBy(Vector3.up * (-30f), 1f).SetEase(Ease.InExpo);

                child.DOLocalRotate(new Vector3(5, 4, 3), 0.4f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
                child.DOPlay();
            }
            else
            {
                float x_B = Random.Range(1, 3);
                float y_B = Random.Range(2, 5);

                Vector3 target = Vector3.right * (x_B) + Vector3.up * (y_B);

                child.DOBlendableMoveBy(target, 1f).SetEase(Ease.OutExpo);

                child.DOBlendableMoveBy(Vector3.up * (-30f), 1f).SetEase(Ease.InExpo);

                child.DOLocalRotate(new Vector3(-5, -4, -3), 0.4f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
                child.DOPlay();
            }
        }
    }

}

