using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour, IDamagable
{
	#region Variables
	private static PlayerBase instance = default;                               // Reference to the Playerbase instance;

	[SerializeField] private ScriptableFloat baseHealth = default;              // Reference to the playerbasehealth scriptableobject
	[SerializeField] private ScriptableFloat playerPoints = default;            // Reference to the Player Points scriptable float.
	[SerializeField] private ScriptableFloat costToUpgrade;                     // How much it costs to upgrade the turrets.
	[SerializeField] private KeyCode turretUpgradeKey = KeyCode.E;              // Which key to press to upgrade turrets.
	[SerializeField] private Turret[] turretsOnBase = default;                  // Array with all the turrets on the base.
	[SerializeField] private float upgradeMultiplier = 1.5f;                    // How much each upgrade multiplies to existing values of the turrets.
	#endregion

	#region Properties
	public static PlayerBase Instance { get => instance; set => instance = value; }
	public KeyCode TurretUpgradeKey { get => turretUpgradeKey; set => turretUpgradeKey = value; }
	public ScriptableFloat PlayerPoints { get => playerPoints; set => playerPoints = value; }
	public ScriptableFloat CostToUpgrade { get => costToUpgrade; set => costToUpgrade = value; }
	#endregion

	#region Monobehaviour Callbacks
	private void Awake()
	{
		if(!instance || instance != this)
			instance = this;
	}
	#endregion

	#region Voids
	/// <summary>
	/// Implements the IDamagable Interface
	/// </summary>
	/// <param name="damageAmount"></param>
	public void Damage(float damageAmount)
	{
		baseHealth.Value -= damageAmount;
		if(baseHealth.Value < -0)
			GameManager.Instance.GameState = GameState.Gameover;
	}

	/// <summary>
	/// Upgrade turret values when the player has enough points.
	/// </summary>
	public void UpgradeTurrets()
	{
		if(playerPoints.Value >= costToUpgrade.Value && costToUpgrade.Value < 800)
		{
			for(int i = 0; i < turretsOnBase.Length; i++)
			{
				turretsOnBase[i].RotationSpeed *= upgradeMultiplier;
				turretsOnBase[i].GunShootingInterval -= upgradeMultiplier / 10;
				turretsOnBase[i].GunDamage *= upgradeMultiplier;
			}
			playerPoints.Value -= costToUpgrade.Value;
			costToUpgrade.Value *= upgradeMultiplier;
		}
	}
	#endregion
}
