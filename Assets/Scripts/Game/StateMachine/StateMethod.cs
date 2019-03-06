using System.Collections;
using UnityEngine;

//状态方法接口
public interface IStateMethod  {
	void OnEnter();		//进入状态
	void OnExecute(); 	//执行状态
	void OnExit();		//退出状态
}

public class Run:IStateMethod
{
	float _y_Speed;
	Transform _human;
	Animator _human_Ani;
	float _time;
    private GameResource.HumanNature _nature;

    public Run(GameResource.HumanNature nature)
	{
        _nature = nature;
        _y_Speed = nature.Run_Speed;
		_human = nature.Human;
		_human_Ani = nature.Human_Ani;
		if (nature.Run_Speed <= 0) {
			Debug.Log ("奔跑速度不能为0或为负！");
			return;
		}

	}
	public void OnEnter(){
        _time = 0;
		_human_Ani.SetInteger (StaticParameter.Ani_Key_HumanState, 0);
    }
	public void OnExecute(){
	    if (ItemColliction.SuperMan.WhetherSpeedUp())
	    {
	        _human.Translate(0, _y_Speed*1.5f, 0, Space.World);
	    }
	    else
	    {
            _human.Translate(0, _y_Speed, 0, Space.World);
        }
		
		_time += Time.deltaTime;
		if(_time>0.2f){
			_time = 0;
            //播放音效
            MyAudio.PlayAudio ( StaticParameter.s_Run,false,StaticParameter.s_Run_Volume);
		}
	}
	public void OnExit(){
    }

}
public class Jump: IStateMethod
{
    #region 属性,字段及构造函数
    private readonly float _y_Speed;                //y轴上的速度
    private readonly Animator _human_Ani;           //人物动画组件
    private readonly ItemState _item;               //道具状态
    private readonly bool _jump_Bool;               //二段跳技能是否开启的标记
    private readonly int _jump_Frames;              //跳跃帧数
    private readonly int _spring_Jump_Frames;       //踩弹簧之后的跳跃帧数
    private readonly Vector3 _direction_Right;      //向右跳跃的方向向量
    private readonly Vector3 _direction_Left;        //向左跳跃的方向向量

    
    private int _jump_Times;                        //记录跳跃次数，实现二段跳
    private int _jump_Times_Max;                    //连续跳跃最大值
  
    private int _times = 0;                         //记录此行为执行帧数
    private float _speed;                           //加速状态下的速度变量
    private float _off_Set;                         //加速状态下加速度

    private readonly float _preprocessing_Frames;   //预处理帧数
    public  static bool _preprocessing_Bool;         //预处理开关
    public  static bool _judge_Bool;                 //预处理开始判断的开关
    private static Transform _human;
    private static GameResource.HumanNature _nature;

    private readonly Transform _fruit_Collider_Transform;

    private readonly float _x_Wall_Inside_Left;
    private readonly float _x_Wall_Inside_Right;

    private float _camera_Speed;
    private float _distance;
    
    public Jump(GameResource.HumanNature nature)
	{
	    _nature = nature;
		_y_Speed = nature.Run_Speed;
		_human = nature.Human;
		_human_Ani = nature.Human_Ani;
		_jump_Bool = false;
        _jump_Frames = 27;
	    _spring_Jump_Frames = _jump_Frames - 3;
        _preprocessing_Frames = 5;
        _x_Wall_Inside_Left  = _nature.Left_Wall_Inside  + _nature.Human_Height*(0.5f - _nature.Off_Set);
        _x_Wall_Inside_Right = _nature.Right_Wall_Inside - _nature.Human_Height*(0.5f - _nature.Off_Set);
        _fruit_Collider_Transform = _human.GetComponentInChildren<FruitCollider>().transform;

        float n =  nature.Jump_Speed/ nature.Run_Speed ; 
        _direction_Right = Vector3.right * n + Vector3.up;
        _direction_Left = Vector3.left * n + Vector3.up;
        foreach (HumanSkill i in nature.Skill_List) {
			if (i == HumanSkill.DoubleJump) {
				_jump_Bool = true;
			}
		}
        
	}
    #endregion

    #region 行为基本函数

    public void OnEnter()
    {
        
        //初始化墙体碰撞保存对象
        HumanColliderManager.WideWallColliderBefore = null;

        //调整水果碰撞框的角度和位置
        ChangeFruitCollider();
        
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
        HumanColliderManager.Instance.ChangeColliderRotation();
        FruitCollider.Instance.ChangeColliderRotation();

        //播放音效
        MyAudio.PlayAudio(StaticParameter.s_Jump, false, StaticParameter.s_Jump_Volume);
        //跳跃动画的播放
        if (_item == ItemState.SuperMan)
        {
            _human_Ani.SetInteger(StaticParameter.Ani_Key_HumanState, 5);
        }
        else
        {
            _human_Ani.SetInteger(StaticParameter.Ani_Key_HumanState, 3);
        }

        //数据初始化
        _preprocessing_Bool = false; //初始化预处理开关
        _judge_Bool = false;
        _jump_Times = 0;
        _jump_Times_Max = 2;
        _times = 0;
        _speed = CalculateSpeed(_spring_Jump_Frames, ref _off_Set, _nature);


        _distance = Camera.main.transform.position.y - _human.position.y- _nature.Difference_Y;
        _camera_Speed = _distance/8;


        if (MyKeys.CurrentSelectedHero == MyKeys.CurrentHero.YuZi ||
                MyKeys.CurrentSelectedHero == MyKeys.CurrentHero.ShouSi)
        {
            _nature.Human_Phantom.enabled = true;
        }
        else
        {
            if (HumanManager.JumpMark == JumpMark.SpeedUp)
            {
                _nature.Human_Phantom.enabled = true;
                Spring.IsJump = true;
            }
        }
           
    }

    public void OnExecute()
    {
        if (_y_Speed <= 0)
        {
            Debug.Log("速度不能为0或为负！");
            return;
        }
        //多段跳技能
        if (!JudgePreprocessingDisrance() && !_judge_Bool)//_times < (_jump_Frames - _preprocessing_Frames)
        {
            JumpTimes();
        }
        else
        {
            _judge_Bool = true;
        }
        //跳跃预判
        if (_judge_Bool && Input.GetMouseButtonDown(0) && !_preprocessing_Bool)
        {
            _preprocessing_Bool = true;
        }
        //判断跳跃模式
        if (HumanManager.JumpMark == JumpMark.SpeedUp)
        {
            _distance = _distance - _camera_Speed;
            _speed = _speed - _off_Set;
            Move(_speed, _human);
            if (_distance > 0)
            {
                Camera.main.transform.position = Vector3.right * Camera.main.transform.position.x +
                                             Vector3.up * (_human.position.y + _distance+ _nature.Difference_Y) +
                                             Vector3.forward * Camera.main.transform.position.z;
            }
            else
            {
                Camera.main.transform.position = Vector3.right * Camera.main.transform.position.x +
                                             Vector3.up * (_human.position.y + _nature.Difference_Y) +
                                             Vector3.forward * Camera.main.transform.position.z;
            }
            
        }
        else
        {
            Move(_y_Speed, _human);
        }
        ChangeState(_times, _jump_Frames, _x_Wall_Inside_Left, _x_Wall_Inside_Right, _nature.HumanManager_Script);
        _times++;
    }

    public void OnExit()
    {
        HumanManager.JumpMark = JumpMark.Normal;//恢复跳跃状态为默认状态

        //调整人物角度
        switch (HumanManager.WallMark)
        {
            case WallMark.Left:
                _human.eulerAngles = Vector3.forward * -90;
                break;
            case WallMark.Right:
                _human.eulerAngles = Vector3.up * 180 + Vector3.forward * -90;
                break;
        }
        HumanColliderManager.Instance.ChangeRotationToDefault();
        FruitCollider.Instance.ChangeRotationToDefault();

        //恢复碰撞框的位置和角度
        ResetFruitCollider();

        //播放音效
        MyAudio.PlayAudio(StaticParameter.s_Down, false, StaticParameter.s_Down_Volume);

        _nature.Human_Phantom.enabled = false;

        HumanManager.Nature.Human_Script.StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
        if (HumanManager.Nature.HumanManager_Script.CurrentState != HumanState.Jump)
        {
            Spring.IsJump = false;
        }
    }

    #endregion

    #region 功能函数
    //多段跳技能
    void JumpTimes()
    {
        if (Input.GetMouseButtonDown(0) && _jump_Bool && (_jump_Times < _jump_Times_Max) && (_jump_Times > 0))
        {
            switch (HumanManager.WallMark)
            {
                case WallMark.Left:
                    HumanManager.WallMark = WallMark.Right;
                    _human.rotation = new Quaternion(0f, 0.7f, 0f, 0.7f);
                    break;
                case WallMark.Right:
                    HumanManager.WallMark = WallMark.Left;
                    _human.rotation = new Quaternion(0f, -0.7f, 0f, 0.7f);
                    break;
            }
        }
        if (Input.GetMouseButtonDown(0) && _jump_Times < _jump_Times_Max)
        {
            _jump_Times++;
        }
    }

    bool JudgePreprocessingDisrance()
    {
        switch (HumanManager.WallMark)
        {
            case WallMark.Left:
                if (_human.position.x <= _nature.Left_Wall_Inside + 0.6f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case WallMark.Right:
                if (_human.position.x >= _nature.Right_Wall_Inside - 0.6f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            default:
                return false;
        }
    }
    //预处理函数
    public static void Preprocessing()
    {
        if(GuidanceBase.GuidanceMark == 0)
            return;
        switch (HumanManager.WallMark)
        {
            case WallMark.Left:
                HumanManager.WallMark = WallMark.Right;
                _human.rotation = new Quaternion(0f, 0.7f, 0f, 0.7f);
                break;
            case WallMark.Right:
                HumanManager.WallMark = WallMark.Left;
                _human.rotation = new Quaternion(0f, -0.7f, 0f, 0.7f);
                break;
        }
        _nature.HumanManager_Script.CurrentState = HumanState.Jump;
        //为避免快速点击时，动画会暂停播放的情况，手动设置播放
        _nature.Human_Ani.PlayInFixedTime("Jump");
    }
    //移动函数
    void Move(float speed,Transform human)
    {
        float times = 1;
        if (ItemColliction.SuperMan.IsRun())
        {
            times = 1.5f;
        }
        switch (HumanManager.WallMark)
        {
            case WallMark.Left:
                human.Translate(_direction_Left* speed * times, Space.World);
                break;
            case WallMark.Right:
                human.Translate(_direction_Right* speed * times, Space.World);
                break;
        }
    }

    //计算加速运动时的加速度
    float CalculateSpeed(int frames, ref float off_Set, GameResource.HumanNature nature)
    {
        float base_Speed = nature.Run_Speed*0.4f;//减速后的最低速度

        float distance_Y = nature.Run_Speed * (frames +8) - base_Speed* frames;

        float num = 0;
        for (int i = 0; i < frames; i++)
        {
            num += i;
        }

        off_Set = distance_Y / num;//速度的递减量

        float speed_Max = off_Set * frames + base_Speed;
        return speed_Max;
    }

    //切换函数
    void ChangeState(float times,float jump_Frames,float x_Wall_Inside_Left,float x_Wall_Inside_Right,HumanManager human)
    {
        if (jump_Frames - times < 0)
        {
            JumpEnd();
        }

        switch (HumanManager.WallMark)
        {
            case WallMark.Left:
                if (HumanManager.Nature.Human_Mesh.bounds.center.x+1.2f <= x_Wall_Inside_Left)
                {
                    JumpEnd();
                }
                break;
            case WallMark.Right:
                if (HumanManager.Nature.Human_Mesh.bounds.center.x-1.2f >= x_Wall_Inside_Right)
                {
                    JumpEnd();
                }
                break;
        }
    }
    //跳跃结束后改变状态
    void JumpEnd()
    {
        _nature.HumanManager_Script.CurrentState = HumanState.Revise;
        float x_Wall_Inside = 0;
        switch (HumanManager.WallMark)
        {
            case WallMark.Left:
                x_Wall_Inside = _nature.Left_Wall_Inside;// + _nature.Human_Height * (0.5f - _nature.Off_Set);
                break;
            case WallMark.Right:
                x_Wall_Inside = _nature.Right_Wall_Inside + 0.4f;// - _nature.Human_Height * (0.5f - _nature.Off_Set - 0.25f);
                break;
        }
        var target_Position = new Vector3(x_Wall_Inside, _nature.Human.position.y, _nature.Human.position.z);
        _nature.Human.position = target_Position;

        if (_preprocessing_Bool)
        {
            Preprocessing();
        }
    }

    void ChangeFruitCollider()
    {
        _fruit_Collider_Transform.localEulerAngles = Vector3.right * 90;
        _fruit_Collider_Transform.localPosition = Vector3.up * 1.1f + Vector3.forward * 0.7f;
    }

    void ResetFruitCollider()
    {
        _fruit_Collider_Transform.localPosition = Vector3.up + Vector3.forward * (-0.18f);
        _fruit_Collider_Transform.localEulerAngles = Vector3.zero;
    }
    #endregion
}
public class Slowdown:IStateMethod
{
	float _y_Speed;
	Transform _human;
	Animator _human_Ani;
    HumanManager _human_Script;
	public Slowdown(GameResource.HumanNature nature)
	{
		_y_Speed = nature.Slow_Speed;
        _human = nature.Human;
		_human_Ani = nature.Human_Ani;
		if (nature.Slow_Speed <= 0) {
			Debug.Log ("奔跑速度不能为0或为负！");
			return;
		}

	}
	public void OnEnter(){
        _human_Script = _human.GetComponent<HumanManager>();
		_human_Ani.SetInteger (StaticParameter.Ani_Key_HumanState, 0);
    }
	public void OnExecute(){
        _human_Script.IsInScreen();
		_human.Translate(0,_y_Speed,0,Space.World);
	}
	public void OnExit(){
		
	}
}
public class Revise:IStateMethod
{
    enum HumanPosition
    {
        Up,
        Down
    }

    private readonly Transform _camera;
    private readonly Transform _human;
    private readonly Animator _human_Ani;
    private readonly float _difference;
    private float _add_Once;
    readonly float _revise_Time;//修正到屏幕正常位置所用时间.
    private float _y_Speed;//y轴上的速度
    private HumanPosition _human_Position;
    
    public Revise(GameResource.HumanNature nature)
	{
		_camera = nature.Main_Camera;
		_human_Ani = nature.Human_Ani;
		_human = nature.Human;
		_difference = nature.Difference_Y;
        _revise_Time = 0.5f;
	}
	public void OnEnter(){
        //播放跑步动画
        _human_Ani.SetInteger (StaticParameter.Ani_Key_HumanState, 0);
        //计算坐标偏差
        var temp = _camera.position.y - _human.position.y - _difference;
        //确定当前是在标准位置的上方还是下方，<0是在上面，>0是在下面
        if (temp < 0)
	    {
            _human_Position = HumanPosition.Up;
        }
	    else
	    {
            _human_Position = HumanPosition.Down;
        }
        //计算移动速度
        float speed = HumanManager.Nature.Run_Speed;
        if (ItemColliction.SuperMan.WhetherSpeedUp())
        {
            speed = HumanManager.Nature.Run_Speed*1.5f;
        }
	    
        _add_Once = temp / (60 * _revise_Time);
        _y_Speed = speed + _add_Once;

        //若坐标偏差小于0.1，则直接修正坐标
	    if (Mathf.Abs(temp) < 0.1)
	    {
	        ResetHumanPosition();
	    }
    }
	public void OnExecute(){

        if (MyCamera.GameEnd)
        {
            HumanManager.Nature.HumanManager_Script.CurrentState = HumanState.Run;
            return;
        }

        float temp_Value = _camera.position.y - _human.position.y - _difference;


        //if (temp_Value >= 0f && _add_Once < 0)
        //{
        //    _human.position = Vector3.right * _human.position.x + Vector3.up * (_camera.position.y - _difference) + Vector3.forward * _human.position.z;
        //    HumanManager.Nature.Human_Script.CurrentState = HumanState.Run;

        //}
        //else if (temp_Value <= 0f && _add_Once > 0)
        //{
        //    _human.position = Vector3.right * _human.position.x + Vector3.up * (_camera.position.y - _difference) + Vector3.forward * _human.position.z;
        //    HumanManager.Nature.Human_Script.CurrentState = HumanState.Run;

        //}
        //else {
        //    _human.Translate(0, _y_Speed, 0, Space.World);
        //}

        _human.Translate(0, _y_Speed, 0, Space.World);

        if (_human_Position == HumanPosition.Up)
	    {
	        if (temp_Value >= 0)
	        {
	            ResetHumanPosition();
	        }
	    }
	    else
	    {
            if (temp_Value <= 0)
            {
                ResetHumanPosition();
            }
        }
    }
	public void OnExit(){

	}

    void ResetHumanPosition()
    {
        _human.position = Vector3.right * _human.position.x + Vector3.up * (_camera.position.y - _difference) + Vector3.forward * _human.position.z;
        HumanManager.Nature.HumanManager_Script.CurrentState = HumanState.Run;
    }
}
public class Stop:IStateMethod
{
    private GameResource.HumanNature _nature;
    public Stop(GameResource.HumanNature nature)
    {
        _nature = nature;
    }

	public void OnEnter(){
	}
	public void OnExecute(){
        _nature.HumanManager_Script.IsInScreen();
        
        if (GameResource.HumanNature._follow_Spring.is_Start)
        {
            switch (HumanManager.WallMark)
            {
                case WallMark.Left:
                    _nature.Human.position = Vector3.right * (GameResource.HumanNature._follow_Spring.spring_Renderer.bounds.max.x + _nature.Human_Height * 0.5f) +
                                   Vector3.up * GameResource.HumanNature._follow_Spring.spring_Renderer.bounds.center.y +
                                   Vector3.forward * _nature.Human.position.z;
                    break;
                case WallMark.Right:
                    _nature.Human.position = Vector3.right * (GameResource.HumanNature._follow_Spring.spring_Renderer.bounds.min.x - _nature.Human_Height * 0.5f) +
                                   Vector3.up * GameResource.HumanNature._follow_Spring.spring_Renderer.bounds.center.y +
                                   Vector3.forward * _nature.Human.position.z;
                    break;

            }
           
        }

    }
	public void OnExit(){

	}
}
public class Dead:IStateMethod
{
    private readonly Transform _human;
    private readonly Animator _ani;
    private readonly GameResource.HumanNature _nature;
    public Dead(GameResource.HumanNature nature){
		_human = nature.Human;
		_ani = nature.Human_Ani;
        _nature = nature;
    }
   
	public void OnEnter(){
	    if (ItemColliction.Dash.IsRun()
	        || ItemColliction.StartDash.IsRun())
	    {
	        _nature.HumanManager_Script.CurrentState = _nature.HumanManager_Script.BeforeTheState;
            return;
	    }
        if(ItemColliction.DeadDash.IsRun())
            return;
        //统计死亡步数
	    StatisticsSteps();

        //播放音效
        switch (MyKeys.CurrentSelectedHero)
	    {
            case MyKeys.CurrentHero.GuiYuZi:
                MyAudio.PlayAudio(StaticParameter.s_GuiYuZi_Dead, false, StaticParameter.s_GuiYuZi_Dead_Volume);
                break;
            case MyKeys.CurrentHero.CiShen:
                MyAudio.PlayAudio(StaticParameter.s_CiShen_Dead, false, StaticParameter.s_CiShen_Dead_Volume);
                break;
            case MyKeys.CurrentHero.YuZi:
                MyAudio.PlayAudio(StaticParameter.s_YuZi_Dead, false, StaticParameter.s_YuZi_Dead_Volume);
                break;
        }
        MyAudio.PlayAudio(StaticParameter.s_Damage, false, StaticParameter.s_Damage_Volume);

        //判断是否购买死亡冲刺
        if (ItemColliction.DeadDash.IsBuy()&&!ItemColliction.DeadDash.IsRun())
	    {
            HumanManager.Nature.HumanManager_Script.ItemState = ItemState.DeadDash;
	    }
        else
        {
           HumanManager.Nature.ColliderManager.SetActive(false);
            
            //判断碰撞物体，选择死亡方式
            if (HumanManager.Nature.HumanManager_Script.CurrentCollide!=null 
                && HumanManager.Nature.HumanManager_Script.CurrentCollide.gameObject.layer == MyTags.Sword_Layer)
            {
                _human.gameObject.SetActive(false);
                MyAudio.PlayAudio(StaticParameter.s_Sword_Death, false, StaticParameter.s_Sword_Death_Volume);
                GameObject hero = null;
                switch (MyKeys.CurrentSelectedHero)
                {
                    case MyKeys.CurrentHero.GuiYuZi:
                        hero = StaticParameter.LoadObject("Ninja", "GuiYuZiDead") as GameObject;
                        break;
                    case MyKeys.CurrentHero.CiShen:
                        hero = StaticParameter.LoadObject("Ninja", "CiShenDead") as GameObject;
                        break;
                    case MyKeys.CurrentHero.YuZi:
                        hero = StaticParameter.LoadObject("Ninja", "YuZiDead") as GameObject;
                        break;
                    case MyKeys.CurrentHero.ShouSi:
                        hero = StaticParameter.LoadObject("Ninja", "ShouSiDead") as GameObject;
                        break;
                }

                GameObject copy = Object.Instantiate(hero);
                copy.transform.position = _human.position;
                copy.transform.eulerAngles = Vector3.left*90 + Vector3.down*90;
            }
            else
            {
                _ani.SetInteger(StaticParameter.Ani_Key_HumanState, 4);
                
            }
            //死亡后人物掉落的移动
            HumanManager.Nature.HumanManager_Script.StartCoroutine(FallDown(12f,3f));

            GameUIManager method = Transform.FindObjectOfType<GameUIManager>();
            
            method.TheClicked(GameUI.GameUI_Resurgence);
     
            //调整人物角度
            switch (HumanManager.WallMark)
            {
                case WallMark.Left:
                    _human.eulerAngles = Vector3.right * 20 + Vector3.up * 90;
                    break;
                case WallMark.Right:
                    _human.eulerAngles = Vector3.right * 20 + Vector3.up * 270;
                    break;
            }
        }

	}

    IEnumerator FallDown(float distance,float time)
    {
        float speed = distance/(time*60);
        float times = 0;

        yield return new WaitUntil(() =>
        {
            if (ItemColliction.Dash.IsRun())
            {
                return true;
            }
            else
            {
                times++;
                _human.Translate(0, -speed, 0,Space.World);
                return times >= time * 60;
            }
            
        });

        _human.gameObject.SetActive(true);
    }

    public void OnExecute()
    {
    }
    public void OnExit(){

	}

    void StatisticsSteps()
    {
        int num = Window_Ingame.Steps / 50;

        UMManager.Event(EventID.UnitOne_StateDead, MyKeys.MissionName + " Step:" + (num + 1)*50 );

    }
}
