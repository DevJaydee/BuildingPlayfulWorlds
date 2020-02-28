using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSwitching : MonoBehaviour
{
	#region Variables
	[SerializeField] private int currentWeaponIndex = 0;    // Which weapon is currently active
	#endregion

	#region Monobehaviour Callback
	private void Start()
	{
		SelectWeapon();
	}

	private void Update()
	{
		GetMouseScrollInput();
		GetKeyInputToSwitchWeapons();
	}

	private void GetMouseScrollInput()
	{
		int previousWeaponIndex = currentWeaponIndex;

		if(Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			if(currentWeaponIndex >= transform.childCount - 1)
				currentWeaponIndex = 0;
			else
				currentWeaponIndex++;
		}
		if(Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			if(currentWeaponIndex <= 0)
				currentWeaponIndex = transform.childCount - 1;
			else
				currentWeaponIndex--;
		}

		#region Keyboard weapon switching

		if(Input.GetKey(KeyCode.Alpha1))
		{
			currentWeaponIndex = 0;
		}
		if(Input.GetKey(KeyCode.Alpha2) && transform.childCount >= 2)
		{
			currentWeaponIndex = 1;
		}
		if(Input.GetKey(KeyCode.Alpha3) && transform.childCount >= 2)
		{
			currentWeaponIndex = 2;
		}
		if(Input.GetKey(KeyCode.Alpha4) && transform.childCount >= 2)
		{
			currentWeaponIndex = 3;
		}
		if(Input.GetKey(KeyCode.Alpha5) && transform.childCount >= 2)
		{
			currentWeaponIndex = 4;
		}
		if(Input.GetKey(KeyCode.Alpha6) && transform.childCount >= 2)
		{
			currentWeaponIndex = 5;
		}
		if(Input.GetKey(KeyCode.Alpha7) && transform.childCount >= 2)
		{
			currentWeaponIndex = 6;
		}
		if(Input.GetKey(KeyCode.Alpha8) && transform.childCount >= 2)
		{
			currentWeaponIndex = 7;
		}
		if(Input.GetKey(KeyCode.Alpha9) && transform.childCount >= 2)
		{
			currentWeaponIndex = 8;
		}
		#endregion

		if(previousWeaponIndex != currentWeaponIndex)
		{
			SelectWeapon();
		}
	}
	#endregion

	public void SelectWeapon()
	{
		int i = 0;
		foreach(Transform weapon in transform)
		{
			if(i == currentWeaponIndex)
				weapon.gameObject.SetActive(true);
			else
				weapon.gameObject.SetActive(false);

			i++;
		}
	}

	private void GetKeyInputToSwitchWeapons()
	{

	}
}
