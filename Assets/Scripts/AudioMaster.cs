using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour
{
	#region Variables
	private static AudioMaster instance = null; // Instance of this
	[SerializeField] private AudioClip[] groundWalkAudioClips = default;    // Array with all the ground walk audio clips.

	public static AudioMaster Instance { get => instance; set => instance = value; }
	#endregion

	#region Monobehaviour Callbacks
	private void Awake()
	{
		if(!instance || instance != this)
			instance = this;
	}
	#endregion

	#region Public Voids
	public void PlayWalkSound(AudioSource source)
	{
		int i = Random.Range(0, groundWalkAudioClips.Length);
		source.clip = groundWalkAudioClips[i];
	}
	#endregion
}
