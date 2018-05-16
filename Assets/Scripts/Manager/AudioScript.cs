using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour {
public static AudioScript instance;
private AudioSource audioSource;
	// Use this for initialization
	void Start () {
		instance=this;
		audioSource=GetComponent<AudioSource>();
	}
	public void PlayOneShot(AudioClip audioClip,int volume){
audioSource.PlayOneShot(audioClip,volume);
	}
public void PlayOneShot(AudioClip audioClip){
audioSource.PlayOneShot(audioClip);
	}
}
