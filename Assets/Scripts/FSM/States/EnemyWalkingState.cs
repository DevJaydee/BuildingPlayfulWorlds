using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RayWenderlich.Unity.StatePatternInUnity;

public class EnemyWalkingState : EnemyGroundedState
{
	private float movementDestinationSetInterval;
	private NavMeshAgent navMeshAgent;

	public EnemyWalkingState(Enemy agent, StateMachine stateMachine) : base(agent, stateMachine)
	{
	}

	public override void Enter()
	{
		base.Enter();

		Debug.Log("I've started moving towards the player!");
		movementDestinationSetInterval = agent.MoveDestinationSetInterval;
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		if(Vector3.Distance(agent.transform.position, agent.Target.transform.position) < agent.TargetEngagementRange)
		{
			stateMachine.ChangeState(agent.Shooting);
		}
		if(Vector3.Distance(agent.transform.position, agent.Target.transform.position) > agent.TargetDetectionRange)
		{
			stateMachine.ChangeState(agent.Searching);
		}

		agent.NavMeshAgent.SetDestination(agent.Target.transform.position);
	}
}
