using RayWenderlich.Unity.StatePatternInUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable
{
	#region Variables
	[SerializeField] private float health = 100;    // The health of this enemy.
	[SerializeField] private ParticleSystem onDeathParticles = default;   // The particles that spawn after an enemy dies.
	[Space]
	[SerializeField] private Transform target = default;    // The target for this enemy. The enemy will try to destroy the target.
	[SerializeField] private float targetDetectionRange = 35f;  // How far the enemy can "sense" the target. This works through walls atm
	[SerializeField] private float targetEngagementRange = 15f; // How far the enemy can shoot at the target
	[SerializeField] private float moveDestinationSetInterval = 0.1f;   // How often the enemy is told to move to the target position.
	[Space]
	[SerializeField] private NavMeshAgent navMeshAgent = default;   // Reference to the navMeshAgent component.
	[Space]
	[SerializeField] private Animator anim = default;   // Reference to the animator component;
	[Space]
	[SerializeField] private AudioSource source = default;  // Audiosource component.
	#endregion

	#region States
	[SerializeField] private StateMachine behaviourSM;
	[SerializeField] private EnemyWalkingState walking;
	[SerializeField] private EnemySearchingState searching;
	[SerializeField] private EnemyShootingState shooting;
	#endregion

	#region Getters & Setters
	public Transform Target { get => target; set => target = value; }

	public float TargetDetectionRange { get => targetDetectionRange; set => targetDetectionRange = value; }
	public float TargetEngagementRange { get => targetEngagementRange; set => targetEngagementRange = value; }
	public float MoveDestinationSetInterval { get => moveDestinationSetInterval; set => moveDestinationSetInterval = value; }

	public EnemyWalkingState Walking { get => walking; set => walking = value; }
	public EnemySearchingState Searching { get => searching; set => searching = value; }
	public EnemyShootingState Shooting { get => shooting; set => shooting = value; }

	public NavMeshAgent NavMeshAgent { get => navMeshAgent; set => navMeshAgent = value; }

	public float Health { get => health; set => health = value; }
	#endregion

	#region MonoBehaviour Callbacks
	private void Start()
	{
		target = GameManager.Instance.PlayerObject.transform;

		behaviourSM = new StateMachine();

		searching = new EnemySearchingState(this, behaviourSM);
		walking = new EnemyWalkingState(this, behaviourSM);
		shooting = new EnemyShootingState(this, behaviourSM);

		navMeshAgent.stoppingDistance = targetEngagementRange;

		behaviourSM.Initialize(searching);

		StartCoroutine(PlayWalkAudio());
	}

	private void Update()
	{
		behaviourSM.CurrentState.HandleInput();
		behaviourSM.CurrentState.LogicUpdate();

		if(health <= 0)
			OnDeathEvent();

		anim.SetFloat("MovementSpeed", (navMeshAgent.velocity).magnitude);
	}

	private void FixedUpdate()
	{
		behaviourSM.CurrentState.PhysicsUpdate();
	}
	#endregion

	#region Cool Functions
	public void OnDeathEvent()
	{
		Instantiate(onDeathParticles, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
	public void Damage(float damageAmount)
	{
		health -= damageAmount;
	}

	private IEnumerator PlayWalkAudio()
	{
		while(true)
		{
			if(behaviourSM.CurrentState == walking)
			{
				AudioMaster.Instance.PlayWalkSound(source);
				yield return new WaitForSeconds(0.5f);
			}
			else
			{
				yield return null;
			}
		}
	}
	#endregion

	#region Debugging Stuff
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, targetDetectionRange);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, targetEngagementRange);
	}
	#endregion
}
