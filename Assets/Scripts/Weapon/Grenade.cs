using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grenade : MonoBehaviour
{
	#region Variables
	[SerializeField] private float throwForce = 10f;                    // How hard/far the grenade will be thrown.
	[SerializeField] private float explosionTimer = 3f;                 // How long before the grenade explodes.
	[SerializeField] private float explosionRadius = 7.5f;              // How far the damage will reach.
	[SerializeField] private float explosionForce = 1000f;              // How far the damage will reach.
	[SerializeField] private float explosionDamage = 100f;              // How far the damage will reach.
	[SerializeField] private GameObject explosionParticles = null;      // Nice particles to show how far/big the explosion radius is.
	#endregion

	#region Getters and Setters
	public float ThrowForce { get => throwForce; set => throwForce = value; }
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		StartCoroutine(StartExplosionTimer());
	}
	#endregion

	#region Private Voids & IEnumerators
	/// <summary>
	/// Starts the explosion timer
	/// </summary>
	/// <returns></returns>
	private IEnumerator StartExplosionTimer()
	{
		yield return new WaitForSeconds(explosionTimer);
		Explode();
	}

	/// <summary>
	/// Explode the grenade after sometime.
	/// </summary>
	private void Explode()
	{
		Instantiate(explosionParticles, transform.position, Quaternion.identity);

		Collider[] collisionsInRange = Physics.OverlapSphere(transform.position, explosionRadius);
		foreach(Collider nearbyObject in collisionsInRange)
		{
			nearbyObject.GetComponent<IDamagable>()?.Damage(explosionDamage);
			Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
			if(rb)
			{
				// Add some physics. After I implement ragdolls :)
				rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
			}
		}

		Destroy(gameObject);
	}
	#endregion
}
