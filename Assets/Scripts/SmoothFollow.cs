using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
	[SerializeField] private Transform target = default;    // What to follow
	[SerializeField] private float smoothingSpeed = 10;     // How much smoothing when moving towards the target transform.
	[SerializeField] private Vector3 velocity = Vector3.zero;   // The velocity of this object while moving.
	[SerializeField] private Vector3 offset = default;    // The default offset from the target component.

	private Vector3 desiredPos;
	private Vector3 smoothedPos;

	private void Update()
	{
	}

	private void FixedUpdate()
	{
		desiredPos = new Vector3(target.position.x + offset.x, target.position.y + offset.y, target.position.z + offset.z);
		smoothedPos = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothingSpeed * Time.deltaTime);
		transform.position = smoothedPos;
	}
}
