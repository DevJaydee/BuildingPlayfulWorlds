using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayWenderlich.Unity.StatePatternInUnity;

public class EnemyGroundedState : State
{
	public EnemyGroundedState(Enemy agent, StateMachine stateMachine) : base(agent, stateMachine)
	{

	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		base.Exit();
	}
}
