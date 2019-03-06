using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class HumanColliderManager : MonoBehaviour
{
    HumanEvolution _human;
    ItemState _item;
    public static GameObject _item_Collider { get; set; }   //道具碰撞框
    GameResource.HumanNature _nature;
    GameObject _effect;//道具特效
    private GameObject _protect_Broke_Mobel;

    public static GameObject Item_Collider { get { return _item_Collider; } }
    public static Collider WideWallColliderBefore { get; set; }

    private static HumanColliderManager _instance;

    public static HumanColliderManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<HumanColliderManager>();
            }
            return _instance;
        }
    }

    private Vector3 _default_Position;
    private Vector3 _default_Rotation;

    [UsedImplicitly]
    void Start()
    {
        _default_Position = transform.localPosition;
        _default_Rotation = transform.localEulerAngles;
        Ice.Is_Jump_Before = false;
        Ice.IsOnIce = false;
        WideWallColliderBefore = null;
        _nature = HumanManager.Nature;
        _human = _nature.Human_Script;
        ItemColliderManager skill_Manager = _human.transform.GetComponentInChildren<ItemColliderManager>();

        StaticParameter.JudgeNull("_human", _human);
        StaticParameter.JudgeNull("skill_Manager", skill_Manager);

        _item_Collider = skill_Manager.gameObject;
        _item_Collider.SetActive(false);

        _effect = (GameObject)StaticParameter.LoadObject("GameEffect", "effect");
        _protect_Broke_Mobel = (GameObject)StaticParameter.LoadObject("Other", "ProtectBroke");
    }

    [UsedImplicitly]
    void OnTriggerEnter(Collider other)
    {
        if (HumanManager.Nature.HumanManager_Script.CurrentState == HumanState.Dead)
            return;
        if (WideWallColliderBefore == other)
            return;

        ChangeItem(other, _nature.HumanManager_Script, _effect);

        //判断碰撞的物体
        switch (other.gameObject.layer)
        {
            case MyTags.Wide_Wall_Collider_Layer:
                WideWallColliderBefore = other;

                if (Judge(HumanManager.WallMark, other.transform))
                {
                    _nature.HumanManager_Script.Collde = other.transform;
                }
                break;
            case MyTags.Toturial_Layer:
            case MyTags.Chest_Layer:
            case MyTags.Fruit_Layer:
                _nature.HumanManager_Script.Collde = other.transform;
                break;
            case MyTags.Spring_Layer:
                if (Judge(HumanManager.WallMark, other.transform))
                {
                    _nature.HumanManager_Script.Collde = other.transform;
                }
                break;
            case MyTags.Obstacle_Layer:
            case MyTags.Missile_Layer:
            case MyTags.Sword_Layer:
                if (other.gameObject.layer == MyTags.Missile_Layer)
                {
                    Destroy(other.gameObject);
                }

                StartCoroutine(CloseAndResetCollider(other));
                
                if (ItemColliction.SuperMan.IsRun() || ItemColliction.Dash.IsRun() || ItemColliction.StartDash.IsRun())
                {
                    //无敌碰撞的声音
                    if (ItemColliction.SuperMan.IsRun() && other.gameObject.layer == MyTags.Obstacle_Layer)
                    {
                        MyAudio.PlayAudio(StaticParameter.s_Obstacle_Break, false, StaticParameter.s_Obstacle_Break_Volume);
                    }
                    //摄像机震动
                    CameraShake.Is_Shake_Camera = true;
                    if (other.gameObject.layer == MyTags.Missile_Layer)
                    {
                        StaticParameter.DestroyOrDespawn(other.transform);
                    }
                    else
                    {
                        //Destroy(other.gameObject);
                        StaticParameter.DestroyOrDespawn(other.transform);
                        GameObject _obstacle = (GameObject)StaticParameter.LoadObject("Other", "BrokenObstacle");
                        GameObject copy = Instantiate(_obstacle, other.transform.position, Quaternion.identity)as GameObject;
                        Destroy(copy,2);

                    }
                }
                else if (ItemColliction.Protect.IsRun())
                {
                    //播放音效
                    MyAudio.PlayAudio(StaticParameter.s_Sheld_Break, false, StaticParameter.s_Sheld_Break_Volume);

                    ItemColliction.Protect.Exit();
                    //摄像机震动
                    CameraShake.Is_Shake_Camera = true;
                    //护盾破碎
                    ProtectBroke();
                }
                else
                {
                    _nature.HumanManager_Script.Collde = other.transform;
                }
                break;
        }

    }

    IEnumerator CloseAndResetCollider(Collider other)
    {
        //碰撞到机关的时候，关闭碰撞框
        Collider collider = other.transform.GetComponent<Collider>();
        collider.enabled = false;

        yield return new WaitForSeconds(1);

        if (collider)
        {
            collider.enabled = true;
        }
    }
    //护盾破碎特效
    void ProtectBroke()
    {
        GameObject copy = Instantiate(_protect_Broke_Mobel);
        copy.transform.position = _nature.Human_Mesh.bounds.center;
    }
    bool Judge(WallMark mark, Transform wall)
    {
        switch (mark)
        {
            case WallMark.Left:
                if (wall.tag.Contains(MyTags.Left_Tag))
                {
                    return true;
                }
                else {
                    return false;
                }
            case WallMark.Right:
                if (wall.tag.Contains(MyTags.Right_Tag))
                {
                    return true;
                }
                else {
                    return false;
                }
            default:
                Debug.Log("mark值有问题");
                return false;
        }
    }
    [UsedImplicitly]
    void OnTriggerExit(Collider other)
    {
        if (HumanManager.Nature.HumanManager_Script.CurrentState == HumanState.Dead)
            return;
        
        if (other.gameObject.layer.Equals(MyTags.Wide_Wall_Collider_Layer)
            &&(_nature.Human.position.y > other.transform.position.y)
            && (_nature.HumanManager_Script.CurrentState == HumanState.Revise 
                || _nature.HumanManager_Script.CurrentState == HumanState.Run 
                || _nature.HumanManager_Script.CurrentState == HumanState.Slowdown))
        {
            switch (HumanManager.WallMark)
            {
                case WallMark.Left:
                    if (other.tag.Equals(MyTags.Left_Tag) && !_nature.HumanManager_Script.CurrentState.Equals(HumanState.Jump))
                    {
                        HumanPosition(_human.transform, _nature.Left_Wall_Inside);
                    }
                    break;
                case WallMark.Right:

                    if (other.tag.Equals(MyTags.Right_Tag) && !_nature.HumanManager_Script.CurrentState.Equals(HumanState.Jump))
                    {
                        HumanPosition(_human.transform, _nature.Right_Wall_Inside + 0.4f);
                    }
                    break;
            }
        }
    }

    void HumanPosition(Transform human, float x_Wall_Inside)
    {
        var target_Position = new Vector3(x_Wall_Inside, human.position.y, human.position.z);
        human.position = target_Position;
    }

    void ChangeItem(Collider other, HumanManager human, GameObject effect)
    {
        ItemState temp = ItemState.Null;
        switch (other.tag)
        {
            case "Dash":
                if (!ItemColliction.DeadDash.IsRun())
                {
                    temp = ItemState.Dash;
                }
                else
                {
                    temp = ItemState.DeadDash;
                }
                break;
            case "Double":
                temp = ItemState.Double;
                break;
            case "Protect":
                temp = ItemState.Protect;
                break;
            case "SuperMan":
                temp = ItemState.SuperMan;
                break;
            case "Magnet":
                temp = ItemState.Magnet;
                break;
        }
        if (temp == ItemState.Null)
        {
            return;
        }
        else
        {
            if (temp != ItemState.DeadDash)
            {
                human.ItemState = temp;
            }
            Effect(other, effect);
            StaticParameter.DestroyOrDespawn(other.transform);
        }

    }


    void Effect(Collider other, GameObject effect)
    {
        GameObject effect_Copy = Instantiate(effect, other.transform.position, Quaternion.identity) as GameObject;
        Destroy(effect_Copy, 0.2f);
    }

    public void ChangeColliderRotation()
    {
        transform.Rotate(90, 0, 0);
        transform.Translate(0.3f, 0, -1.6f, Space.Self);
    }

    public void ChangeRotationToDefault()
    {
        transform.localPosition = _default_Position;
        transform.localEulerAngles = _default_Rotation;
    }
}
