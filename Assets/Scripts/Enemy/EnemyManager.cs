﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	#region Variables
	[SerializeField] private Transform[] enemySpawns = default;         // Array with all the spawn points for the enemies.
	[SerializeField] private Transform enemySpawnParent = default;      // Which transform the enemies will be parented to.
	[Space]
	[SerializeField] private float timeBetweenWaves = default;          // How long before the next wave spawns.
	[SerializeField] private EnemyWave[] enemyWaves = default;          // Array with all the enemy waves.
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		StartCoroutine(SpawnEnemyWaves());
	}
	#endregion

	#region Private Voids
	/// <summary>
	/// This is the core functionality of this class.
	/// This spawns all the enemies and applies the multipliers to the enemies each new wave.
	/// This picks a random spot to spawn the enemies at.
	/// </summary>
	/// <returns></returns>
	private IEnumerator SpawnEnemyWaves()
	{
		while(true)
		{
			// loop Through all the enemy waves
			for(int i = 0; i < enemyWaves.Length; i++)
			{

				// Spawn the amount of enemies from the wave properties
				for(int x = 0; x < enemyWaves[i].EnemyAmount; x++)
				{
					int randSpawnIndex = Random.Range(0, enemySpawns.Length);
					GameObject newEnemyGO = Instantiate(enemyWaves[i].EnemyPrefab, enemySpawns[randSpawnIndex].position, Quaternion.identity, enemySpawnParent);
					newEnemyGO.GetComponent<Enemy>().Health *= enemyWaves[i].EnemyHealthMultiplier;

					yield return new WaitForSeconds(enemyWaves[i].EnemySpawnInterval);
				}
				yield return new WaitForSeconds(timeBetweenWaves);
			}

			yield return null;
		}
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
	[SerializeField] private int enemySpawnInterval;            // How many second inbetween spawns.
	[SerializeField] private float enemyHealthMultiplier;       // Enemy Health multiplier. This normally increase each wave.
	#endregion

	#region Properties
	public GameObject EnemyPrefab { get => enemyPrefab; set => enemyPrefab = value; }
	public int EnemyAmount { get => enemyAmount; set => enemyAmount = value; }
	public int EnemySpawnInterval { get => enemySpawnInterval; set => enemySpawnInterval = value; }
	public float EnemyHealthMultiplier { get => enemyHealthMultiplier; set => enemyHealthMultiplier = value; }
	#endregion
}