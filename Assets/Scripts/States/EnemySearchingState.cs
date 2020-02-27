using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayWenderlich.Unity.StatePatternInUnity;

public class EnemySearchingState : EnemyGroundedState
{
	public EnemySearchingState(Enemy agent, StateMachine stateMachine) : base(agent, stateMachine)
	{

	}

	public override void Enter()
	{
		base.Enter();
		Debug.Log("I've Started Searching For The Player!");
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		if(Vector3.Distance(agent.Target.transform.position, agent.transform.position) <= agent.TargetDetectionRange)
		{
			Debug.Log("Player found!");
			stateMachine.ChangeState(agent.Walking);
		}
	}
}
