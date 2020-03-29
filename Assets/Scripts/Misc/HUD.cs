using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
	#region Variables
	[SerializeField] private float hudUpdateInterval = 0.1f;                        // How often per second the HUD should update.
	[Space(15)]
	[Header("Player HUD Elements")]
	[SerializeField] private TextMeshProUGUI ammoTextMesh = default;                // Reference to the Ammo Text Mesh.
	[Space]
	[SerializeField] private TextMeshProUGUI grenadeTextMesh = default;             // Reference to the Ammo Text Mesh.
	[SerializeField] private ScriptableFloat grenadeAmount = default;               // Reference to the grenade amount float.
	[Space]
	[SerializeField] private Image playerHealthImage = default;                     // Reference to the Player Health Image Component.
	[SerializeField] private ScriptableFloat playerHealthAmount = default;          // Reference to the player health amount float.
	[Space]
	[SerializeField] private Image playerStaminaImage = default;                    // Reference to the Player Stamina Image Component.
	[SerializeField] private ScriptableFloat playerStaminaAmount = default;         // Reference to the player stamina amount float.

	[Header("Enemy HUD Elements")]
	[SerializeField] private TextMeshProUGUI currentEnemiesAliveTextMesh = default; // Reference to the Zombies alive text mesh.
	[SerializeField] private ScriptableFloat currentEnemiesAliveAmount = default;   // Reference to the Zombies Alive Scriptablefloat.
	[Space]
	[SerializeField] private TextMeshProUGUI currentEnemyWaveTextMesh = default;  // Reference to the Zombie Wave text mesh.
	[SerializeField] private ScriptableFloat currentEnemyWaveAmount = default;  // Reference to the current enemy wave Scriptablefloat.
	[Space]
	[SerializeField] private TextMeshProUGUI currentEnemyHealthMultiplierTextMesh = default;  // Reference to the Zombies health multiplier text mesh.
	[SerializeField] private ScriptableFloat currentEnemyHealthMultiplierAmount = default;  // Reference to the current enemy health mulitplier Scriptablefloat.
	#endregion

	#region Getters and Setters
	public TextMeshProUGUI AmmoTextMesh { get => ammoTextMesh; set => ammoTextMesh = value; }
	public float HudUpdateInterval { get => hudUpdateInterval; set => hudUpdateInterval = value; }
	#endregion

	private void Start()
	{
		StartCoroutine(UpdateHUD());
	}

	private IEnumerator UpdateHUD()
	{
		while(true)
		{
			grenadeTextMesh.text = grenadeAmount.Value.ToString();
			playerHealthImage.fillAmount = playerHealthAmount.Value / 100;
			playerStaminaImage.fillAmount = playerStaminaAmount.Value / 100;

			currentEnemiesAliveTextMesh.text = "Zombies Alive: " + currentEnemiesAliveAmount.Value;
			currentEnemyWaveTextMesh.text = "Current Wave: " + currentEnemyWaveAmount.Value;
			currentEnemyHealthMultiplierTextMesh.text = "Health Multiplier: " + currentEnemyHealthMultiplierAmount.Value;

			yield return new WaitForSeconds(hudUpdateInterval);
		}
	}
}
