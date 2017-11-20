using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager  {
	private static AudioManager _p;

	public static AudioManager GetInstance()
	{
		if (_p == null)
			_p = new AudioManager();
		return _p;
	}

	public void PlayClip(AudioSource Source, string AudioName, bool isLoop)
	{
		if (Source != null)
		{
			Source.clip = Resources.Load(AudioName) as AudioClip;
			if (Source.clip == null)
			{ 
				Debug.Log("音频为空");
				return;
			}
			Source.loop = isLoop;
			Source.Play();
		}
	}
}
