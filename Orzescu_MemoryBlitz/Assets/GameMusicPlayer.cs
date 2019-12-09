using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicPlayer : MonoBehaviour {

    public AudioClip GameMusic;
    public AudioSource GameMusicSource;

	// Use this for initialization
	void Start () {

        GameMusicSource = gameObject.GetComponent<AudioSource>();
        GameMusicSource.volume = SoundManager.instance.musicVolume;
        SoundManager.instance.MenuMusicMaker = GameMusicSource;
        GameMusicSource.clip = GameMusic;
        GameMusicSource.Play();

    }
	
	// Update is called once per frame
	void Update () {


	}
}
