using System;
using UnityEngine;


public class MyCamera : MonoBehaviour
{
    GameResource.HumanNature _nature;
    float _move_Speed;
    float _y;
    Renderer _end_Point;
    SkinnedMeshRenderer _human_Render;
    bool _success_Bool;
    public static bool GameEnd { get; set; }

    public static float _jump_speed;
    // Use this for initialization
    void Start()
    {
        _nature = HumanManager.Nature;
        _move_Speed = _nature.Run_Speed;
        if (_move_Speed <= 0)
        {
            Debug.Log("移动速度不能为0或为负！");
            return;
        }

        _end_Point = _nature.End_Point;
        _human_Render = GameObject.FindGameObjectWithTag("Player").GetComponent<SkinnedMeshRenderer>();
        _success_Bool = true;
        GameEnd = false;
    }

    public void Update()
    {
        if (MyKeys.Pause_Game)
            return;
        if (ItemColliction.DeadDash.IsRun())
        {
            transform.Translate(0, _move_Speed * 1.5f, 0, Space.World);
        }
        if (_nature.HumanManager_Script.CurrentState == HumanState.Dead)
            return;

        if (_end_Point.isVisible)
        {
            if (_nature.Human_Script.transform.position.y >= _end_Point.transform.position.y && _success_Bool)
            {
                GameUIManager method = Transform.FindObjectOfType<GameUIManager>();
                method.TheClicked(GameUI.GameUI_Pass);
                MyKeys.Pause_Game = true;
                _success_Bool = false;
            }
            GameEnd = true;
            return;
        }
        if (!ItemColliction.DeadDash.IsRun())
        {
            if (ItemColliction.Dash.IsRun() || ItemColliction.StartDash.IsRun() || ItemColliction.SuperMan.WhetherSpeedUp())
            {
                transform.Translate(0, _move_Speed * 1.5f, 0, Space.World);
            }
            else
            {
                transform.Translate(0, _move_Speed, 0, Space.World);
            }
        }
        
    }
}
