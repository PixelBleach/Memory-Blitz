using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;

    [Header("Data To Be Carried to Other Scenes")]
    public float musicVolume;
    public float sfxVolume;

    [Header("Game Sounds")]
    public AudioClip damagedSound;
    public AudioClip attackSound;
    public AudioClip powerUpSound;
    public AudioClip deathSound;

    [Header("UI Sounds")]
    public AudioClip UIEnterSound;
    public AudioClip UISelectSound;

    public AudioSource UISoundMaker;
    public AudioSource MenuMusicMaker;

	// Use this for initialization
	void Start () {
        //This'll ensure only 1 gamemanager object is in a scene at any time
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else {
            instance = this.gameObject.GetComponent<SoundManager>();
            DontDestroyOnLoad(gameObject);
        }
	}

    void Update()
    {
        if (MenuMusicMaker != null && UISoundMaker != null)
        {
            musicVolume = MenuMusicMaker.volume;
            sfxVolume = UISoundMaker.volume;
        }

    }

    public void PlayUIEnterSound()
    {
        UISoundMaker.clip = UIEnterSound;
        UISoundMaker.Play();
    }

    public void PlayUISelectSound()
    {
        UISoundMaker.clip = UISelectSound;
        UISoundMaker.Play();
    }

}
