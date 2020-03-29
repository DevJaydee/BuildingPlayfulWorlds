using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	#region Variables
	private static EnemyManager instance = null;                        // Instance of this class.

	[SerializeField] private Transform[] enemySpawns = default;         // Array with all the spawn points for the enemies.
	[SerializeField] private Transform enemySpawnParent = default;      // Which transform the enemies will be parented to.
	[Space]
	[SerializeField] private float timeBetweenWaves = default;          // How long before the next wave spawns.
	[SerializeField] private EnemyWave[] enemyWaves = default;          // Array with all the enemy waves.
	[Space]
	[SerializeField] private List<GameObject> enemiesInScene = new List<GameObject>();      // List with all the enemies in the scene.
	[Space]
	[SerializeField] private ScriptableFloat currentEnemiesAliveScriptableFloat = default;              // Reference to the current Enemies alive scriptable float object
	[SerializeField] private ScriptableFloat currentEnemyHealthMultiplierScriptableFloat = default;     // Reference to the current Enemy Health Multiplier scriptable float object
	[SerializeField] private ScriptableFloat currentEnemyWaveScriptableFloat = default;                 // Reference to the current Enemy Wave scriptable float object
	#endregion

	#region Properties
	public static EnemyManager Instance { get => instance; set => instance = value; }
	#endregion

	#region Monobehaviour Callbacks
	private void Awake()
	{
		if(!instance || instance != this)
			instance = this;
	}

	private void Start()
	{
		StartCoroutine(SpawnEnemyWaves());
	}

	private void Update()
	{
		currentEnemiesAliveScriptableFloat.Value = enemiesInScene.Count;
	}
	#endregion

	#region Private Functions
	/// <summary>
	/// This is the core functionality of this class.
	/// This spawns all the enemies and applies the multipliers to the enemies each new wave.
	/// This picks a random spot to spawn the enemies at.
	/// </summary>
	/// <returns></returns>
	private IEnumerator SpawnEnemyWaves()
	{
		// loop Through all the enemy waves
		for(int i = 0; i < enemyWaves.Length; i++)
		{
			currentEnemyWaveScriptableFloat.Value++;
			// Spawn the amount of enemies from the wave properties
			for(int x = 0; x < enemyWaves[i].EnemyAmount; x++)
			{
				int randSpawnIndex = Random.Range(0, enemySpawns.Length);
				GameObject newEnemyGO = Instantiate(enemyWaves[i].EnemyPrefab, enemySpawns[randSpawnIndex].position, Quaternion.identity, enemySpawnParent);
				newEnemyGO.GetComponent<Enemy>().Health *= enemyWaves[i].EnemyHealthMultiplier;
				newEnemyGO.GetComponent<Enemy>().MoveSpeed *= enemyWaves[i].EnemySpeedMultiplier;

				enemiesInScene.Add(newEnemyGO);

				currentEnemyHealthMultiplierScriptableFloat.Value = enemyWaves[i].EnemyHealthMultiplier;
				yield return new WaitForSeconds(enemyWaves[i].EnemySpawnInterval);
			}
			HUD.Instance.NextWaveCountdown = timeBetweenWaves;
			yield return new WaitForSeconds(timeBetweenWaves);
		}
		yield return null;
	}

	/// <summary>
	/// Public so the Enemy can call this and remove itself from the List when dead
	/// </summary>
	/// <param name="enemy"></param>
	public void RemoveFromList(GameObject enemy)
	{
		enemiesInScene.Remove(enemy);
	}
	#endregion
}

[System.Serializable]
public struct EnemyWave
{
	#region Variables
	[SerializeField] private GameObject enemyPrefab;            // Prefab of the enemy that will spawn.
	[Space]
	[SerializeField] private int enemyAmount;                   // How many enemies will spawn this wave.
	[SerializeField] private float enemySpawnInterval;            // How many second inbetween spawns.
	[SerializeField] private float enemyHealthMultiplier;       // Enemy Health multiplier. This normally increase each wave.
	[SerializeField] private float enemySpeedMultiplier;        // Enemy Speed Multiplier.
	#endregion

	#region Properties
	public GameObject EnemyPrefab { get => enemyPrefab; set => enemyPrefab = value; }
	public int EnemyAmount { get => enemyAmount; set => enemyAmount = value; }
	public float EnemySpawnInterval { get => enemySpawnInterval; set => enemySpawnInterval = value; }
	public float EnemyHealthMultiplier { get => enemyHealthMultiplier; set => enemyHealthMultiplier = value; }
	public float EnemySpeedMultiplier { get => enemySpeedMultiplier; set => enemySpeedMultiplier = value; }
	#endregion
}
