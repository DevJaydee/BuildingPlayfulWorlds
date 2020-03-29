using RayWenderlich.Unity.StatePatternInUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyGroundedState
{
	private float attackCounter;

	public EnemyAttackingState(Enemy agent, StateMachine stateMachine) : base(agent, stateMachine)
	{

	}

	public override void Enter()
	{
		base.Enter();
		Debug.Log("I've Started Attacking!");
		attackCounter = agent.TargetEngagementSpeed;
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		Attack();
	}

	private void Attack()
	{
		attackCounter -= Time.deltaTime;
		if(attackCounter <= 0f)
		{
			agent.Target.GetComponent<IDamagable>()?.Damage(agent.TargetDamageAmount);
			attackCounter = agent.TargetEngagementSpeed;
		}
	}
}
