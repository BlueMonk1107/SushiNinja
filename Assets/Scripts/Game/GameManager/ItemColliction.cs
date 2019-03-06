using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ItemColliction
{
    private static DashData _dash;
    private static DoubleData _double;
    private static MagnetData _magnet;
    private static ProtectData _protect;
    private static SuperManData _superMan;
    private static StartDashData _startDash;
    private static DeadDashData _deadDash;

    private List<AItemData> _colliction;

    #region 数据对象
    public static DashData Dash
    {
        get
        {
            if (_dash == null)
            {
                _dash = new DashData();
            }
            return _dash;
        }
    }

    public static DoubleData Double
    {
        get
        {
            if (_double == null)
            {
                _double = new DoubleData();
            }
            return _double;
        }
    }

    public static MagnetData Magnet
    {
        get
        {
            if (_magnet == null)
            {
                _magnet = new MagnetData();
            }
            return _magnet;
        }
    }

    public static ProtectData Protect
    {
        get
        {
            if (_protect == null)
            {
                _protect = new ProtectData();
            }
            return _protect;
        }
    }

    public static SuperManData SuperMan
    {
        get
        {
            if (_superMan == null)
            {
                _superMan = new SuperManData();
            }
            return _superMan;
        }
    }
    public static StartDashData StartDash
    {
        get
        {
            if (_startDash == null)
            {
                _startDash = new StartDashData();
            }
            return _startDash;
        }
    }

    public static DeadDashData DeadDash
    {
        get
        {
            if (_deadDash == null)
            {
                _deadDash = new DeadDashData();
            }
            return _deadDash;
        }
    }

    public List<AItemData> Colliction
    {
        get
        {
            if (_colliction == null)
            {
                _colliction = new List<AItemData>();
            }
            return _colliction;
        }
    }
    #endregion

    public void Update()
    {
        for (int i = 0; i < Colliction.Count; i++)
        {
            AItemData temp = Colliction[i];
            if (temp.IsRun())
            {
                temp.Update();
            }
        }
    }

    public void TheBuyItems()
    {
        //购买道具的实现
        BuyTheItems.SetTheItems();
    }

    public ItemColliction()
    {
        Initialize(Dash);
        Initialize(Double);
        Initialize(Magnet);
        Initialize(Protect);
        Initialize(SuperMan);
        Initialize(StartDash);
        Initialize(DeadDash);
    }

    private void Initialize(AItemData item)
    {
        item.Initialize();
        Colliction.Add(item);
    }
    //释放对象，清理内存
    public void Clear()
    {
        _dash = null;
        _double = null;
        _magnet = null;
        _protect = null;
        _superMan = null;
        _startDash = null;
        _deadDash = null;
        _colliction = null;
    }

}
/// <summary>
/// 道具数据抽象类 
/// </summary>
public abstract class AItemData
{
    /// <summary>
    /// 此道具持续时间
    /// </summary>
    public float Time { set; get; }
    /// <summary>
    /// 正在执行的协程对象
    /// </summary>
    protected Coroutine PreviousCoroutine { set; get; }
    /// <summary>
    /// 实例化
    /// </summary>
    public abstract void Initialize();
    /// <summary>
    /// 此道具是否正在运行
    /// </summary>
    /// <returns></returns>
    public abstract bool IsRun();
    /// <summary>
    /// 进入道具状态
    /// </summary>
    public abstract void EnterState();
    /// <summary>
    /// 退出道具状态
    /// </summary>
    protected abstract void ExitState();
    /// <summary>
    /// 每帧执行函数
    /// </summary>
    public abstract void Update();

    public void OpenItemCollider()
    {
        if (ItemColliction.Magnet.IsRun() || ItemColliction.Double.IsRun() || ItemColliction.Dash.IsRun()||ItemColliction.StartDash.IsRun()||ItemColliction.DeadDash.IsRun())
        {
            HumanColliderManager.Item_Collider.SetActive(true);
        }
    }
}

/// <summary>
/// 冲刺数据类
/// </summary>
public class DashData : AItemData
{

    private StateMachine<ItemState> _dash_Machine;

    public StateMachine<ItemState> Dash_Machine
    {
        get
        {
            if (_dash_Machine == null)
            {
                 _dash_Machine = new StateMachine<ItemState>();
            }
            return _dash_Machine;
        }
    }

    public override bool IsRun()
    {
        if (Dash_Machine.CurrentState == ItemState.Dash)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void EnterState()
    {
        if (!GameUIManager.Is_Resurgence)
        {
            GameGuidance.Instance.GuidanceDash++;
        }
        
        if(BuyTheItems.start_Dash_Bool)
            return;
        Dash_Machine.CurrentState = ItemState.Dash;
        //关闭还未结束的此状态协程
        if (PreviousCoroutine != null)
        {
            HumanManager.Nature.Human_Script.StopCoroutine(PreviousCoroutine);
        }
        PreviousCoroutine = HumanManager.Nature.Human_Script.StartCoroutine(DashTime());
        OpenItemCollider();
    }
    IEnumerator DashTime()
    {
        float times = 0;
        yield return new WaitUntil(() =>
        {
            if (MyKeys.Pause_Game)
                return false;
            times++;
            return times >= ItemColliction.Dash.Time * 60;
        });
        ExitState();
    }

    protected override void ExitState()
    {
        Dash_Machine.CurrentState = ItemState.Null;
        HumanManager.Nature.HumanManager_Script.CurrentState = HumanState.Jump;
        ItemColliction.SuperMan.Time = 1.0f;
        HumanManager.Nature.HumanManager_Script.ItemState = ItemState.SuperMan;
        ItemCollider();
        PreviousCoroutine = null;
    }

    public override void Update()
    {
        Dash_Machine.Update();
    }

    public override void Initialize()
    {
        Dash_Machine.AddState(ItemState.Dash, new Dash(HumanManager.Nature));
        Dash_Machine.AddState(ItemState.Null, new Null(HumanManager.Nature));
        Dash_Machine.CurrentState = ItemState.Null;
        Time = HumanManager.Nature.Dash_Time;
    }
    //检测是否开启或关闭碰撞框
    public void ItemCollider()
    {
        if (ItemColliction.Magnet.IsRun() || ItemColliction.Double.IsRun())
        {
            HumanColliderManager.Item_Collider.SetActive(true);
        }
        else
        {
            HumanColliderManager.Item_Collider.SetActive(false);
        }
    }
}

/// <summary>
/// 双倍数据类
/// </summary>
public class DoubleData : AItemData
{
    private StateMachine<ItemState> _double_Machine;

    public StateMachine<ItemState> Double_Machine
    {
        get
        {
            if (_double_Machine == null)
            {
                _double_Machine = new StateMachine<ItemState>();
            }
            return _double_Machine;
        }
    }

    public override bool IsRun()
    {
        if (Double_Machine.CurrentState == ItemState.Double)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void EnterState()
    {
        GameGuidance.Instance.GuidanceDouble++;
        Double_Machine.CurrentState = ItemState.Double;
        //Times++;
        //关闭还未结束的此状态协程
        if (PreviousCoroutine != null)
        {
            HumanManager.Nature.Human_Script.StopCoroutine(PreviousCoroutine);
        }
        PreviousCoroutine = HumanManager.Nature.Human_Script.StartCoroutine(DoubleTime());
        OpenItemCollider();
    }
    IEnumerator DoubleTime()
    {
        float times = 0;
        yield return new WaitUntil(() =>
        {
            if (MyKeys.Pause_Game)
                return false;

            times++;
            return times >= ItemColliction.Double.Time * 60;
        });
        ExitState();
    }
    protected override void ExitState()
    {
        Double_Machine.CurrentState = ItemState.Null;
        ItemCollider();
        PreviousCoroutine = null;
    }

    public override void Update()
    {
        Double_Machine.Update();
    }

    public override void Initialize()
    {
        Double_Machine.AddState(ItemState.Double, new Double(HumanManager.Nature));
        Double_Machine.AddState(ItemState.Null, new Null(HumanManager.Nature));
        Double_Machine.CurrentState = ItemState.Null;
        Time = HumanManager.Nature.Double_Time;
    }
    //检测是否开启或关闭碰撞框
    public void ItemCollider()
    {
        if (ItemColliction.Magnet.IsRun() || ItemColliction.Dash.IsRun()||ItemColliction.StartDash.IsRun())
        {
            HumanColliderManager.Item_Collider.SetActive(true);
        }
        else
        {
            HumanColliderManager.Item_Collider.SetActive(false);
        }
    }
}

/// <summary>
/// 护盾数据类
/// </summary>
public class ProtectData : AItemData
{
    private StateMachine<ItemState> _protect_Machine;

    private Coroutine _coroutine;

    public StateMachine<ItemState> Protect_Machine
    {
        get
        {
            if (_protect_Machine == null)
            {
                _protect_Machine = new StateMachine<ItemState>();
            }
            return _protect_Machine;
        }
    }

    public override bool IsRun()
    {
        if (Protect_Machine.CurrentState == ItemState.Protect)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void EnterState()
    {
        GameGuidance.Instance.GuidanceProtect++;
        if (_coroutine != null)
        {
            HumanManager.Nature.Human_Script.StopCoroutine(_coroutine);
        }
        Protect_Machine.CurrentState = ItemState.Protect;
        _coroutine = HumanManager.Nature.Human_Script.StartCoroutine(ProtectTime());
    }

    IEnumerator ProtectTime()
    {
        float times = 0;
        yield return new WaitUntil(() =>
        {
            if (MyKeys.Pause_Game)
                return false;

            times++;
            return times >= ItemColliction.Protect.Time * 60;
        });
        ExitState();
    }

    public void Exit()
    {
        ExitState();
    }

    protected override void ExitState()
    {
        Protect_Machine.CurrentState = ItemState.Null;
    }

    public override void Update()
    {
        Protect_Machine.Update();
    }

    public override void Initialize()
    {
        Protect_Machine.AddState(ItemState.Protect, new Protect(HumanManager.Nature));
        Protect_Machine.AddState(ItemState.Null, new Null(HumanManager.Nature));
        Protect_Machine.CurrentState = ItemState.Null;
        Time = HumanManager.Nature.Protect_Time;
    }
}

/// <summary>
/// 无敌数据类
/// </summary>
public class SuperManData : AItemData
{
    private StateMachine<ItemState> _super_Machine;

    public StateMachine<ItemState> Super_Machine
    {
        get
        {
            if (_super_Machine == null)
            {
                _super_Machine = new StateMachine<ItemState>();
            }
            return _super_Machine;
        }
    }

    public override bool IsRun()
    {
        if (Super_Machine.CurrentState == ItemState.SuperMan)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool WhetherSpeedUp()
    {
        if (IsRun() && Time > 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void EnterState()
    {
        if (!GameUIManager.Is_Resurgence && Time > 2)
        {
            GameGuidance.Instance.GuidanceSuperMan++;
        }
            
        Super_Machine.CurrentState = ItemState.SuperMan;
        //Times++;
        //关闭还未结束的此状态协程
        if (PreviousCoroutine != null)
        {
            HumanManager.Nature.Human_Script.StopCoroutine(PreviousCoroutine);
        }
        PreviousCoroutine = HumanManager.Nature.Human_Script.StartCoroutine(SuperTime());
    }

    IEnumerator SuperTime()
    {
        float times = 0;
        yield return new WaitUntil(() =>
        {
            if (MyKeys.Pause_Game)
                return false;

            times++;
            return times >= ItemColliction.SuperMan.Time * 60;
        });
        ExitState();
    }

    protected override void ExitState()
    {
        Super_Machine.CurrentState = ItemState.Null;
        Time = HumanManager.Nature.Super_Man_Time;
        PreviousCoroutine = null;
    }

    public override void Update()
    {
        Super_Machine.Update();
    }

    public override void Initialize()
    {
        Super_Machine.AddState(ItemState.SuperMan, new SuperMan(HumanManager.Nature));
        Super_Machine.AddState(ItemState.Null, new Null(HumanManager.Nature));
        Super_Machine.CurrentState = ItemState.Null;
        Time = HumanManager.Nature.Super_Man_Time;
    }
}

/// <summary>
/// 磁铁数据类
/// </summary>
public class MagnetData : AItemData
{
    private StateMachine<ItemState> _magnet_Machine;

    public StateMachine<ItemState> Magnet_Machine
    {
        get
        {
            if (_magnet_Machine == null)
            {
                _magnet_Machine = new StateMachine<ItemState>();
            }
            return _magnet_Machine;
        }
    }

    public override bool IsRun()
    {
        if (Magnet_Machine.CurrentState == ItemState.Magnet)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public override void EnterState()
    {
        GameGuidance.Instance.GuidanceMagnet++;
        Magnet_Machine.CurrentState = ItemState.Magnet;
        //Times++;
        //关闭还未结束的此状态协程
        if (PreviousCoroutine != null)
        {
            HumanManager.Nature.Human_Script.StopCoroutine(PreviousCoroutine);
        }
        PreviousCoroutine = HumanManager.Nature.Human_Script.StartCoroutine(MagnetTime());
        OpenItemCollider();
    }

    IEnumerator MagnetTime()
    {
        float times = 0;
        yield return new WaitUntil(() =>
        {
            if (MyKeys.Pause_Game)
                return false;

            times++;
            return times >= ItemColliction.Magnet.Time * 60;
        });
        ExitState();
    }

    protected override void ExitState()
    {
        Magnet_Machine.CurrentState = ItemState.Null;
        HumanManager.Nature.Human.GetComponentInChildren<FruitCollider>().gameObject.SetActive(true);
        ItemCollider();
        PreviousCoroutine = null;
    }

    public override void Update()
    {
        Magnet_Machine.Update();
    }

    public override void Initialize()
    {
        Magnet_Machine.AddState(ItemState.Magnet, new Magnet(HumanManager.Nature));
        Magnet_Machine.AddState(ItemState.Null, new Null(HumanManager.Nature));
        Magnet_Machine.CurrentState = ItemState.Null;
        Time = HumanManager.Nature.Magnet_Time;
    }
    //检测是否开启或关闭碰撞框
    public void ItemCollider()
    {
        if (ItemColliction.Double.IsRun() || ItemColliction.Dash.IsRun() || ItemColliction.StartDash.IsRun())
        {
            HumanColliderManager.Item_Collider.SetActive(true);
        }
        else
        {
            HumanColliderManager.Item_Collider.SetActive(false);
        }
    }
}

/// <summary>
/// 开始冲刺类
/// </summary>
public class StartDashData : AItemData
{

    private StateMachine<ItemState> _start_Dash_Machine;

    public StateMachine<ItemState> Start_Dash_Machine
    {
        get
        {
            if (_start_Dash_Machine == null)
            {
                _start_Dash_Machine = new StateMachine<ItemState>();
            }
            return _start_Dash_Machine;
        }
    }

    public override bool IsRun()
    {
        if (Start_Dash_Machine.CurrentState == ItemState.StartDash)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public int Steps { set; get; }

    public override void EnterState()
    {
        Start_Dash_Machine.CurrentState = ItemState.StartDash;
        OpenItemCollider();
    }


    protected override void ExitState()
    {
        BuyTheItems.start_Dash_Bool = false;
        Start_Dash_Machine.CurrentState = ItemState.Null;
        HumanManager.Nature.HumanManager_Script.CurrentState = HumanState.Jump;
        ItemColliction.SuperMan.Time = 1.0f;
        HumanManager.Nature.HumanManager_Script.ItemState = ItemState.SuperMan;
        ItemCollider();
    }

    //因为开始冲刺判定条件是步数，所以在外部判定满足步数才退出
    public void WhetherExit(int now_Steps)
    {
        if (now_Steps == Steps)
        {
            ExitState();
        }
    }

    public override void Update()
    {
        Start_Dash_Machine.Update();
    }

    public override void Initialize()
    {
        Start_Dash_Machine.AddState(ItemState.StartDash, new Dash(HumanManager.Nature));
        Start_Dash_Machine.AddState(ItemState.Null, new Null(HumanManager.Nature));
        Start_Dash_Machine.CurrentState = ItemState.Null;
    }
    //检测是否开启或关闭碰撞框
    public void ItemCollider()
    {
        if (ItemColliction.Magnet.IsRun() || ItemColliction.Double.IsRun())
        {
            HumanColliderManager.Item_Collider.SetActive(true);
        }
        else
        {
            HumanColliderManager.Item_Collider.SetActive(false);
        }
    }
}
/// <summary>
/// 死亡冲刺类
/// </summary>
public class DeadDashData : AItemData
{
    private StateMachine<ItemState> _dead_Dash_Machine;

    public StateMachine<ItemState> Dead_Dash_Machine
    {
        get
        {
            if (_dead_Dash_Machine == null)
            {
                _dead_Dash_Machine = new StateMachine<ItemState>();
            }
            return _dead_Dash_Machine;
        }
    }

    /// <summary>
    /// DeadDashData中的此方法，是用来判定是否可以执行死亡冲刺
    /// </summary>
    /// <returns></returns>
    public override bool IsRun()
    {
        if (Dead_Dash_Machine.CurrentState == ItemState.DeadDash)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool IsBuy()
    {
        if (BuyTheItems.dead_Dash_Bool)
        {
            return true;
        }
        return false;
    }


    public override void EnterState()
    {
        Dead_Dash_Machine.CurrentState = ItemState.DeadDash;
        HumanManager.Nature.Human_Script.StartCoroutine(DashTime());
        OpenItemCollider();
    }
    IEnumerator DashTime()
    {
        float times = 0;
        yield return new WaitUntil(() =>
        {
            if (MyKeys.Pause_Game)
                return false;

            times++;
            return times >= ItemColliction.Dash.Time * 60;
        });
        ExitState();
    }

    protected override void ExitState()
    {
        BuyTheItems.dead_Dash_Bool = false;
        Dead_Dash_Machine.CurrentState = ItemState.Null;
        HumanManager.Nature.HumanManager_Script.CurrentState = HumanState.Dead;
    }

    public override void Update()
    {
        Dead_Dash_Machine.Update();
    }

    public override void Initialize()
    {
        Dead_Dash_Machine.AddState(ItemState.DeadDash, new Dash(HumanManager.Nature));
        Dead_Dash_Machine.AddState(ItemState.Null, new Null(HumanManager.Nature));
        Dead_Dash_Machine.CurrentState = ItemState.Null;
        Time = HumanManager.Nature.Dash_Time;
    }
}
