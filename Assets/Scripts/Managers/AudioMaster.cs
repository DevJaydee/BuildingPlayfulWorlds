using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour
{
	#region Variables
	private static AudioMaster instance = null; // Instance of this
	[Header("Ground Walking Sounds")]
	[SerializeField] private AudioClip[] groundWalkAudioClips = default;    // Array with all the ground walk audio clips.
	[Header("Zombie Growls")]
	[SerializeField] private AudioClip[] zombieGrowls = default;            // Array with zombie Growls
	[Header("Weapon Sounds")]
	[SerializeField] private AudioClip[] PistolAudio = default;       // Audio clip for the pistol.
	[SerializeField] private AudioClip[] RifleAudio = default;        // Audio clip for the Rifle.
	[SerializeField] private AudioClip[] SniperAudio = default;       // Audio clip for the Sniper.
	[SerializeField] private AudioClip[] SilencedAudio = default;     // Audio clip for the Silenced.

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
	/// <summary>
	/// Selects and plays a random walking audio clip
	/// </summary>
	/// <param name="source"></param>
	public void PlayWalkSound(AudioSource source)
	{
		int i = Random.Range(0, groundWalkAudioClips.Length - 1);
		source.clip = groundWalkAudioClips[i];
		source.PlayOneShot(source.clip);
	}

	/// <summary>
	/// Selects and play a random zombiegrowls
	/// </summary>
	/// <param name="source"></param>
	public void PlayZombieGrowl(AudioSource source)
	{
		int i = Random.Range(0, zombieGrowls.Length - 1);
		source.clip = zombieGrowls[i];
		source.PlayOneShot(source.clip);

	}

	/// <summary>
	/// Selects and play a random audioclip depennding on the GunType.
	/// </summary>
	/// <param name="source"></param>
	/// <param name="type"></param>
	public void PlayWeaponSound(AudioSource source, GunType type)
	{
		switch(type)
		{
			case GunType.Pistol:
				source.PlayOneShot(PistolAudio[Random.Range(0, PistolAudio.Length - 1)]);
				break;

			case GunType.Rifle:
				source.PlayOneShot(RifleAudio[Random.Range(0, PistolAudio.Length - 1)]);
				break;

			case GunType.Sniper:
				source.PlayOneShot(SniperAudio[Random.Range(0, PistolAudio.Length - 1)]);
				break;

			case GunType.Silenced:
				source.PlayOneShot(SilencedAudio[Random.Range(0, PistolAudio.Length - 1)]);
				break;

			default:
				break;
		}
	}
	#endregion
}
