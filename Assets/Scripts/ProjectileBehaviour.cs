using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
	[SerializeField] private float destroyTime = 10f;       // How much time has to elapse for this projectile to destroy itself.
	[SerializeField] private float movementAmount = 0.01f;    // The speed at which the projectile moves.
	[SerializeField] private float damageAmount = 10f;      // How much damage this projectile does.
	[SerializeField] private LayerMask collisionMask = default;  // Which layers the raycast can check for.
	[SerializeField] private float collisionRayLength = 1.25f;  // The length of the raycast.
	[SerializeField] private Rigidbody rb;  //reference to the Rigidbody component.

	private Ray ray;
	private Vector3 currentPosition;
	private Vector3 currentVelocity;

	Vector3 newPosition = Vector3.zero;
	Vector3 newVelocity = Vector3.zero;


	private void Start()
	{
		Destroy(gameObject, destroyTime);
		rb.AddForce(transform.forward * (movementAmount * 10000) * Time.deltaTime);
		currentVelocity = transform.forward * movementAmount;
	}

	private void FixedUpdate()
	{
		MoveBullet();
	}

	private void CheckBulletHit()
	{
		Vector3 fireDirection = (newPosition - currentPosition).normalized;
		float fireDistance = Vector3.Distance(newPosition, currentPosition);

		if(Physics.Raycast(currentPosition, fireDirection, out RaycastHit hit, collisionMask))
		{
			Debug.Log("My raycast hit: " + hit.collider.name);
			//OnTriggerEnterEvent(hit.collider);
		}

		Debug.DrawRay(currentPosition, fireDirection, Color.red);
	}

	private void MoveBullet()
	{
		//Use an integration method to calculate the new position of the bullet
		float h = Time.fixedDeltaTime;

		//First we need these coordinates to check if we have hit something
		CheckBulletHit();

		currentPosition = newPosition;
		currentVelocity = newVelocity;

		//Add the new position to the bullet
		transform.position = currentPosition;
	}

	private void OnTriggerEnter(Collider other)
	{
		//OnTriggerEnterEvent(other);
	}

	private void OnTriggerEnterEvent(Collider other)
	{
		if(other.GetComponent<Enemy>())
		{
			other.GetComponent<Enemy>().Health -= damageAmount;
		}
		else if(other.GetComponent<PlayerBehaviour>())
		{
			//other.GetComponent<PlayerBehaviour>()
		}
		else if(other.GetComponent<ButtonBehaviour>())
		{
			other.GetComponent<ButtonBehaviour>().Triggerbutton();
		}

		Debug.Log(gameObject.name + "Hit: " + other.gameObject.name);
		Destroy(gameObject);
	}

	private void OnDrawGizmosSelected()
	{
		//Gizmos.color = Color.red;
		//Gizmos.DrawRay(transform.position, transform.forward * collisionRayLength);
	}
}
