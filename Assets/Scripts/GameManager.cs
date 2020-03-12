using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	#region Variables
	private static GameManager instance = null; // Instance of this object. or rather, this instance.

	[SerializeField] private HUD hud = default;     // Reference to the HUD component.
	[Space]
	[SerializeField] private GameObject playerObject = default; // Reference to the player GameObject.
	[SerializeField] private Player player = default; // Reference to the PlayerBehaviour.
	[SerializeField] private PlayerCamera playerCamera = default; // Reference to the PlayerCameraBehaviour.
	[SerializeField] private Transform playerWeaponTransform = default; // Reference to the PlayerWeaponTransform.
	[SerializeField] private List<Gun> playerWeapons = default; // List with all the Weapons the player has picked up.
	[SerializeField] private Gun playerActiveWeapon = default;  // Reference to the active and equiped weapon.
	[Space]
	[SerializeField] private KeyCode pauseMenuKey = KeyCode.Escape; // Which key you have to press to open the pause menu.
	[SerializeField] private GameObject pauseMenuObject = default;  // The Pausemenu.
	[SerializeField] private bool pauseMenuEnableb = false; // Bool to keep track of the state of the pause menu.
	#endregion

	#region Getters & Setters
	public static GameManager Instance { get => instance; set => instance = value; }
	public GameObject PlayerObject { get => playerObject; set => playerObject = value; }
	public Player Player { get => player; set => player = value; }
	public HUD Hud { get => hud; set => hud = value; }
	#endregion

	private void Awake()
	{
		if(instance != this || instance == null)
			instance = this;
	}

	private void Start()
	{
		GetComponents();
		GetWeapons();
		StartCoroutine(UpdateHUD());
		StartCoroutine(GetActiveWeapon());
	}

	private void Update()
	{
		if(Input.GetKeyDown(pauseMenuKey))
			TriggerPauseMenu();
	}

	private void GetComponents()
	{
		playerObject = GameObject.FindGameObjectWithTag("Player");
		player = playerObject.GetComponent<Player>();
		playerCamera = playerObject.GetComponentInChildren<PlayerCamera>();
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
		pauseMenuEnableb = !pauseMenuEnableb;

		Time.timeScale = pauseMenuEnableb ? 0f : 1f;
		pauseMenuObject.SetActive(pauseMenuEnableb);
		playerCamera.LockCursor(pauseMenuEnableb);
	}
	#endregion
}
