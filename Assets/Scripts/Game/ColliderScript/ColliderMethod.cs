using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public abstract class AColliderMethod
{
	protected GameResource.HumanNature _nature;
	protected Transform _collider;
    protected SkinnedMeshRenderer _human_Mesh;

	public abstract void OnExecute ();

    //计算墙体内侧边缘X轴坐标
    protected float XValue(ref HumanState state, int mark, SkinnedMeshRenderer human_Mesh, Renderer wall_Mesh)
    {
        if (wall_Mesh == null)
        {
            Debug.Log("wall_Mesh未赋值");
            return 100;
        }
        switch (mark)
        {
            case 1:
                float xLeft = wall_Mesh.bounds.max.x + human_Mesh.bounds.extents.y / 2;
                return xLeft;

            case 2:
                float xRight = wall_Mesh.bounds.min.x - human_Mesh.bounds.extents.y / 2;
                return xRight;

            default:
                Debug.Log("人物X取值有问题！");
                return 100;
        }

    }

    //碰撞墙体后，进行人物坐标的修正
    protected void HumanPosition(Transform human,float x_Wall_Inside)
    {
        var target_Position = new Vector3 (x_Wall_Inside, human.position.y, human.position.z);
        human.position = target_Position;
    }

    protected void PlayAnimator(Animator animator)
    {
        animator.SetBool("OnOff",true);
    }
}

public class Wall:AColliderMethod
{
    public Wall(GameResource.HumanNature nature,Transform wall)
	{
		_nature = nature;
		_collider = wall;
        _human_Mesh = GameObject.FindGameObjectWithTag("Player").GetComponent<SkinnedMeshRenderer>();
	}
	public override void OnExecute()
	{
        WallExecute(_nature, _collider); 
	}

	void WallExecute(GameResource.HumanNature nature,Transform wall)
	{
	    if (Time.timeSinceLevelLoad <= 0.5f)
        {
            return;
        }
            
		HumanState state = nature.HumanManager_Script.CurrentState;

        //计算人物在墙内侧的x轴坐标
        float x = JudgeX(HumanManager.WallMark,nature,nature.Off_Set);

		if (x == 10000) {
			Debug.Log ("获取墙内侧坐标失败!");
			return;
		}

        switch (state)
        {
            case HumanState.Run:
            case HumanState.Revise:
            case HumanState.Slowdown:
                state = HumanState.Stop;
                break;
            case HumanState.Jump:
                HumanPosition(nature.Human, x);
                if (Ice.Is_Jump_Before)
                {
                    state = HumanState.Slowdown;
                }
                else
                {
                    state = HumanState.Revise;
                }
                
                break;
        }
	    Ice.Is_Jump_Before = false;
        
        nature.HumanManager_Script.CurrentState = state;

        if (Jump._preprocessing_Bool)
        {
            Jump.Preprocessing();
        }
    }

    //判断，获取墙体内侧x坐标
    float JudgeX(WallMark mark, GameResource.HumanNature nature, float off_Set)
    {
        switch (mark)
        {
            case WallMark.Left:
                return (nature.Left_Wide_Wall_Inside);
            case WallMark.Right:
                return (nature.Right_Wide_Wall_Inside);
            default:
                Debug.Log("mark值有问题");
                return 10000;
        }
    }

}
public class Ice : AColliderMethod
{
    public static bool Is_Jump_Before = false;//碰撞冰的时候，是否是跳跃状态
    public static bool IsOnIce { get; set; }//目前是否在冰面上
    public Ice(GameResource.HumanNature nature, Transform wall)
    {
        _nature = nature;
        _collider = wall;
        _human_Mesh = GameObject.FindGameObjectWithTag("Player").GetComponent<SkinnedMeshRenderer>();
    }
    public override void OnExecute()
    {
        WallExecute(_nature, _collider);
    }

    void WallExecute(GameResource.HumanNature nature, Transform wall)
    {
        if (Time.time == 0.0f)
            return;
        if(IsOnIce)
            return;
        if (_nature.HumanManager_Script.CurrentState == HumanState.Jump)
        {
            Is_Jump_Before = true;
        }
        else
        {
            Is_Jump_Before = false;
            _nature.HumanManager_Script.CurrentState = HumanState.Slowdown;
        }
        IsOnIce = true;
    }
}
public class Spring:AColliderMethod
{
    public static bool IsJump { get; set; }//连续跳跃判定条件的开关（和正常的判定有冲突，不开会出bug）
    private static float _time;//记录碰撞的时间
	public Spring(GameResource.HumanNature nature,Transform spring)
	{
		_nature = nature;
		_collider = spring;
	}

	public override void OnExecute ()
	{
	    if (Time.time - _time > 0.2f)
	    {
	        _time = Time.time;
            _nature.Human_Script.StartCoroutine(SpringFunction(_nature, _collider));
        }
	}

    IEnumerator SpringFunction(GameResource.HumanNature nature, Transform spring)
    {
        //HumanState state = HumanState.Stop;
        //register_State(state, state_Machines, nature, item);
        bool temp_Bool = JudgeState(nature, spring);

        nature.Human_Ani.SetInteger(StaticParameter.Ani_Key_HumanState, 7);

        Animator ani = spring.GetComponent<Animator>();
        //判定是否播放弹簧动画
        if (temp_Bool)
        {
            //调整人物角度
            switch (HumanManager.WallMark)
            {
                case WallMark.Left:
                    _nature.Human.eulerAngles = Vector3.up * 180;
                    break;
                case WallMark.Right:
                    _nature.Human.eulerAngles = Vector3.zero;
                    break;
            }
            ani.SetBool(StaticParameter.Ani_Key_OnOff, true);
        }
        else
        {
            if (ItemColliction.SuperMan.IsRun())
            {
                //摄像机震动
                CameraShake.Is_Shake_Camera = true;
                ani.SetBool(StaticParameter.Ani_Key_Broke, true);
            }
        }
        yield return new WaitForSeconds(0.17f);

        if (temp_Bool)
        {
            GameResource.HumanNature._follow_Spring.is_Start = false;
            SpringExecute(_nature, _collider);
        }
        ani.SetBool(StaticParameter.Ani_Key_OnOff, false);
        ani.SetBool(StaticParameter.Ani_Key_Broke, false);
        //AColliderMethod method = new Spring(register_State, state_Machines, nature, spring, item);
        //method.OnExecute();

    }


    //判断碰撞弹簧后的状态 
    bool JudgeState(GameResource.HumanNature nature, Transform spring)
    { 
        bool temp;
        SkinnedMeshRenderer spring_Renderer = spring.GetComponentInChildren<SkinnedMeshRenderer>();

        if (nature.HumanManager_Script.CurrentState == HumanState.Jump)//nature.Human_Mesh.bounds.center.y > spring_Renderer.bounds.min.y&&
        {
            //可以发动弹簧
            temp = true;
            //重置人物状态和位置，并给人物跟随弹簧移动的参数结构体赋值
            SetHumanPosition(nature, spring, spring_Renderer);
        }
        else
        {
            if (nature.Human_Mesh.bounds.center.y >= spring_Renderer.bounds.min.y - 0.2f
            && nature.Human_Mesh.bounds.center.y <= spring_Renderer.bounds.max.y + 0.2f
            && IsJump)
            {
                temp = true;
                SetHumanPosition(nature, spring, spring_Renderer);
                IsJump = false;
            }
            else
            {
                //不能发动弹簧
                temp = false;

                if (!ItemColliction.SuperMan.IsRun())
                {
                    nature.HumanManager_Script.CurrentState = HumanState.Stop;
                }
            }
            
        }

        
        
        return temp;
    }
    //重置人物状态和位置，并给人物跟随弹簧移动的参数结构体赋值
    void SetHumanPosition(GameResource.HumanNature nature, Transform spring, SkinnedMeshRenderer spring_Renderer)
    {
        if (ItemColliction.SuperMan.IsRun())
        {
            nature.Human.position = Vector3.right * nature.Human.position.x + Vector3.up * (spring.position.y-0.4f) +
                                    Vector3.forward * nature.Human.position.z;
        }
        else
        {
            nature.Human.position = Vector3.right * nature.Human.position.x + Vector3.up * spring.position.y +
                                   Vector3.forward * nature.Human.position.z;
            
        }

        //给结构体赋值
        GameResource.HumanNature._follow_Spring.is_Start = true;
        GameResource.HumanNature._follow_Spring.spring_Renderer = spring_Renderer;

        nature.HumanManager_Script.CurrentState = HumanState.Stop;

    }

    void SpringExecute( GameResource.HumanNature nature,Transform spring)
	{
        //播放音效
        MyAudio.PlayAudio(StaticParameter.s_Spring, false, StaticParameter.s_Spring_Volume);

        HumanState state = HumanState.Jump;

	    if (spring.tag.Contains(MyTags.Left_Tag))
	    {
	        HumanManager.WallMark = WallMark.Right;
	    }
	    else
	    {
            HumanManager.WallMark = WallMark.Left;
        }

	    HumanManager.JumpMark = JumpMark.SpeedUp;//改变跳跃类型

	    nature.HumanManager_Script.CurrentState = state;

	}
}

public class DeadFunction : AColliderMethod
{
    SpriteRenderer _wall_Mesh;
    public DeadFunction(GameResource.HumanNature nature,Transform ob)
	{
		_nature = nature;
		_collider = ob;
        _human_Mesh = GameObject.FindGameObjectWithTag("Player").GetComponent<SkinnedMeshRenderer>();
        _wall_Mesh = ob.GetComponent<SpriteRenderer>();
    }

	public override void OnExecute ()
	{
		DeadExecute (_nature);
	}
	//死亡状态的赋值方法
	void DeadExecute(GameResource.HumanNature nature)
	{
        if(ItemColliction.DeadDash.IsRun())
            return;
        //摄像机震动
        CameraShake.Is_Shake_Camera = true;

        nature.HumanManager_Script.CurrentState = HumanState.Dead;
    }
}

public class Fruit : AColliderMethod
{
	FruitScript fruitScript;

	public Fruit(Transform fruit)
	{
		fruitScript = fruit.GetComponent<FruitScript> ();
		_collider = fruit;
	}
	public override void OnExecute ()
	{
        //   if (ItemColliction.Magnet.IsRun())
        //{
        //       if (HumanManager.Nature.Human_Script.CurrentState != HumanState.Jump)
        //       {
        //           HumanManager.Nature.Human_Ani.SetInteger(StaticParameter.Ani_Key_HumanState, 1);
        //       }
        //       fruitScript.CutFruit();
        //   }
        fruitScript.CutFruit();
    }

}

//新手引导标记
public class Chest : AColliderMethod
{
    private ChestScript _chest;
    
    public Chest(Transform chest)
    {
        _chest = chest.GetComponent<ChestScript>();
    }
    public override void OnExecute()
    {
        _chest.Excute();
    }
}

public class Toturial : AColliderMethod
{
    private GameObject _toturial;
    public Toturial(Transform chest)
    {
        _toturial = chest.gameObject;
    }
    public override void OnExecute()
    {
        Object.Destroy(_toturial);
    }
}


