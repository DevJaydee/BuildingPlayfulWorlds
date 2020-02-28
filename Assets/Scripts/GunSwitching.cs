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
			if(currentWeaponIndex <= transform.childCount - 1)
				currentWeaponIndex = transform.childCount;
			else
				currentWeaponIndex--;
		}

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
}
