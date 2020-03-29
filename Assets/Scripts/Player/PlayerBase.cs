using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour, IDamagable
{
	#region Variables
	[SerializeField] private ScriptableFloat baseHealth = default;              // Reference to the playerbasehealth scriptableobject
	#endregion

	#region Voids
	public void Damage(float damageAmount)
	{
		baseHealth.Value -= damageAmount;
		if(baseHealth.Value < -0)
			GameManager.Instance.GameState = GameState.Gameover;
	}
	#endregion
}
