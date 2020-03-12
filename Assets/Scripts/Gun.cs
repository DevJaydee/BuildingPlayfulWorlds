using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
	private enum GunState { Collectable, Equiped, Reloading };
	private enum GunFireMode { Semi, FullAuto };
	[SerializeField] private GunState gunState = GunState.Collectable;  // Dictates if the gun is collectable (In the world ready to be picked up) or already equiped by the player.
	[SerializeField] private GunFireMode fireMode = GunFireMode.Semi;   // Dictates if the gun can shoot fullauto or not.
	[SerializeField] private BoxCollider interactionCollider = default; // This is the collider that checks for trigger events. This collider will be deleted after interaction.
	[SerializeField] private GunSwitching gunSwitching = default;       // Reference to the GunSwitching script.
	[Space]
	[SerializeField] private float damage = 10f;    // How much damage the gun does.
	[SerializeField] private float range = 100f;    // How far the gun can shoot.
	[SerializeField] private float fireRate = 1f;
	[SerializeField] private LayerMask hitMask = default;     // What the gun can hit.
	[SerializeField] private GameObject hitParticles = default;  // These are the particles that will showup on the surface that is hit.
	[Space]
	[SerializeField] private TMPro.TextMeshProUGUI currentAmmoText = default;   // TextMesh on the gun that indicates how much ammo is left.
	[SerializeField] private int currentAmmo = 0;   // How much ammo is currently in the Gun (Magazine).
	[SerializeField] private int maxAmmo = 30;  // How much ammo the gun could hold.
	[SerializeField] private float reloadTime = 1;  // How long it will take to reload the gun.
	[Space]
	[SerializeField] private Camera fpsCam = default;   // A refernce to the Camera. The raycast will be shot from here.
	[Space]
	[SerializeField] private Vector3 rotationVector = default; // Which direction the Weapon rotates.
	[SerializeField] private float rotationSpeed = default; // How fast the Weapon rotates when it is in Collectable state.

	private Vector3 pos = Vector3.zero; // Contains the pos of the player. Is only used for the Sinus movement.
	private float fireRateCounter = 0;  // Contains the current time between shots.

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

			if(Input.GetKey(KeyCode.R))
				StartCoroutine(Reload());

			// Shoot when the player pressed their Left Mousebutton Down
			if(Input.GetButton("Fire1") && fireMode == GunFireMode.FullAuto && Time.time >= fireRateCounter)
			{

				fireRateCounter = Time.time + 1f / fireRate;
				Shoot();

			}
			else if(Input.GetButtonDown("Fire1") && fireMode == GunFireMode.Semi)
			{
				Shoot();
			}

			currentAmmoText.text = currentAmmo + " / " + maxAmmo;
		}
		else if(gunState == GunState.Collectable)
		{   // Rotate the weapon
			transform.Rotate(rotationVector * rotationSpeed * Time.deltaTime);
			currentAmmoText.enabled = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			PickUp(other);
			gunSwitching = other.GetComponentInChildren<GunSwitching>();
			gunSwitching.SelectWeapon();
		}
	}

	/// <summary>
	/// Shoots a raycast and depending on the firemode it will shoot continious. Or when the player click the button again.
	/// </summary>
	public void Shoot()
	{
		RaycastHit hit;
		if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, hitMask))
		{
			Debug.Log("Hit: " + hit.collider.name);
			OnHitEvent(hit);
		}

		currentAmmo--;
		if(currentAmmo <= 0)
			StartCoroutine(Reload());
	}

	private IEnumerator Reload()
	{
		gunState = GunState.Reloading;
		currentAmmoText.text = "Reloading...";
		yield return new WaitForSeconds(reloadTime);
		currentAmmo = maxAmmo;
		gunState = GunState.Equiped;
	}

	/// <summary>
	/// This function handles all the stuff that happens depending on what the player hit.
	/// </summary>
	private void OnHitEvent(RaycastHit hit)
	{
		Instantiate(hitParticles, hit.point, Quaternion.LookRotation(hit.normal));

		hit.collider.GetComponent<IDamagable>()?.Damage(damage);



		//else if(hit.collider.CompareTag("Collidable"))
		//{ }
	}

	/// <summary>
	/// When the gunstate is Collectable, the player can pick the weapon up.
	/// When the player interacts with the weapon, it get's put onto the player weaponTransform.
	/// </summary>
	/// <param name="other"></param>
	private void PickUp(Collider other)
	{
		transform.SetParent(other.GetComponent<Player>().WeaponTransform);
		transform.position = transform.parent.position;
		transform.rotation = transform.parent.rotation;
		gunState = GunState.Equiped;
		currentAmmoText.enabled = true;
		Destroy(interactionCollider);
	}
}
