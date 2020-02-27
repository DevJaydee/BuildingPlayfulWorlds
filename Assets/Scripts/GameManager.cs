﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region Variables
	private static GameManager instance = null; // Instance of this object. or rather, this instance.
	[SerializeField] private GameObject playerObject = default; // Reference to the player GameObject.
	[SerializeField] private PlayerBehaviour playerBehaviour = default; // Reference to the PlayerBehaviour.
	[SerializeField] private PlayerCameraBehaviour playerCameraBehaviour = default; // Reference to the PlayerCameraBehaviour.
	[Space]
	[SerializeField] private KeyCode pauseMenuKey = KeyCode.Escape; // Which key you have to press to open the pause menu.
	[SerializeField] private GameObject pauseMenuObject = default;  // The Pausemenu.
	[SerializeField] private bool pauseMenuEnableb = false; // Bool to keep track of the state of the pause menu.
	#endregion

	#region Getters & Setters
	public static GameManager Instance { get => instance; set => instance = value; }
	public GameObject PlayerObject { get => playerObject; set => playerObject = value; }
	public PlayerBehaviour PlayerBehaviour { get => playerBehaviour; set => playerBehaviour = value; }
	#endregion

	private void Awake()
	{
		if(instance != this || instance == null)
			instance = this;
	}

	private void Start()
	{
		GetComponents();
	}

	private void Update()
	{
		if(Input.GetKeyDown(pauseMenuKey))
			TriggerPauseMenu();
	}

	void GetComponents()
	{
		playerObject = GameObject.FindGameObjectWithTag("Player");
		playerBehaviour = playerObject.GetComponent<PlayerBehaviour>();
		playerCameraBehaviour = playerObject.GetComponentInChildren<PlayerCameraBehaviour>();
	}

	#region Menu Functions
	public void TriggerPauseMenu()
	{
		pauseMenuEnableb = !pauseMenuEnableb;

		Time.timeScale = pauseMenuEnableb ? 0f : 1f;
		pauseMenuObject.SetActive(pauseMenuEnableb);
		playerCameraBehaviour.LockCursor(pauseMenuEnableb);
	}
	#endregion
}
