using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
	#region Variables
	[SerializeField] private GameObject target = null;                  // Target of the Turret.
	[SerializeField] private LayerMask targetMask = default;            // Target mask. 
	[SerializeField] private float rotationSpeed = default;             // How fast the turret rotates towards the target.
	[SerializeField] private Transform rotationPivot = default;         // Pivot of the turret barel part.
	[SerializeField] private float detectionRange = default;            // Minimum distance before the turrets see the enemies
	[Space]
	[SerializeField] private float gunShootingInterval = default;       // Shooting interval.
	[SerializeField] private float gunDamage = default;                 // The damage it will deal... Duh.
	[SerializeField] private GameObject hitParticles = default;         // Particles that spawn when hitting something.
	[SerializeField] private Transform muzzleTransform = default;       // The Muzzle Transform.
	[SerializeField] private GameObject muzzleFlash = default;         // Muzzleflash object.
	[SerializeField] private AudioSource source = default;              // Reference to the AudioSource component.

	private bool canShoot = false;                                      // Get's set to true when the turret raycast hits the object.

	#endregion
	public float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }
	public float GunShootingInterval { get => gunShootingInterval; set => gunShootingInterval = value; }
	public float GunDamage { get => gunDamage; set => gunDamage = value; }
	public float DetectionRange { get => detectionRange; set => detectionRange = value; }
	#region Properties

	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		StartCoroutine(Shoot());
	}

	private void Update()
	{
		if(GameManager.Instance.GameState == GameState.Playing)
		{
			if(!target)
				GetClosestTarget();
			else
			{
				SmoothRotation();
			}
		}
	}
	#endregion

	#region Voids & Ienumerators
	#region Private
	/// <summary>
	/// Get's the closest target
	/// </summary>
	private void GetClosestTarget()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, targetMask);
		Collider nearestCollider = null;
		float minSqrDistance = Mathf.Infinity;
		for(int i = 0; i < colliders.Length; i++)
		{
			float sqrDistanceToCenter = (transform.position - colliders[i].transform.position).sqrMagnitude;
			if(sqrDistanceToCenter < minSqrDistance)
			{
				minSqrDistance = sqrDistanceToCenter;
				nearestCollider = colliders[i];
				target = nearestCollider.gameObject;
			}
		}
	}

	/// <summary>
	/// Smoothly rotates the turret towards the target
	/// </summary>
	private void SmoothRotation()
	{
		Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

		// Smoothly rotate towards the target point.
		rotationPivot.rotation = Quaternion.Slerp(rotationPivot.rotation, targetRotation, rotationSpeed * Time.deltaTime);
	}

	/// <summary>
	/// Shoot at the target at an interval
	/// </summary>
	/// <returns></returns>
	private IEnumerator Shoot()
	{
		while(true)
		{
			if(GameManager.Instance.GameState == GameState.Playing)
				if(Physics.Raycast(muzzleTransform.position, muzzleTransform.forward, out RaycastHit hit, Mathf.Infinity, targetMask))
				{
					hit.collider.GetComponent<IDamagable>()?.Damage(gunDamage);

					Instantiate(hitParticles, hit.point, Quaternion.identity);
					Instantiate(muzzleFlash, muzzleTransform.position, Quaternion.identity);

					AudioMaster.Instance.PlayWeaponSound(this.source, GunType.Rifle);

					Debug.Log("I shot at something");
					yield return new WaitForSeconds(gunShootingInterval);
				}
			yield return null;
		}
	}
	#endregion

	#region Public

	#endregion
	#endregion

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(muzzleTransform.position, muzzleTransform.forward * 1000);

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, detectionRange);
	}
}
