using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableFloat", menuName = "ScriptableObjects/New Scriptable Float")]
public class ScriptableFloat : ScriptableObject
{
	[SerializeField] private new string name;
	[SerializeField] private float startingValue;
	[SerializeField] private float value;

	public string Name { get => name; set => name = value; }
	public float StartingValue { get => startingValue; set => startingValue = value; }
	public float Value { get => value; set => this.value = value; }

	private void Awake()
	{
	}

	private void OnEnable()
	{
		value = startingValue;
	}
}
