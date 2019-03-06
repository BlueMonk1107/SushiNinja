using UnityEngine;
using System.Collections;

public class BehaviourData {

    private StateMachine<HumanState> _state_Machines; //人物状态状态机

    public StateMachine<HumanState> State_Machines
    {
        get
        {
            if (_state_Machines == null)
            {
                _state_Machines = new StateMachine<HumanState>();
            }
            return _state_Machines;
        }
    }
    public HumanState CurrentState
    {
        get { return State_Machines.CurrentState;  }
        set { State_Machines.CurrentState = value; }
    }

    public HumanState BeforeTheState
    {
        get { return State_Machines.BeforeTheState; }
    }


    public BehaviourData()
    {
        InitializeStateMachines(State_Machines, HumanManager.Nature);
    }

    void InitializeStateMachines(StateMachine<HumanState> state_Machines, GameResource.HumanNature nature)
    {
        state_Machines.AddState(HumanState.Run, new Run(nature));
        state_Machines.AddState(HumanState.Jump, new Jump(nature));
        state_Machines.AddState(HumanState.Slowdown, new Slowdown(nature));
        state_Machines.AddState(HumanState.Revise, new Revise(nature));
        state_Machines.AddState(HumanState.Stop, new Stop(nature));
        state_Machines.AddState(HumanState.Dead, new Dead(nature));
    }

    public void Update()
    {
        State_Machines.Update();
    }
    //释放状态机对象，清理内存
    public void Clear()
    {
        _state_Machines = null;
    }
}
