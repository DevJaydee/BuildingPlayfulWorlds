using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
	#region Variables
	[Header("Variables")]
	[SerializeField] private string buildingName;					// Name of the Building (CornerBuilding, Fronbuilding etc.)
	[SerializeField] private float heightDif;						// Height Difference between objects	
	[SerializeField] private int minHeight;							// Minimum amount of floors.
	[SerializeField] private int maxHeight;							// Maximum amount of floors.
	[SerializeField] private int currentHeight;						// Current height of the building.

	[Header("Objects")]
	[SerializeField] private GameObject[] baseObjects;				// All the possible Base Objects. (a.k.a. Ground Floor)
	[SerializeField] private GameObject[] midObjects;				// All the possible Middle Objects. (a.k.a. First through tenth floor)
	[SerializeField] private GameObject[] roofObjects;				// All the possible Roof Objects.

	[Header("Editor Specific Values")]
	[SerializeField] private float buildingPlacementSpeed = 0.1f; // How long the IEnumerator has to wait before placing another piece to the building.
	#endregion

	#region Properties
	public string BuildingName { get => buildingName; set => buildingName = value; }
	public int MinHeight { get => minHeight; set => minHeight = value; }
	public int MaxHeight { get => maxHeight; set => maxHeight = value; }
	public int CurrentHeight { get => currentHeight; set => currentHeight = value; }

	public GameObject[] BaseObjects { get => baseObjects; set => baseObjects = value; }
	public GameObject[] MidObjects { get => midObjects; set => midObjects = value; }
	public GameObject[] RoofObjects { get => roofObjects; set => roofObjects = value; }
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		StartCoroutine(Generate());
	}
	#endregion

	#region Private IEnumerators
	/// <summary>
	/// Generates the building from the list with objects.
	/// </summary>
	/// <returns></returns>
	private IEnumerator Generate()
	{
		int amountOfBuildingBlocks = Random.Range(minHeight, maxHeight);
		currentHeight = amountOfBuildingBlocks;

		int baseObjectIndex = Random.Range(0, BaseObjects.Length - 1);
		int roofObjectIndex = Random.Range(0, RoofObjects.Length - 1);

		Instantiate(BaseObjects[baseObjectIndex], transform.position, transform.rotation, transform);

		for(int buildingBlockIndex = 1; buildingBlockIndex < amountOfBuildingBlocks + 1; buildingBlockIndex++)
		{
			int midObjectIndex = Random.Range(0, midObjects.Length - 1);

			float heightOffset = heightDif * buildingBlockIndex;
			Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + heightOffset, transform.position.z);

			Instantiate(MidObjects[midObjectIndex], spawnPos, transform.rotation, transform);

			if(buildingBlockIndex == amountOfBuildingBlocks)
			{
				spawnPos.y += heightDif;
				Instantiate(RoofObjects[roofObjectIndex], spawnPos, transform.rotation, transform);
			}
			yield return new WaitForSeconds(buildingPlacementSpeed);
		}
		yield return null;
	}
	#endregion

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(transform.position, Vector3.one);
	}
}
