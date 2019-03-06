using UnityEngine;
using System.Collections.Generic;

//人物进化属性脚本
public abstract class HumanEvolution : MonoBehaviour
{
    /// <summary>
    /// 跳跃斜率(y/x)
    /// </summary>
    protected float _slope;
    public float Slope { get { return _slope; } }

	protected float _jump_Speed;		//跳跃过程的速度
	public float Jump_Speed{ get{ return _jump_Speed; } }

	protected float _run_Speed;		//人物奔跑速度
	public float Run_Speed{ get{ return _run_Speed; } }

	protected float _slow_Speed;		//减速后的速度
	public float Slow_Speed{ get{ return _slow_Speed; } }

	protected float _dash_Time;		//冲锋时间
	public float Dash_Time{ get{ return _dash_Time; } }

	protected float _double_Time;	//双倍时间
	public float Double_Time{ get{ return _double_Time; } }

	protected float _protect_Time;	//护盾时间
	public float Protect_Time{ get{ return _protect_Time; } }

	protected float _superMan_Time;	//无敌时间
	public float SuperMan_Time{ get{ return _superMan_Time; } }

	protected float _magnet_Time;	//磁铁时间
	public float Magnet_Time{ get{ return _magnet_Time; } }



	protected List<HumanSkill> _inherent_Skill = new List<HumanSkill>();//角色持有的固有技能
	public List<HumanSkill> InherentSkillList{ get{ return _inherent_Skill; } }

	public abstract void AddSkill (List<HumanSkill> skill_List);
	public abstract void HumanNature(int level);//人物属性

}
