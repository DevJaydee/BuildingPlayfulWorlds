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
	[SerializeField] private float targetEngagementRange = 1f; // How far the enemy can attack at the target
	[SerializeField] private float targetEngagementSpeed = 1f; // How fast the enemy can attack at the target
	[SerializeField] private float targetDamageAmount = 5f; // How much damage the enemy does.
	[SerializeField] private float moveDestinationSetInterval = 0.1f;   // How often the enemy is told to move to the target position.
	[SerializeField] private float moveSpeed = 2.5f;                    // Movementspeed of the enemy.
	[Space]
	[SerializeField] private NavMeshAgent navMeshAgent = default;   // Reference to the navMeshAgent component.
	[Space]
	[SerializeField] private Animator anim = default;   // Reference to the animator component;
	[Space]
	[SerializeField] private AudioSource source = default;  // Audiosource component.
	[Space]
	[SerializeField] private float minTimeBetweenAudio = 10f;   // Minimum time between audio clips.
	[SerializeField] private float maxTimeBetweenAudio = 30f;   // Macimum time between audio clips.
	[SerializeField] private float timeBetweenAudio = default;  // Time between audio clips
	#endregion

	#region States
	[SerializeField] private StateMachine behaviourSM;
	[SerializeField] private EnemyWalkingState walking;
	[SerializeField] private EnemySearchingState searching;
	[SerializeField] private EnemyAttackingState attacking;
	#endregion

	#region Properties
	public Transform Target { get => target; set => target = value; }

	public float TargetDetectionRange { get => targetDetectionRange; set => targetDetectionRange = value; }
	public float TargetEngagementRange { get => targetEngagementRange; set => targetEngagementRange = value; }
	public float TargetEngagementSpeed { get => targetEngagementSpeed; set => targetEngagementSpeed = value; }
	public float TargetDamageAmount { get => targetDamageAmount; set => targetDamageAmount = value; }
	public float MoveDestinationSetInterval { get => moveDestinationSetInterval; set => moveDestinationSetInterval = value; }

	public EnemyWalkingState Walking { get => walking; set => walking = value; }
	public EnemySearchingState Searching { get => searching; set => searching = value; }
	public EnemyAttackingState Attacking { get => attacking; set => attacking = value; }

	public NavMeshAgent NavMeshAgent { get => navMeshAgent; set => navMeshAgent = value; }

	public float Health { get => health; set => health = value; }
	public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
	#endregion

	#region MonoBehaviour Callbacks
	private void Start()
	{
		target = GameManager.Instance.EnemyTargetObjects[Random.Range(0, GameManager.Instance.EnemyTargetObjects.Length)].transform;

		behaviourSM = new StateMachine();

		searching = new EnemySearchingState(this, behaviourSM);
		walking = new EnemyWalkingState(this, behaviourSM);
		attacking = new EnemyAttackingState(this, behaviourSM);

		timeBetweenAudio = Random.Range(0.5f, 3f);

		navMeshAgent.stoppingDistance = targetEngagementRange;
		navMeshAgent.speed = moveSpeed;

		behaviourSM.Initialize(searching);

		StartCoroutine(PlayWalkAudio());
		StartCoroutine(PlayGrowlAudio());
	}

	private void Update()
	{
		behaviourSM.CurrentState.HandleInput();
		behaviourSM.CurrentState.LogicUpdate();

		if(health <= 0)
			OnDeathEvent();

		if(GameManager.Instance.GameState == GameState.Paused) source.Pause(); else source.UnPause();

		anim.SetFloat("MovementSpeed", (navMeshAgent.velocity).magnitude);
	}

	private void FixedUpdate()
	{
		behaviourSM.CurrentState.PhysicsUpdate();
	}
	#endregion

	#region Voids
	public void OnDeathEvent()
	{
		Instantiate(onDeathParticles, transform.position, Quaternion.identity);
		EnemyManager.Instance.RemoveFromList(gameObject);
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

	private IEnumerator PlayGrowlAudio()
	{
		while(true)
		{
			yield return new WaitForSeconds(timeBetweenAudio);
			AudioMaster.Instance.PlayZombieGrowl(source);
			timeBetweenAudio = Random.Range(minTimeBetweenAudio, maxTimeBetweenAudio);
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
