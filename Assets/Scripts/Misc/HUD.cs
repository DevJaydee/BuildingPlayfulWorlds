using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
	#region Variables
	private static HUD instance = null;                                             // Static instance of the HUD

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
	[Space]
	[SerializeField] private Image playerBaseHealthImage = default;                 // Reference to the Player Base Health Image Component.
	[SerializeField] private ScriptableFloat playerBaseHealthAmount = default;      // Reference to the Player Base Health Scriptable Float.
	[Space]
	[SerializeField] private TextMeshProUGUI playerPointsTextMesh = default;        // Reference to the Player Points Text Mesh.
	[SerializeField] private ScriptableFloat playerPointsAmount = default;          // Reference to the Player Points Scriptable Float.
	[Space]
	[SerializeField] private TextMeshProUGUI turretUpgradeTextMesh = default;       // Reference to the Turret Upgrade Text Mesh.
	[SerializeField] private ScriptableFloat turretUpgradeAmount = default;         // Reference to the Turret Upgrade amount Scriptable Object.

	[Header("Enemy HUD Elements")]
	[SerializeField] private TextMeshProUGUI currentEnemiesAliveTextMesh = default; // Reference to the Zombies alive text mesh.
	[SerializeField] private ScriptableFloat currentEnemiesAliveAmount = default;   // Reference to the Zombies Alive Scriptablefloat.
	[Space]
	[SerializeField] private TextMeshProUGUI currentEnemyWaveTextMesh = default;  // Reference to the Zombie Wave text mesh.
	[SerializeField] private ScriptableFloat currentEnemyWaveAmount = default;  // Reference to the current enemy wave Scriptablefloat.
	[Space]
	[SerializeField] private TextMeshProUGUI currentEnemyHealthMultiplierTextMesh = default;  // Reference to the Zombies health multiplier text mesh.
	[SerializeField] private ScriptableFloat currentEnemyHealthMultiplierAmount = default;  // Reference to the current enemy health mulitplier Scriptablefloat.
	[Space]
	[SerializeField] private TextMeshProUGUI nextWaveCountdownTextMesh = default;               // Reference to the nextWaveCountdownTextMesh.
	[SerializeField] private float nextWaveCountdown = default;                                 // Time until next wave.
	#endregion

	#region Getters and Setters
	public static HUD Instance { get => instance; set => instance = value; }
	public TextMeshProUGUI AmmoTextMesh { get => ammoTextMesh; set => ammoTextMesh = value; }
	public float HudUpdateInterval { get => hudUpdateInterval; set => hudUpdateInterval = value; }
	public float NextWaveCountdown { get => nextWaveCountdown; set => nextWaveCountdown = value; }
	#endregion

	#region Monobehaviour Callbacks
	private void Awake()
	{
		if(!instance || instance != this)
			instance = this;
	}
	private void Start()
	{
		StartCoroutine(UpdateHUD());
	}
	private void Update()
	{
		WaveCountdown();
	}

	#endregion
	/// <summary>
	/// Update the HUD on an interval. This is done because the HUD really doesn't have to be updated every frame.
	/// </summary>
	/// <returns></returns>
	private IEnumerator UpdateHUD()
	{
		while(true)
		{
			grenadeTextMesh.text = grenadeAmount.Value.ToString();
			playerHealthImage.fillAmount = playerHealthAmount.Value / 100;
			playerStaminaImage.fillAmount = playerStaminaAmount.Value / 100;
			playerBaseHealthImage.fillAmount = playerBaseHealthAmount.Value / 100;
			playerPointsTextMesh.text = "Points: \n" + playerPointsAmount.Value;
			turretUpgradeTextMesh.text = "Upgrade turrets: " + turretUpgradeAmount.Value + "pts";

			currentEnemiesAliveTextMesh.text = "Zombies Alive: " + currentEnemiesAliveAmount.Value;
			currentEnemyWaveTextMesh.text = "Current Wave: " + currentEnemyWaveAmount.Value;
			currentEnemyHealthMultiplierTextMesh.text = "Health Multiplier: " + currentEnemyHealthMultiplierAmount.Value;

			yield return new WaitForSeconds(hudUpdateInterval);
		}
	}

	/// <summary>
	/// Start Enemy Wave Countdown when the current wave is done
	/// </summary>
	public void WaveCountdown()
	{
		nextWaveCountdown -= Time.deltaTime;
		if(nextWaveCountdown > 0)
			nextWaveCountdownTextMesh.text = "Next Wave in: " + nextWaveCountdown.ToString("F2");
		else
			nextWaveCountdownTextMesh.text = "";

	}
}
