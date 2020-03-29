using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
	/// <summary>
	/// Base IDamagable function
	/// </summary>
	/// <param name="damageAmount"></param>
	void Damage(float damageAmount);
}
