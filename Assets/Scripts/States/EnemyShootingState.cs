using RayWenderlich.Unity.StatePatternInUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingState : EnemyGroundedState
{
	public EnemyShootingState(Enemy agent, StateMachine stateMachine) : base(agent, stateMachine)
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

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		agent.transform.LookAt(agent.transform.position);

		// only shoot at the target if it's close enough. Otherwise the Enemy will start to walk again towards the target.
		if(Vector3.Distance(agent.transform.position, agent.Target.transform.position) > agent.TargetEngagementRange)
		{
			stateMachine.ChangeState(agent.Walking);
		}
	}
}
