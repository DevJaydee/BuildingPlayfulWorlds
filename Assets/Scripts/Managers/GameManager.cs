using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
	Playing,
	Paused,
	Gameover
}

public class GameManager : MonoBehaviour
{
	#region Variables
	private static GameManager instance = null; // Instance of this object. or rather, this instance.

	[SerializeField] private GameState gameState = GameState.Playing;   // State of the game
	[Space]
	[SerializeField] private HUD hud = default;     // Reference to the HUD component.
	[SerializeField] private GameObject uiFadeIn = default; // Reference to the FadeIn gameobject.
	[Space]
	[SerializeField] private GameObject[] enemyTargetObjects = default; // Reference to the enemy target GameObjects.
	[SerializeField] private Player player = default; // Reference to the PlayerBehaviour.
	[SerializeField] private PlayerCamera playerCamera = default; // Reference to the PlayerCameraBehaviour.
	[SerializeField] private Transform playerWeaponTransform = default; // Reference to the PlayerWeaponTransform.
	[SerializeField] private List<Gun> playerWeapons = default; // List with all the Weapons the player has picked up.
	[SerializeField] private Gun playerActiveWeapon = default;  // Reference to the active and equiped weapon.
	[Space]
	[SerializeField] private KeyCode pauseMenuKey = KeyCode.Escape; // Which key you have to press to open the pause menu.
	[SerializeField] private GameObject pauseMenuObject = default;  // The Pausemenu.
	[SerializeField] private bool pauseMenuEnabled = false; // Bool to keep track of the state of the pause menu.
	[Space]
	[SerializeField] private GameObject GameOverMenu = default;     // Reference to the Gameover menu object.
	#endregion

	#region Getters & Setters
	public static GameManager Instance { get => instance; set => instance = value; }
	public GameObject[] EnemyTargetObjects { get => enemyTargetObjects; set => enemyTargetObjects = value; }
	public Player Player { get => player; set => player = value; }
	public HUD Hud { get => hud; set => hud = value; }
	public GameState GameState { get => gameState; set => gameState = value; }
	#endregion

	private void Awake()
	{
		if(instance != this || instance == null)
			instance = this;
	}

	private void Start()
	{
		uiFadeIn.SetActive(true);

		GetComponents();
		GetWeapons();
		StartCoroutine(UpdateHUD());
		StartCoroutine(GetActiveWeapon());

		Time.timeScale = 1;
	}

	private void Update()
	{
		if(Input.GetKeyDown(pauseMenuKey) && !pauseMenuEnabled)
			TriggerPauseMenu();

		if(gameState == GameState.Gameover)
		{
			GameOver();
		}
	}

	private void GameOver()
	{
		GameOverMenu.SetActive(true);
		Time.timeScale = 0;
		playerCamera.LockCursor(false);
	}

	private void GetComponents()
	{
		enemyTargetObjects = GameObject.FindGameObjectsWithTag("Target");
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		playerCamera = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerCamera>();
	}

	public void GetWeapons()
	{
		for(int i = 0; i < playerWeaponTransform.childCount; i++)
		{
			if(playerWeaponTransform.GetChild(i).GetComponent<Gun>() && !playerWeapons.Contains(playerWeaponTransform.GetChild(i).GetComponent<Gun>()))
			{
				playerWeapons.Add(playerWeaponTransform.GetChild(i).GetComponent<Gun>());
			}
		}
	}

	private IEnumerator GetActiveWeapon()
	{
		while(true)
		{
			for(int i = 0; i < playerWeapons.Count; i++)
			{
				if(playerWeapons[i].GunState == GunState.Equiped)
				{
					playerActiveWeapon = playerWeapons[i];
					break;
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	private IEnumerator UpdateHUD()
	{
		while(true)
		{
			if(hud.AmmoTextMesh)
				hud.AmmoTextMesh.text = playerActiveWeapon.CurrentAmmo + " / " + playerActiveWeapon.MaxAmmo;

			yield return new WaitForSeconds(hud.HudUpdateInterval);
		}
	}

	#region Menu Functions
	public void TriggerPauseMenu()
	{
		pauseMenuEnabled = !pauseMenuEnabled;

		Time.timeScale = pauseMenuEnabled ? 0f : 1f;
		gameState = Time.timeScale == 0 ? GameState.Paused : GameState.Playing;

		pauseMenuObject.SetActive(pauseMenuEnabled);
		playerCamera.LockCursor(!pauseMenuEnabled);
	}

	public void QuitApplication()
	{
		Application.Quit();
	}
	#endregion
}
