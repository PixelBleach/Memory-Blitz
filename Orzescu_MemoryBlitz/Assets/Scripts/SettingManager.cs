using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;


public class SettingManager : MonoBehaviour {

    public static SettingManager instance;

    public Toggle fullscreenToggle;
    public Toggle twoPlayerMode;
    public Toggle isMapOfDay;
    public Dropdown resolutionDropdown;
    public Dropdown textureQualityDropdown;
    public Dropdown antialisasingDropdown;
    public Dropdown vSyncDropdown;
    public Slider musicVolumeSlider;
    public Slider SFXVolumeSlider;
    public Button applyButton;

    public AudioSource musicSource;
    public AudioSource SFXSource;
    public Resolution[] resolutionArray;
    public GameSettings gameSettings;

    void OnStart()
    {
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

    void OnEnable()
    {

        gameSettings = new GameSettings();

        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); } );
        twoPlayerMode.onValueChanged.AddListener(delegate { OnTwoPlayerToggle(); });
        isMapOfDay.onValueChanged.AddListener(delegate { OnMapOfDay(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        antialisasingDropdown.onValueChanged.AddListener(delegate { OnAntialiasingChange(); });
        vSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });
        SFXVolumeSlider.onValueChanged.AddListener(delegate { OnSFXVolumeChange(); });
        applyButton.onClick.AddListener(delegate { OnApplyButtonClick(); });

        resolutionArray = Screen.resolutions;

        foreach(Resolution res in resolutionArray)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(res.width.ToString() + "x" + res.height.ToString()));
        }

        LoadSettings();

    }

    //Methods

    public void OnFullscreenToggle()
    {
        gameSettings.fullscreen = Screen.fullScreen = fullscreenToggle.isOn;
    }

    public void OnTwoPlayerToggle()
    {
        gameSettings.twoPlayerMode = GameManager.instance.isTwoPlayer = twoPlayerMode.isOn;
    }

    public void OnMapOfDay()
    {
        gameSettings.isMapOfDay = GameManager.instance.isMapOfDay = isMapOfDay.isOn;
    }

    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutionArray[resolutionDropdown.value].width, resolutionArray[resolutionDropdown.value].height, Screen.fullScreen);
        gameSettings.resolutionIndex = resolutionDropdown.value;
    }

    public void OnTextureQualityChange()
    {
        QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureQualityDropdown.value;
    }

    public void OnAntialiasingChange()
    {
        QualitySettings.antiAliasing = gameSettings.antialiasing = (int)Mathf.Pow(2, antialisasingDropdown.value);
    }

    public void OnVSyncChange()
    {
        QualitySettings.vSyncCount = gameSettings.vSync = vSyncDropdown.value;
    }

    public void OnMusicVolumeChange()
    {
        musicSource.volume = gameSettings.musicVolume = musicVolumeSlider.value;
    }

    public void OnSFXVolumeChange()
    {
        SFXSource.volume = gameSettings.SFXVolume = SFXVolumeSlider.value;
    }

    public void OnApplyButtonClick()
    {
        SaveSettings();
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
    }

    public void LoadSettings()
    {
        gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));

        musicVolumeSlider.value = gameSettings.musicVolume;
        antialisasingDropdown.value = gameSettings.antialiasing;
        vSyncDropdown.value = gameSettings.vSync;
        textureQualityDropdown.value = gameSettings.textureQuality;
        resolutionDropdown.value = gameSettings.resolutionIndex;
        fullscreenToggle.isOn = gameSettings.fullscreen;
        isMapOfDay.isOn = gameSettings.isMapOfDay;
        twoPlayerMode.isOn = gameSettings.twoPlayerMode;
        SFXVolumeSlider.value = gameSettings.SFXVolume;
        resolutionDropdown.RefreshShownValue();

    }

}
