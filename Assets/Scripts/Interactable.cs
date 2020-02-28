using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	private enum InteractableState
	{
		OFF,
		ON
	}

	[SerializeField] private InteractableState interactableState = InteractableState.OFF;
	[SerializeField] private bool triggered = false;
	[Space]
	[SerializeField] private Animator anim = default;

	public void StartTriggerEvent(bool _trigger)
	{
		triggered = _trigger;
		anim.SetBool("Triggered", triggered);
		interactableState = triggered ? InteractableState.ON : InteractableState.OFF;
	}
}
