using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Missile : MonoBehaviour
{
    private Quaternion _line_Rotation;
    private float _distance;//人物与导弹的相对距离，到达此距离开始移动
    private GameObject _bomb_Effect;//爆炸特效
    private Transform _fire_Effect;//火焰特效
    private int times = 0;

    public void Start()
    {
        Initialization();

        //导弹的判定和移动
        StartCoroutine(StartToMove());
    }

    public void OnEnable()
    {
        if (times == 0)
        {
            times++;
            return;
        }
        Initialization();

        //导弹的判定和移动
        StartCoroutine(StartToMove());
    }

    void Initialization()
    {
        _line_Rotation = new Quaternion(0.7f, 0, 0, 0.7f);
        transform.eulerAngles = Vector3.left * 90;
        _distance = 5;
        _bomb_Effect = (GameObject)StaticParameter.LoadObject("GameEffect", "bomb");
    }

    IEnumerator StartToMove()
    {
        yield return new WaitUntil(() =>
        {
            if (MyKeys.Pause_Game)
                return false;
            if (IsHighSpeed())
            {
                return transform.position.y - (Camera.main.transform.position.y - HumanManager.Nature.Difference_Y) < 22.5;
            }
            else
            {
                return transform.position.y - (Camera.main.transform.position.y - HumanManager.Nature.Difference_Y) < 15;
            }

        });

        //显示导弹提示线
        MyPool.Instance.Spawn(StaticParameter.s_Prefab_Line, transform.position, _line_Rotation);

        yield return new WaitUntil(() =>
        {
            if (MyKeys.Pause_Game)
                return false;
            if (IsHighSpeed())
            {
                _distance -= HumanManager.Nature.Run_Speed * 1.5f;
            }
            else
            {
                _distance -= HumanManager.Nature.Run_Speed;
            }

            return _distance <= 0;
        });

        //开启火焰特效
        Transform fire = MyPool.Instance.Spawn(StaticParameter.s_Prefab_Fire);
        _fire_Effect = fire;
        fire.position = transform.position;
        fire.rotation = transform.rotation;

        yield return new WaitUntil(() =>
        {
            if (MyKeys.Pause_Game)
                return false;

            transform.Translate(0, -0.15f, 0, Space.World);
            fire.position = transform.position;
            return StaticParameter.ComparePosition(this.transform, HumanManager.Nature.Human) < -50;
        });

        MyPool.Instance.Despawn(fire);
        //到达距离，销毁导弹，在无尽模式下回收对象
        //Destroy(gameObject);
        StaticParameter.DestroyOrDespawn(transform);

    }

    bool IsHighSpeed()
    {
        return ItemColliction.StartDash.IsRun()
               || ItemColliction.SuperMan.IsRun()
               || ItemColliction.Dash.IsRun()
               || ItemColliction.DeadDash.IsRun();
    }

    void OnTriggerEnter(Collider other)
    {
        if ((ItemColliction.Dash.IsRun() || ItemColliction.StartDash.IsRun() || ItemColliction.DeadDash.IsRun()) && other.name == "SkillCollider")
        {
            Effect();
        }
        else if (other.name == "HumanCollider"|| other.name == "Human")
        {
            if (other.transform.position.y > transform.position.y)
                return;
            Effect();
            MissileCollider();
        }

    }

    void MissileCollider()
    {
        Destroy(gameObject);

        if (ItemColliction.SuperMan.IsRun() || ItemColliction.Dash.IsRun() || ItemColliction.StartDash.IsRun())
        {
            //无敌碰撞的声音
            if (ItemColliction.SuperMan.IsRun() && gameObject.layer == MyTags.Obstacle_Layer)
            {
                MyAudio.PlayAudio(StaticParameter.s_Obstacle_Break, false, StaticParameter.s_Obstacle_Break_Volume);
            }
            //摄像机震动
            CameraShake.Is_Shake_Camera = true;
                StaticParameter.DestroyOrDespawn(transform);
        }
        else if (ItemColliction.Protect.IsRun())
        {
            //播放音效
            MyAudio.PlayAudio(StaticParameter.s_Sheld_Break, false, StaticParameter.s_Sheld_Break_Volume);

            ItemColliction.Protect.Exit();
            //摄像机震动
            CameraShake.Is_Shake_Camera = true;
        }
        else
        {
            HumanManager.Nature.HumanManager_Script.Collde = transform;
        }
    }
    void Effect()
    {
        MyAudio.PlayAudio(StaticParameter.s_Bomb, false, StaticParameter.s_Bomb_Volume);
        if (_bomb_Effect != null)
        {
            GameObject copy = Instantiate(_bomb_Effect);
            copy.transform.position = transform.position;
        }

        //Destroy(gameObject);
        MyPool.Instance.Despawn(_fire_Effect);

        StaticParameter.DestroyOrDespawn(transform);
    }

    public void OnDisable()
    {
        if (_fire_Effect != null)
        {
            MyPool.Instance.Despawn(_fire_Effect);
            _fire_Effect = null;
        }
    }
}
