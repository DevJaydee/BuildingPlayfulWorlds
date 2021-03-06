﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
	Idle,
	Walking
}

public class Player : MonoBehaviour
{
	#region Variables
	[SerializeField] private PlayerState state = PlayerState.Idle;
	[Space]
	[SerializeField] private string horizontalInputName = default;  // The Unity Input string for Horizontal Axis.
	[SerializeField] private string verticalInputName = default;    // The Unity Input string for Vertical Axis.

	private float movementSpeed = default;  // The final movement speed of the player
	[SerializeField] private float walkSpeed = default; // The walkspeed of the player.
	[SerializeField] private float runSpeed = default;  // The runspeed of the player.
	[SerializeField] private float runBuildUpSpeed = default;   // The amount of time it takes for the player to get to full running speed.
	[SerializeField] private KeyCode runKey = default;  // The key which the player has to press to start running

	[SerializeField] private float slopeForce = default;    // How much force is applied on the Character Controller to stick the player to the ground.
	[SerializeField] private float slopeForceRayLength = default;   // How far down we check for the ground.
	[Space]
	[SerializeField] private bool isJumping = false;    // Get's triggered when the player jumps
	[SerializeField] private AnimationCurve jumpfallOff = default;  // The Jump fall off of the jump event. Allows for a nice custom curve.
	[SerializeField] private float jumpMultiplier = default;    // How much upwards force is applied when pressing the jump button
	[Space]
	[SerializeField] private KeyCode jumpKey = default; // The key which the player has to press to jump.
	[Space]
	[SerializeField] private Transform weaponTransform = default;   // The transform for the weapon.
	[Space]
	[SerializeField] private GameObject grenadePrefab = default;    // Prefab of the grenade
	[Space]
	[SerializeField] private AudioSource source = default;      // AudioSource Component.
	private CharacterController charController = default;   // The Character controller variable.

	[Space]
	[SerializeField] private float staminaDrainSpeed = 20f; // How much stamina is drained each second when the run key is pressed.
	[SerializeField] private bool canRun = true;
	[SerializeField] private float staminaRegenTimeout = 2.5f;  // How long the player has to wait before he/she can run again/

	[Header("Scriptable Objects")]
	[SerializeField] private ScriptableFloat grenadeCount = default;
	[SerializeField] private ScriptableFloat health = default;
	[SerializeField] private ScriptableFloat stamina = default;
	#endregion

	#region Getters and Setters
	public Transform WeaponTransform { get => weaponTransform; set => weaponTransform = value; }
	#endregion

	#region Monobehaviour Callbacks
	/// <summary>
	/// Get the Character Controller
	/// </summary>
	private void Awake()
	{
		charController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		PlayerMovement();

		if(Input.GetKeyDown(KeyCode.G))
			ThrowGrenade();

		if(GameManager.Instance.GameState == GameState.Paused) source.Pause(); else source.UnPause();

		if(Input.GetKeyDown(PlayerBase.Instance.TurretUpgradeKey))
			PlayerBase.Instance.UpgradeTurrets();
	}

	private void Start()
	{
		StartCoroutine(PlayWalkAudio());
	}
	#endregion

	#region Moving
	/// <summary>
	/// Get's the player input and makes the Character move.
	/// </summary>
	private void PlayerMovement()
	{
		float horInput = Input.GetAxisRaw(horizontalInputName);
		float verInput = Input.GetAxisRaw(verticalInputName);

		Vector3 forwardMovement = transform.forward * verInput;
		Vector3 rightMovement = transform.right * horInput;

		charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1f) * movementSpeed);

		if((verInput != 0f || horInput != 0f) && OnSlope())
			charController.Move(Vector3.down * charController.height / 2f * slopeForce * Time.deltaTime);

		if(charController.velocity == Vector3.zero)
			state = PlayerState.Idle;
		else
			state = PlayerState.Walking;

		if(stamina.Value <= 0f && canRun)
			StartCoroutine(RunCooldown());

		SetMovementSpeed();
		JumpInput();
	}

	/// <summary>
	/// Set's the movement speed depending on if the player is walking or pressing the run button.
	/// </summary>
	private void SetMovementSpeed()
	{
		if(Input.GetKey(runKey) && canRun)
		{
			movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUpSpeed);
			DrainStamina();
		}
		else
		{
			movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUpSpeed);
			FillStamina();
		}
	}

	/// <summary>
	/// Plays a walking sound when... walking. duh
	/// </summary>
	/// <returns></returns>
	private IEnumerator PlayWalkAudio()
	{
		while(true)
		{
			if(state == PlayerState.Walking)
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

	/// <summary>
	/// Slowly drain stamina when running
	/// </summary>
	private void DrainStamina()
	{
		stamina.Value -= staminaDrainSpeed * Time.deltaTime;
		stamina.Value = Mathf.Clamp(stamina.Value, 0, stamina.StartingValue);
	}

	/// <summary>
	/// Slowly Fills the Stamina when not running
	/// </summary>
	private void FillStamina()
	{
		stamina.Value += staminaDrainSpeed * Time.deltaTime;
		stamina.Value = Mathf.Clamp(stamina.Value, 0, stamina.StartingValue);
	}

	/// <summary>
	/// Starts the runcooldown when running for too long.
	/// </summary>
	/// <returns></returns>
	private IEnumerator RunCooldown()
	{
		canRun = false;
		yield return new WaitForSeconds(staminaRegenTimeout);
		canRun = true;
	}
	#endregion

	#region Jumping
	/// <summary>
	/// Checks for user input to jump. Then triggers said jump event.
	/// </summary>
	private void JumpInput()
	{
		if(Input.GetKeyDown(jumpKey) && !isJumping)
		{
			isJumping = true;
			StartCoroutine(JumpEvent());
		}
	}

	/// <summary>
	/// This makes the Character Controller jump and follow a nice jump falloff curve.
	/// </summary>
	/// <returns></returns>
	private IEnumerator JumpEvent()
	{
		charController.slopeLimit = 90f;
		float timeInAir = 0f;
		do
		{
			float jumpForce = jumpfallOff.Evaluate(timeInAir);
			charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
			timeInAir += Time.deltaTime;
			yield return null;
		} while(!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);
		charController.slopeLimit = 45f;
		isJumping = false;
	}
	#endregion

	#region Charactercontroller Physics
	/// <summary>
	/// Checks if the players is on a slope.
	/// </summary>
	/// <returns></returns>
	private bool OnSlope()
	{
		if(isJumping)
			return false;

		RaycastHit hit;
		if(Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2f * slopeForceRayLength))
			if(hit.normal != Vector3.up)
				return true;
		return false;
	}
	#endregion

	#region Grenade Throwing
	/// <summary>
	/// throws the grenade on input.
	/// </summary>
	private void ThrowGrenade()
	{
		if(grenadeCount.Value > 0)
		{
			GameObject newGrenadeGO = Instantiate(grenadePrefab, Camera.main.transform.position, Camera.main.transform.rotation);
			Grenade grenadeComp = newGrenadeGO.GetComponent<Grenade>();
			newGrenadeGO.GetComponent<Rigidbody>().AddForce(newGrenadeGO.transform.forward * grenadeComp.ThrowForce, ForceMode.VelocityChange);

			grenadeCount.Value--;
		}
	}
	#endregion
}
