using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public sealed class StateMachine<TLabel>
{
	private class State
	{
		public readonly Action onStart;
		public readonly Action onExecute;
		public readonly Action onStop;
		public readonly TLabel label;

		public State(TLabel label, Action onStart, Action onExecute, Action onStop)
		{
			this.onStart = onStart;
			this.onExecute = onExecute;
			this.onStop = onStop;
			this.label = label;
		}
	}

	private readonly Dictionary<TLabel, State> stateDictionary;
	private State currentState;
    
	public TLabel CurrentState
	{
		get { return currentState.label; }

		set { ChangeState(value); }
	}

    private State beforeTheState;

    public TLabel BeforeTheState
    {
        get { return beforeTheState.label; }
    }


    public StateMachine()
	{
		stateDictionary = new Dictionary<TLabel, State>();
	}

	//注册一个新状态到字典里
	public void AddState(TLabel label,IStateMethod state_Method)
	{
		stateDictionary[label] = new State(label, state_Method.OnEnter, state_Method.OnExecute, state_Method.OnExit);
	}

	//切换状态
	private void ChangeState(TLabel newState)
	{
		if (currentState != null && currentState.onStop != null)
		{
			currentState.onStop();
		}

	    beforeTheState = currentState;
        currentState = stateDictionary[newState];

		if (currentState.onStart != null)
		{
			currentState.onStart();
		}
	}
	//执行状态，每帧执行
	public void Update()
	{
		if (currentState != null && currentState.onExecute != null)
		{
			currentState.onExecute();
		}
	}
}
