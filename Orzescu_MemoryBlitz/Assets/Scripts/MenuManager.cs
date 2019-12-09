using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public static MenuManager instance;

    public Canvas GUICanvas;
    public Canvas startMenu;
    public Canvas instructionsMenu;
    public Canvas winMenu;
    public Canvas loseMenu;
    public Canvas optionsMenu;

	void Awake () {

        //This'll ensure only 1 gamemanager object is in a scene at any time
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
	}

    void Start()
    {
        optionsMenu.enabled = false;
        instructionsMenu.enabled = false;
        startMenu.enabled = true;
    }
	
    public void StartGame()
    {
        SoundManager.instance.MenuMusicMaker.Stop();
        SceneManager.LoadScene(1);

    }

    public void BackToMainMenuFromInstructions()
    {     
        startMenu.enabled = true;
        instructionsMenu.enabled = false;
    }

    public void BackToMainMenuFromOptions()
    {
        optionsMenu.enabled = false;
        startMenu.enabled = true;
    }

    public void OpenInstructionsMenu()
    {
        startMenu.enabled = false;
        instructionsMenu.enabled = true;
    }

    public void OpenOptionsMenu()
    {
        optionsMenu.enabled = true;
        startMenu.enabled = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

}
