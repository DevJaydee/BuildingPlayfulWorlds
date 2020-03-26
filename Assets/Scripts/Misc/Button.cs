using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
	private enum ButtonState
	{
		OFF,
		ON
	}

	[SerializeField] private ButtonState buttonState = ButtonState.OFF;
	[SerializeField] private bool triggered = false;
	[Space]
	[SerializeField] private GameObject buttonStateOffGO = default;
	[SerializeField] private GameObject buttonStateOnGO = default;
	[Space]
	[SerializeField] private GameObject objectToTrigger = default;

	private void Start()
	{
		StartButtonStateEvent();
	}

	public void Triggerbutton()
	{
		triggered = !triggered;

		buttonState = triggered ? ButtonState.ON : ButtonState.OFF;

		StartButtonStateEvent();
	}

	private void StartButtonStateEvent()
	{
		switch (buttonState)
		{
			case ButtonState.OFF:
				buttonStateOnGO.SetActive(false);
				buttonStateOffGO.SetActive(true);
				break;

			case ButtonState.ON:
				buttonStateOnGO.SetActive(true);
				buttonStateOffGO.SetActive(false);
				break;

			default:
				break;
		}

		objectToTrigger.GetComponent<Interactable>().StartTriggerEvent(triggered);
	}

	private void OnApplicationQuit()
	{
		buttonStateOffGO.SetActive(true);
		buttonStateOnGO.SetActive(true);
	}
}
