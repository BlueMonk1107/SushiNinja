using UnityEngine;
using System.Collections.Generic;

#region 枚举
/// <summary>
/// 人物行为状态
/// </summary>
public enum HumanState
{
    Run,
    Slowdown,
    Revise,//修正坐标
    Jump,
    Stop,
    Dead
}
/// <summary>
/// 道具状态
/// </summary>
public enum ItemState
{
    Null,
    Dash,       //冲锋
    Double,     //双倍
    Protect,    //护盾
    SuperMan,   //无敌
    Magnet,     //磁铁
    StartDash,  //开局冲刺
    DeadDash    //死亡冲刺
}
/// <summary>
/// 人物技能
/// </summary>
public enum HumanSkill
{
    DoubleJump, //二段跳
    DoubleGold, //点金手
    DoubleLife  //两条命
}
/// <summary>
/// 现在所处墙体的标记
/// </summary>
public enum WallMark
{
    Left,
    Right
}

public enum JumpMark
{
    Normal,
    SpeedUp
}
/// <summary>
/// 开始前购买的道具
/// </summary>
public enum Items
{
    /// <summary>
    /// 祝福
    /// </summary>
    Blessing,

    /// <summary>
    /// 清除本关所有机关
    /// </summary>
    DestroyObstacle,

    /// <summary>
    /// 清除本关所有导弹
    /// </summary>
    DestroyMissile,

    /// <summary>
    /// 开局护盾
    /// </summary>
    Protect,

    /// <summary>
    /// 开局无敌
    /// </summary>
    SuperMan,

    /// <summary>
    /// 开局冲刺600
    /// </summary>
    StartDash600,

    /// <summary>
    /// 开局冲刺1200
    /// </summary>
    StartDash1200,

    /// <summary>
    /// 死亡后冲刺
    /// </summary>
    AfterDeadDash
}

#endregion


#region 结构体

/// <summary>
/// 用于保存弹簧开始弹跳时的数据 
/// </summary>
public struct FollowSpring
{
    public bool is_Start;
    public SkinnedMeshRenderer spring_Renderer;

    public FollowSpring(bool a)
    {
        is_Start = a;
        spring_Renderer = null;
    }
}

public class GameResource : MonoBehaviour
{
    public struct HumanNature
    {
        #region 属性

        public static FollowSpring _follow_Spring; //人物跟随弹簧运动的结构体的对象

        //速度
        public readonly float Jump_Speed; //跳跃速度
        public readonly float Run_Speed; //人物奔跑速度
        public readonly float Slow_Speed; //减速后的速度
        //public readonly float Slope; //斜率
        //道具时间
        public readonly float Dash_Time; //冲锋时间
        public readonly float Double_Time; //双倍时间
        public readonly float Protect_Time; //护盾时间
        public readonly float Super_Man_Time; //无敌时间
        public readonly float Magnet_Time; //磁铁时间
        //人物相关属性
        public readonly List<HumanSkill> Skill_List; //人物自带技能
        public readonly List<Items> Buy_Items_List; //开始前购买的道具
        public readonly float Difference_Y; //人物和摄像机y轴上的初始位置差
        public readonly Animator Human_Ani; //人物动画
        public readonly Transform Main_Camera; //摄像机
        public readonly Transform Human; //人物组件
        public readonly HumanManager HumanManager_Script; //游戏管理脚本实例
        public readonly HumanEvolution Human_Script; //人物脚本实例
        public readonly SkinnedMeshRenderer Human_Mesh; //人物的mesh组件
        public readonly Shader Human_Shader; 
        public readonly float Human_Height; //人物模型的身高
        public readonly float Human_Helf_Height; //人物模型，脚到轴心点的距离
        public readonly Vector3 Human_Start_Position;
        public readonly float Off_Set; //人物与墙壁之前的偏移参数
        public readonly Renderer End_Point;//关卡结束标志点
        public readonly HumanColliderManager HumanCollider;//人物碰撞框
        public readonly Phantom Human_Phantom;//人物幻影
        public readonly GameObject ColliderManager;//人物碰撞框父物体
        /// <summary>
        /// 人物碰撞的默认角度
        /// </summary>
        public readonly Vector3 Normal_Angle;
        //墙体属性
        public readonly float Left_Wall_Inside; //左侧墙体内侧坐标
        public readonly float Right_Wall_Inside; //右侧墙体内侧坐标
        public readonly float Left_Wide_Wall_Inside; //左侧宽墙内侧坐标
        public readonly float Right_Wide_Wall_Inside; //右侧宽墙内侧坐标
        
        #endregion

        //构造，给属性赋值
        public HumanNature(int mark)
        {
            _follow_Spring = new FollowSpring(false);

            //Debug.Log("属性初始化");
            HumanEvolution hero = FindObjectOfType<HumanEvolution>();

            //墙体属性
            HumanManager.WallMark = WallMark.Left; //现在所处墙体的标记
            HumanManager.JumpMark = JumpMark.Normal; //跳跃类型标记

            var left_Wall = MyFind(MyTags.Left_Tag, MyTags.Wall_Layer);
            var right_Wall = MyFind(MyTags.Right_Tag, MyTags.Wall_Layer);
            var left_Wide_Wall = MyFind(MyTags.Left_Tag, MyTags.Wide_Wall_Layer);
            var right_Wide_Wall = MyFind(MyTags.Right_Tag, MyTags.Wide_Wall_Layer);

            Left_Wall_Inside        = left_Wall.GetComponent<MeshRenderer>().bounds.max.x;
            Right_Wall_Inside       = right_Wall.GetComponent<MeshRenderer>().bounds.min.x;
            Left_Wide_Wall_Inside   = left_Wide_Wall.GetComponent<MeshRenderer>().bounds.max.x;
            Right_Wide_Wall_Inside  = right_Wide_Wall.GetComponent<MeshRenderer>().bounds.min.x;
            //速度
            //Slope = hero.Slope; //斜率
            Jump_Speed = hero.Jump_Speed; //跳跃速度
            Run_Speed = hero.Run_Speed; //人物奔跑速度
            if (Jump_Speed == 0 || Run_Speed == 0)
            {
                Debug.Log("速度被赋值了0，自动赋值默认值");
                Jump_Speed = 14.0f * Time.fixedDeltaTime; //跳跃速度
                Run_Speed  = 5.5f * Time.fixedDeltaTime;  //人物奔跑速度
            }
            Slow_Speed = Run_Speed*0.5f; //减速后的速度
            //道具时间
            Dash_Time = hero.Dash_Time; //冲锋时间
            Double_Time = hero.Double_Time; //双倍时间
            Protect_Time = hero.Protect_Time; //护盾时间
            Super_Man_Time = hero.SuperMan_Time; //无敌时间
            Magnet_Time = hero.Magnet_Time; //磁铁时间
            //人物相关属性
            Off_Set = 0.1f;
            Skill_List = hero.InherentSkillList; //人物技能列表
            Buy_Items_List = BuyTheItems.Buy_Items_List; //开局前购买的道具
            HumanManager_Script = FindObjectOfType<HumanManager>();
            Human_Script = hero;
            Human = hero.transform;
            Human_Ani = Human.transform.GetComponent<Animator>();
            Main_Camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
            Difference_Y = Main_Camera.position.y - Human.position.y; //人物和摄像机y轴上的初始位置差
            Human_Mesh = GameObject.FindGameObjectWithTag("Player").GetComponent<SkinnedMeshRenderer>();
            Human_Shader = Human_Mesh.material.shader;
            Human_Height = Human_Mesh.bounds.extents.y;
            Human_Helf_Height = Human.position.x - Human_Mesh.bounds.min.x;
            Vector3 temp = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth*0.5f,
                Camera.main.pixelHeight*0.25f, Human.transform.position.z - Camera.main.transform.position.z));
            Human_Start_Position = new Vector3(Left_Wall_Inside + Human_Height*(0.5f - Off_Set), temp.y,
                left_Wall.transform.GetComponent<MeshRenderer>().bounds.center.z - 0.2f);
            End_Point = GameObject.Find("EndPoint").transform.GetComponent<Renderer>();
            HumanCollider = Human.GetComponentInChildren<HumanColliderManager>();
            Normal_Angle = HumanCollider.transform.localEulerAngles;
            if (Human_Ani == null)
            {
                Debug.Log("未找到动画组件");
            }

            Human_Phantom = Human.GetComponent<Phantom>();
            Human_Phantom.enabled = false;
            ColliderManager = GameObject.FindGameObjectWithTag("ColliderManager");

        }
        //判断墙体组件是否加载成功
        private static GameObject MyFind(string tag, int layer)
        {
            GameObject[] a = GameObject.FindGameObjectsWithTag(tag);
            foreach (var o in a)
            {
                if (o.layer == layer)
                {
                    return o;
                }
            }
            Debug.Log("墙体组件加载失败");
            return null;
        }
    }
}

#endregion

