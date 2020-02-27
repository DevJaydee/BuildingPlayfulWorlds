using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraBehaviour : MonoBehaviour
{
	[SerializeField] private Transform playerBody = default;	// Transform of the playerbody
	[Space]
	[SerializeField] private string mouseXInputName = default;	// The input name for the X axis on the mouse
	[SerializeField] private string mouseYInputName = default;  // The input name for the Y axis on the mouse
	[SerializeField] private float mouseSensitivity = default;	// Mouse sensitivity

	// The max rotation on the X axis. This is to prevent the player from doing loops with the camera.
	private float mouseXAxisClamp = 90f;

	private float xAxisClamp = default;

	private void Awake()
	{
		LockCursor(true);
		xAxisClamp = 0.0f;
	}

	private void Update()
	{
		RotateCamera();
	}

	/// <summary>
	/// Rotates the camera on the Mouse X and Y axis.
	/// </summary>
	private void RotateCamera()
	{
		float multiplier = mouseSensitivity * Time.deltaTime;
		float mouseX = Input.GetAxisRaw(mouseXInputName) * multiplier;
		float mouseY = Input.GetAxisRaw(mouseYInputName) * multiplier;

		xAxisClamp += mouseY;

		if (xAxisClamp > mouseXAxisClamp)
		{
			xAxisClamp = 90f;
			mouseY = 0f;
			ClampXAxisRotationToValue(270);
		}
		else if (xAxisClamp < -mouseXAxisClamp)
		{
			xAxisClamp = -90f;
			mouseY = 0f;
			ClampXAxisRotationToValue(90);
		}

		transform.Rotate(Vector3.left * mouseY);
		playerBody.Rotate(Vector3.up * mouseX);
	}

	/// <summary>
	/// Clamps the Rotation on the X axis
	/// </summary>
	/// <param name="value"></param>
	private void ClampXAxisRotationToValue(float value)
	{
		Vector3 eulerRotation = transform.eulerAngles;
		eulerRotation.x = value;
		transform.eulerAngles = eulerRotation;
	}

	/// <summary>
	/// Locks and hides the cursor.
	/// </summary>
	/// <param name="lockState"></param>
	public void LockCursor(bool lockState)
	{
		Cursor.lockState = lockState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}
