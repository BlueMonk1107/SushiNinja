using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HumanManager : MonoBehaviour
{
    private static GameResource.HumanNature _nature;
    private BehaviourData _state_Data;
    private ItemColliction _item_Data;
    #region 对外开放的属性

    /// <summary>
    /// 墙体标记
    /// </summary>
    public static WallMark WallMark { get; set; }
    /// <summary>
    /// 跳跃类型标记
    /// </summary>
    public static JumpMark JumpMark { get; set; }
    /// <summary>
    /// 给碰撞物体赋值，调用碰撞管理函数，创建该碰撞物体类的实例，并调用方法	
    /// </summary>
    public Transform Collde
    {
        set
        {
            _collide_Transform = value;
            ColliderManager(Nature, value);
        }
    }

    /// <summary>
    /// 当前状态(用于状态的改变)
    /// </summary>
    public HumanState CurrentState
    {
        get { return _state_Data.CurrentState; }
        set { _state_Data.CurrentState = value; }
    }

    /// <summary>
    /// 上一个状态
    /// </summary>
    public HumanState BeforeTheState
    {
        get { return _state_Data.BeforeTheState; }
    }

    private Transform _collide_Transform;
    /// <summary>
    /// 当前状态(用于状态的改变)
    /// </summary>
    public Transform CurrentCollide
    {
        get
        {
            return _collide_Transform;
        }
    }

    /// <summary>
    /// 人物属性
    /// </summary>
    public static GameResource.HumanNature Nature { get { return _nature; } }

    /// <summary>
    /// 改变道具状态
    /// </summary>
    public ItemState ItemState
    {
        set
        {
            ChangeItemState(value);
        }
    }

    #endregion
    void Awake()
    {
        GameObject hero = null;
        switch (MyKeys.CurrentSelectedHero)
        {
            case MyKeys.CurrentHero.GuiYuZi:
                hero = StaticParameter.LoadObject("Ninja", "GuiYuZi") as GameObject;
                break;
            case MyKeys.CurrentHero.CiShen:
                hero = StaticParameter.LoadObject("Ninja", "CiShen") as GameObject;
                break;
            case MyKeys.CurrentHero.YuZi:
                hero = StaticParameter.LoadObject("Ninja", "Yuzi") as GameObject;
                break;
            case MyKeys.CurrentHero.ShouSi:
                hero = StaticParameter.LoadObject("Ninja", "Shousi") as GameObject;
                break;
        }

        Instantiate(hero);
        //if (SceneManager.GetActiveScene().name.Contains("Endless"))
        //{
        //    copy.transform.position = new Vector3(-4.2f, -25.3f, 1.06f);
        //}

        //初始化人物属性对象
        _nature = new GameResource.HumanNature(1);

        //初始化行为数据
        _state_Data = new BehaviourData();
        _item_Data = new ItemColliction();
    }

    // Use this for initialization
    void Start()
    {
        CurrentState = HumanState.Run;
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);
        //开始前购买道具的实现
        _item_Data.TheBuyItems();
    }

    public void Update()
    {
        if (MyKeys.Pause_Game)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (JumpCondition())
            {
                //修改墙体标记
                switch (WallMark)
                {
                    case WallMark.Left:
                        WallMark = WallMark.Right;
                        break;
                    case WallMark.Right:
                        WallMark = WallMark.Left;
                        break;
                }

                CurrentState = HumanState.Jump;
            }
        }

        _state_Data.Update();

        _item_Data.Update();
    }

    //点击事件的判定条件
    bool JumpCondition()
    {
        if (EventSystem.current.currentSelectedGameObject)
        {
            return false;
        }

        ////如果是教学状态，就不判定此点击事件
        //if (GuidanceBase.GuidanceMark < 3)
        //    return false;
        return (CurrentState != HumanState.Jump/*人物状态不是跳*/
            && ItemColliction.Dash.IsRun() == false/*当前道具状态不是冲刺*/&& CurrentState != HumanState.Dead/*人物没有死亡*/
            && ItemColliction.StartDash.IsRun() == false && ItemColliction.DeadDash.IsRun() == false);
    }

    /// <summary>
    /// 改变当前道具状态,此方法只管理道具状态的改变，并不涉及道具效果的实现
    /// </summary>
    /// <param name="item"></param>
    void ChangeItemState(ItemState item)
    {
        switch (item)
        {
            case ItemState.Dash:
                CurrentState = HumanState.Stop;
                ItemColliction.Dash.EnterState();
                break;
            case ItemState.Double:
                ItemColliction.Double.EnterState();
                break;
            case ItemState.Protect:
                ItemColliction.Protect.EnterState();
                break;
            case ItemState.SuperMan:
                ItemColliction.SuperMan.EnterState();
                break;
            case ItemState.Magnet:
                ItemColliction.Magnet.EnterState();
                //nature.Human.GetComponentInChildren<FruitCollider> ().gameObject.SetActive (false);
                break;
            case ItemState.StartDash:
                CurrentState = HumanState.Stop;
                ItemColliction.StartDash.EnterState();
                break;
            case ItemState.DeadDash:
                CurrentState = HumanState.Stop;
                ItemColliction.DeadDash.EnterState();
                break;
        }

    }

    /// <summary>
    /// 人物碰撞管理器(因为碰撞物体不同，所以每碰撞一次，创建一次方法类对象)
    /// </summary>
    /// <param name="register_State"></param>
    /// <param name="state_Machines"></param>
    /// <param name="nature"></param>
    /// <param name="sth"></param>
    /// <param name="item"></param>
    void ColliderManager(GameResource.HumanNature nature, Transform sth)
    {
        AColliderMethod method = null;

        switch (sth.gameObject.layer)
        {
            case MyTags.Wide_Wall_Collider_Layer:

                //墙体碰撞方法实例
                method = new Wall(nature, sth);

                break;
            case MyTags.Ice_Layer:
                method = new Ice(nature, sth);
                break;
            case MyTags.Spring_Layer:
                //弹簧碰撞方法实例
                method = new Spring(nature, sth);
                break;
            case MyTags.Obstacle_Layer:
            case MyTags.Missile_Layer:
            case MyTags.Sword_Layer:
                //致死机关碰撞方法实例
                method = new DeadFunction(nature, sth);
                break;
            case MyTags.Fruit_Layer:
                method = new Fruit(sth);
                break;
            case MyTags.Chest_Layer:
                method = new Chest(sth);
                break;
            case MyTags.Toturial_Layer:
                method = new Toturial(sth);
                break;
        }
        if (method != null)
        {
            method.OnExecute();
        }
    }

    /// <summary>
    /// 判断人物是否在屏幕内
    /// </summary>
    public void IsInScreen()
    {
        if (ItemColliction.Dash.IsRun() || Time.timeSinceLevelLoad == 0)
        {
            return;
        }

        if (!Nature.Human_Mesh.isVisible)
        {
            Nature.HumanManager_Script.CurrentState = HumanState.Dead;
        }
    }

    void OnDestroy()
    {
        //清空购买道具的数组
        BuyTheItems.Buy_Items_List.Clear();
        BuyTheItems.dead_Dash_Bool = false;
        BuyTheItems.start_Dash_Bool = false;
        //释放对象，清理内存
        _state_Data.Clear();
        _item_Data.Clear();
        _nature = default(GameResource.HumanNature);
    }
}
