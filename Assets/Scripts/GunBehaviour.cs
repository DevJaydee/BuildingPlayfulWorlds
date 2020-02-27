using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
	private enum GunState { Collectable, Equiped };
	[SerializeField] private GunState gunState = GunState.Collectable;  // Dictates if the gun is collectable (In the world ready to be picked up) or already equiped by the player.
	[SerializeField] private BoxCollider interactionCollider = default; // This is the collider that checks for trigger events. This collider will be deleted after interaction.
	[Space]
	[SerializeField] private float damage = 10f;    // How much damage the gun does.
	[SerializeField] private float range = 100f;    // How far the gun can shoot.
	[SerializeField] private LayerMask hitMask;     // What the gun can hit.
	[SerializeField] private GameObject hitParticles;  // These are the particles that will showup on the surface that is hit.
	[Space]
	[SerializeField] private Camera fpsCam = default;   // A refernce to the Camera. The raycast will be shot from here.
	[Space]
	[SerializeField] private Vector3 rotationVector = default; // Which direction the Weapon rotates.
	[SerializeField] private float rotationSpeed = default; // How fast the Weapon rotates when it is in Collectable state.

	private Vector3 pos = Vector3.zero; // Contains the pos of the player. Is only used for the Sinus movement.

	private void Awake()
	{
		fpsCam = Camera.main;
		interactionCollider = GetComponent<BoxCollider>();
		interactionCollider.isTrigger = true;
		pos = transform.position;
	}

	private void Update()
	{
		if(gunState == GunState.Equiped)
		{
			transform.rotation = Camera.main.transform.rotation;
			// Shoot when the player pressed their Left Mousebutton Down
			if(Input.GetMouseButtonDown(0))
			{
				Shoot();
			}
		}
		else
		{
			// Rotate the weapon to indicate it can be picked up
		}
		{
			transform.Rotate(rotationVector * rotationSpeed * Time.deltaTime);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			PickUp(other);
		}
	}

	/// <summary>
	/// Shoots a raycast and then does some stuff.
	/// </summary>
	public void Shoot()
	{
		RaycastHit hit;
		if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, hitMask))
		{
			Debug.Log("Hit: " + hit.collider.name);
			OnHitEvent(hit);
		}

	}

	/// <summary>
	/// This function handles all the stuff that happens depending on what the player hit.
	/// </summary>
	private void OnHitEvent(RaycastHit hit)
	{
		Instantiate(hitParticles, hit.point, Quaternion.LookRotation(hit.normal));

		if(hit.collider.CompareTag("Enemy"))
		{
			hit.collider.GetComponent<Enemy>().Health -= damage;
		}
		else if(hit.collider.CompareTag("Collidable"))
		{ }
	}

	/// <summary>
	/// When the gunstate is Collectable, the player can pick the weapon up.
	/// When the player interacts with the weapon, it get's put onto the player weaponTransform.
	/// </summary>
	/// <param name="other"></param>
	private void PickUp(Collider other)
	{
		transform.SetParent(other.GetComponent<PlayerBehaviour>().WeaponTransform);
		transform.position = transform.parent.position;
		transform.rotation = transform.parent.rotation;
		gunState = GunState.Equiped;
		Destroy(interactionCollider);
	}
}
