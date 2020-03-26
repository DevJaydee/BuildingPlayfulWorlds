using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
	#region Variables
	[SerializeField] private Building[] buildingsToSpawn = default;             // Array with all the possible building configs.
	[SerializeField] private List<Building> buildingsInScene = default;         // List with all the Buildings in the Scene.
	[SerializeField] private Transform[] buildingSpawnPoints = default;         // Array with all the Spawn Points for the Buildings.
	#endregion

	#region Properties

	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		StartCoroutine(Generate());
	}
	#endregion

	#region Private Voids
	private IEnumerator Generate()
	{
		for(int i = 0; i < buildingSpawnPoints.Length; i++)
		{
			Building newBuilding = buildingsToSpawn[Random.Range(0, buildingsToSpawn.Length)];
			int amountOfBuildingBlocks = Random.Range(newBuilding.MinHeight, newBuilding.MaxHeight);
			int baseObjectIndex = Random.Range(0, newBuilding.BaseObjects.Length - 1);
			int roofObjectIndex = Random.Range(0, newBuilding.RoofObjects.Length - 1);

			newBuilding.CurrentHeight = amountOfBuildingBlocks;

			GameObject newBuildingGoParent = new GameObject();
			newBuildingGoParent.transform.parent = buildingSpawnPoints[i].transform;
			newBuildingGoParent.name = newBuildingGoParent.transform.parent.name + "_Objects";
			newBuildingGoParent.transform.position = buildingSpawnPoints[i].position;
			newBuildingGoParent.transform.rotation = buildingSpawnPoints[i].rotation;

			Instantiate(newBuilding.BaseObjects[baseObjectIndex], newBuildingGoParent.transform.position, newBuildingGoParent.transform.rotation, newBuildingGoParent.transform);

			for(int buildingBlockIndex = 1; buildingBlockIndex < amountOfBuildingBlocks + 1; buildingBlockIndex++)
			{
				int midObjectIndex = Random.Range(0, newBuilding.MidObjects.Length - 1);

				float heightOffset = 5f * buildingBlockIndex;
				Vector3 spawnPos = new Vector3(newBuildingGoParent.transform.position.x, newBuildingGoParent.transform.position.y + heightOffset, newBuildingGoParent.transform.position.z);

				Instantiate(newBuilding.MidObjects[midObjectIndex], spawnPos, newBuildingGoParent.transform.rotation, newBuildingGoParent.transform);

				if(buildingBlockIndex == amountOfBuildingBlocks)
				{
					spawnPos.y += 5f;
					Instantiate(newBuilding.RoofObjects[roofObjectIndex], spawnPos, newBuildingGoParent.transform.rotation, newBuildingGoParent.transform);
				}
				yield return new WaitForSeconds(0.5f);
			}

			buildingsInScene.Add(newBuilding);
		}

		yield return null;
	}
	#endregion

	#region Public Voids

	#endregion
}

[System.Serializable]
public struct Building
{
	#region Variables
	[Header("Objects")]
	[SerializeField] private GameObject[] baseObjects;            // All the possible Base Objects. (a.k.a. Ground Floor)
	[SerializeField] private GameObject[] midObjects;             // All the possible Middle Objects. (a.k.a. First through tenth floor)
	[SerializeField] private GameObject[] roofObjects;            // All the possible Roof Objects.

	[Header("Variables")]
	[SerializeField] private int minHeight;                             // Minimum amount of floors.
	[SerializeField] private int maxHeight;                             // Maximum amount of floors.
	[SerializeField] private int currentHeight;                         // Current height of the building.
	#endregion

	#region Properties
	public GameObject[] BaseObjects { get => baseObjects; set => baseObjects = value; }
	public GameObject[] MidObjects { get => midObjects; set => midObjects = value; }
	public GameObject[] RoofObjects { get => roofObjects; set => roofObjects = value; }

	public int MinHeight { get => minHeight; set => minHeight = value; }
	public int MaxHeight { get => maxHeight; set => maxHeight = value; }
	public int CurrentHeight { get => currentHeight; set => currentHeight = value; }
	#endregion
}
