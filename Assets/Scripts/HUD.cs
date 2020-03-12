using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
	#region Variables
	[SerializeField] private float hudUpdateInterval = 0.1f;                // How often per second the HUD should update.
	[Space(15)]
	[SerializeField] private TextMeshProUGUI ammoTextMesh = default;        // Reference to the Ammo Text Mesh.
	#endregion

	#region Getters and Setters
	public TextMeshProUGUI AmmoTextMesh { get => ammoTextMesh; set => ammoTextMesh = value; }
	public float HudUpdateInterval { get => hudUpdateInterval; set => hudUpdateInterval = value; }
	#endregion
}
