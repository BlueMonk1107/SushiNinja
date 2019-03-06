using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class StaticParameter {
    /// <summary>
    /// 人物动画参数名
    /// </summary>
    public static readonly string Ani_Key_HumanState="AniState";
    /// <summary>
    /// 破碎动画参数名
    /// </summary>
    public static readonly string Ani_Key_Broke = "Broke";

    /// <summary>
    /// 动画里默认状态的动画参数名
    /// </summary>
    public static readonly string Ani_Key_Idle = "Idle";
    /// <summary>
    /// 人物猜弹簧动画开关
    /// </summary>
    public static readonly string Ani_Key_OnOff = "OnOff";

    #region 音频路径

    //音效

    //系统音效
    public static readonly string s_Stage_BGM = "stage_bgm";	//游戏BGM
	public static readonly string s_UI_BGM = "ui_bgm";		    //UI BGM
	public static readonly string s_Countdown = "Countdown";    //游戏开始倒计时
	public static readonly string s_Cancel = "Cancel";		    //取消
	public static readonly string s_Count = "Count";	        //系统计数
	public static readonly string s_Buy = "buy";	            //购买道具
	public static readonly string s_Levelup = "levelup";        //角色升级
    public static readonly string s_Rankup = "rankup";          //技能条上的升级字体声音
    public static readonly string s_New_record = "New_record";	//新纪录达成
	public static readonly string s_No = "no";		            //不可执行
	public static readonly string s_UI_OK = "ok";		        //确定
	public static readonly string s_UI_Pause = "pause";	        //游戏暂停
    public static readonly string s_Star = "star";              //星星音效
    //行为音效
    public static readonly string s_Damage = "damage";      //受伤判定
    public static readonly string s_Down = "down";		        //角色落地
	public static readonly string s_Jump = "jump";		        //角色起跳
	public static readonly string s_Run = "run";                //角色移动


    public static readonly string s_Tips = "tips";		                //教学弹出框出现时播放
    public static readonly string s_Score = "Score";		            //吃手里剑时播放
    public static readonly string s_Shoot = "Shoot";		            //敌人开枪时播放
    public static readonly string s_Attack = "attack";                  //跳起攻击命中敌人时播放
    public static readonly string s_Sword_Death = "death";		            //碰上刀片死亡时播放

    public static readonly string s_GuiYuZi_Dead = "ikura_dead";		//鲑鱼子寿司普通死亡（撞墙，被枪击中）
    public static readonly string s_GuiYuZi_Skill = "ikura_skill";		//鲑鱼子寿司使用冲刺和无敌时播放
    public static readonly string s_GuiYuZi_Win = "ikura_win";		    //鲑鱼子寿司到达终点时播放
    public static readonly string s_GuiYuZi_Select = "ikura_select";	//鲑鱼子寿司选中时播放（菜单界面）

    public static readonly string s_CiShen_Dead = "maguro_dead";		//赤身寿司普通死亡（撞墙，被枪击中）
    public static readonly string s_CiShen_Skill = "maguro_skill";		//赤身寿司使用冲刺和无敌时播放
    public static readonly string s_CiShen_Win = "maguro_win";		    //赤身寿司到达终点时播放
    public static readonly string s_CiShen_Select = "maguro_select";    //赤身寿司选中时播放（菜单界面）

    public static readonly string s_YuZi_Dead = "tamago_dead";		  //玉子寿司普通死亡（撞墙，被枪击中）
    public static readonly string s_YuZi_Skill = "tamago_skill";	  //玉子寿司使用冲刺和无敌时播放
    public static readonly string s_YuZi_Win = "tamago_win";          //玉子寿司到达终点时播放
    public static readonly string s_YuZi_Select = "tamago_select";    //玉子寿司选中时播放（菜单界面）

    //道具音效
    public static readonly string s_Atkup = "atkup";                        //无敌发动
    public static readonly string s_Dash = "dash";		                //冲刺道具发动
    public static readonly string s_Double = "double";		            //双倍道具发动
	public static readonly string s_Magent = "magent";		            //磁铁道具发动
	public static readonly string s_Sheld = "sheld";	                //护盾道具发动
	public static readonly string s_Sheld_Break = "sheld_break";	    //护盾破坏
	public static readonly string s_Obstacle_Break = "obstacle_break";  //障碍物破坏
	public static readonly string s_Spring = "spring";		            //弹簧触发
    public static readonly string s_Bomb = "Bomb";                      //导弹爆炸

    //音量

    //系统音量
    public static readonly float s_Stage_BGM_Volume = 0.2f;	//游戏BGM音量
	public static readonly float s_UI_BGM_Volume = 0.5f;	//UI BGM音量
	public static readonly float s_Countdown_Volume = 1f;   //游戏开始倒计时音量
	public static readonly float s_Cancel_Volume = 1f;		//取消音量
	public static readonly float s_Count_Volume = 1f;	    //系统计数音量
	public static readonly float s_Buy_Volume = 1f;	        //购买道具音量
	public static readonly float s_Levelup_Volume = 1f;	    //角色升级音量
    public static readonly float s_Rankup_Volume = 1;       //技能条上的升级字体声音
    public static readonly float s_New_record_Volume = 1f;	//新纪录达成音量
	public static readonly float s_No_Volume = 1f;		    //不可执行音量
	public static readonly float s_UI_OK_Volume = 1f;		//确定音量 
	public static readonly float s_UI_Pause_Volume = 1f;    //游戏暂停音量
    public static readonly float s_Star_Volume = 1f;        //星星音效音量

    //行为音量
    public static readonly float s_Damage_Volume = 1f;             //受伤判定
    public static readonly float s_Down_Volume = 1f;		//角色落地音量
	public static readonly float s_Jump_Volume = 1f;		//角色起跳音量
	public static readonly float s_Run_Volume = 1f;         //角色移动音量

    //道具音量
    public static readonly float s_Atkup_Volume = 1f;           //无敌发动
    public static readonly float s_Dash_Volume = 1f;            //冲刺道具发动
    public static readonly float s_Double_Volume = 1f;		    //双倍道具发动音量
	public static readonly float s_Magent_Volume = 1f;		    //磁铁道具发动音量
	public static readonly float s_Sheld_Volume = 1f;	        //护盾道具发动音量
	public static readonly float s_Sheld_Break_Volume = 1f; 	//护盾破坏音量
	public static readonly float s_Obstacle_Break_Volume = 1f;	//障碍物破坏音量
	public static readonly float s_Spring_Volume = 1f;          //弹簧触发音量
    public static readonly float s_Bomb_Volume = 1f;            //导弹爆炸音量

    public static readonly float s_Attack_Volume = 1f;          //跳起攻击命中敌人时播放
    public static readonly float s_Sword_Death_Volume = 1f;		    //碰上刀片死亡时播放
    public static readonly float s_Score_Volume = 0.1f;		    //吃手里剑时播放
    public static readonly float s_Shoot_Volume = 1f;		    //敌人开枪时播放
    public static readonly float s_Tips_Volume = 1f;		    //教学弹出框出现时播放

    public static readonly float s_GuiYuZi_Dead_Volume = 1.5f;	//鲑鱼子寿司普通死亡（撞墙，被枪击中）
    public static readonly float s_GuiYuZi_Skill_Volume = 1.5f;	//鲑鱼子寿司使用冲刺和无敌时播放
    public static readonly float s_GuiYuZi_Win_Volume = 1.5f;		//鲑鱼子寿司到达终点时播放
    public static readonly float s_GuiYuZi_Select_Volume = 1.5f;	//鲑鱼子寿司选中时播放（菜单界面）

    public static readonly float s_CiShen_Dead_Volume = 1f;		//赤身寿司普通死亡（撞墙，被枪击中）
    public static readonly float s_CiShen_Skill_Volume = 1f;    //赤身寿司使用冲刺和无敌时播放
    public static readonly float s_CiShen_Win_Volume = 1f;		//赤身寿司到达终点时播放
    public static readonly float s_CiShen_Select_Volume = 1f;   //赤身寿司选中时播放（菜单界面）
    
    public static readonly float s_YuZi_Dead_Volume = 1f; 	    //玉子寿司普通死亡（撞墙，被枪击中）
    public static readonly float s_YuZi_Skill_Volume = 1f;	    //玉子寿司使用冲刺和无敌时播放
    public static readonly float s_YuZi_Win_Volume = 1f;		//玉子寿司到达终点时播放
    public static readonly float s_YuZi_Select_Volume = 1f;     //玉子寿司选中时播放（菜单界面）
    
    #endregion

    #region Resources文件夹名称及资源名称

    public static readonly string s_Folder_GameEffect = "GameEffect";
    public static readonly string s_Folder_Materials  = "Materials";
    public static readonly string s_Folder_Other      = "Other";

    //GameEffect
    public static readonly string s_Prefab_Effect = "effect";
    public static readonly string s_Prefab_Juice = "juice";
    public static readonly string s_Prefab_Step = "Step";
    public static readonly string s_Prefab_SwordLight = "SwordLight";
    public static readonly string s_Prefab_Line = "line";
    public static readonly string s_Prefab_Fire = "Fire";
    //Materials
    public static readonly string s_Prefab_Golden = "Golden";
    //Other
    public static readonly string s_Prefab_Text = "Text";

    #endregion

    /// <summary>
    /// 资源加载方法,加载预制体
    /// </summary>
    /// <param name="folder_Name"></param>
    /// <param name="prefab_Name"></param>
    /// <returns></returns>
    public static Object LoadObject(string folder_Name,string prefab_Name)
    {
        Object ob = Resources.Load(folder_Name + "/" + prefab_Name);
       
        if (ob == null)
        {
            Debug.Log(prefab_Name + "加载失败");
        }
        return ob;
    }

    //判断组件是否为空,提示赋值失败
    public static void JudgeNull(string ob_Name, Object ob)
    {
        if (!ob)
        {
            Debug.Log(ob_Name + "赋值失败");
        }
    }
    /// <summary>
    /// 返回两个物体在y轴上的距离
    /// </summary>
    /// <param name="sth"></param>
    /// <param name="human"></param>
    /// <returns></returns>
     public static float ComparePosition(Transform sth, Transform human)
    {
        return (sth.position.y - human.position.y);

    }
    /// <summary>
    /// 道具的球形特效
    /// </summary>
    /// <param name="item"></param>
    public static GameObject ItemEffect(Transform item)
    {
        GameObject effect_Mobel = (GameObject)LoadObject("Other", "effect");
        GameObject copy = Object.Instantiate(effect_Mobel);
        copy.transform.position = item.position;
        copy.transform.localScale = Vector3.one * 1.3f;
        copy.transform.eulerAngles = Vector3.zero;
        return copy;
    }
    /// <summary>
    /// 删除组件后清空引用
    /// </summary>
    /// <param name="sth"></param>
    public static void ClearReference(ref GameObject sth)
    {
        sth = null;
        Resources.UnloadUnusedAssets();
    }

    public static void DestroyOrDespawn(Transform transform)
    {
        if (SceneManager.GetActiveScene().name.Contains("Endless"))
        {
            LoadScene.Instance.EndlessPool.Inactive(transform);
        }
        else
        {
            Object.Destroy(transform.gameObject);
        }
    }
}
