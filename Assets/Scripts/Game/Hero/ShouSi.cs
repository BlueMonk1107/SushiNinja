using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShouSi : HumanEvolution
{
    private int[] _level_List;//第一项代表第一个技能的等级，以此类推
    public float _run;
    public float _jump;
    void Awake()
    {
        _level_List = new int[5];
        HumanNature(MyKeys.ShouSi_Level_Value);
        AddSkill(_inherent_Skill);    
    }

    public override void AddSkill(List<HumanSkill> skill_List)
    {
        skill_List.Add (HumanSkill.DoubleJump);
        //skill_List.Add (HumanSkill.DoubleGold);
    }

    public override void HumanNature(int level)
    {
        _run_Speed = _run * Time.fixedDeltaTime;        //人物奔跑速度
        _jump_Speed = _jump * Time.fixedDeltaTime;      //减速后的速度

        EachLevel();

        float temp = 4f;

        _protect_Time = 15.0f + _level_List[0] * MyKeys.Protect_Time_Add_Once;    //护盾时间
        _superMan_Time = temp + _level_List[1] * MyKeys.Free_Hero_Time_Add_Once;	   //无敌时间
        _double_Time = temp + _level_List[2] * MyKeys.Free_Hero_Time_Add_Once;	   //双倍时间
        _magnet_Time = temp + _level_List[3] * MyKeys.Free_Hero_Time_Add_Once;    //磁铁时间
        _dash_Time = 3 + _level_List[4] * MyKeys.Dash_Time_Add_Once;	       //冲锋时间


    }

    void EachLevel()
    {
        //计算每种技能的等级
        for (int i = 0; i < MyKeys.ShouSi_Level_Value; i++)
        {
            var index = i % 5;
            _level_List[index] += 1;
        }
    }
}
