using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Collections;
using LootLocker.Requests;

public class MainMenu : MonoBehaviour
{
    #region Vars & Refs
    [SerializeField] private GameObject[] windows;

    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TMP_Text masterValueText;
    [SerializeField] private TMP_Text musicValueText;
    [SerializeField] private TMP_Text sfxValueText;

    [SerializeField] private Toggle postProcessToggle;
    [SerializeField] private Toggle camShakeToggle;
    [SerializeField] private Toggle vibrateToggle;

    private Resolution[] resolutions;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingSlider;
    #endregion

    #region Functions

    #region INIT
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        #region Set SettingsToggle values when going to mainMenu
        postProcessToggle.isOn = playerStats.postProcessingEnabled;
        camShakeToggle.isOn = playerStats.cameraShakeEnabled;
        vibrateToggle.isOn = playerStats.vibrationsEnabled;
        #endregion

        #region Set VolumeSlider values when going to mainMenu
        mainMixer.GetFloat("MasterVolume", out float masterFloat);
        mainMixer.GetFloat("MusicVolume", out float musicFloat);
        mainMixer.GetFloat("SFXVolume", out float sfxFloat);
        //Mathf.Pow inverts MathfLog10 for the correct output value
        masterSlider.value = Mathf.Pow(10, masterFloat);
        musicSlider.value = Mathf.Pow(10, musicFloat);
        sfxSlider.value = Mathf.Pow(10, sfxFloat);
        #endregion

        #region Show avaiable Resolutions on the dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            resolutionOptions.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        #endregion
    }
    #endregion

    #region ToggleWindows
    public void ToggleWindow(int indexInArray)
    {
        for (int i = 0; i < windows.Length; i++)
        {
            if (i == indexInArray)
            {
                //Toggle the selected window
                windows[i].SetActive(!windows[i].activeSelf);
            }
            else
            {
                //Deactivate every other window
                windows[i].SetActive(false);
            }
        }
    }
    #endregion

    #region Play
    public void Play(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            loadingSlider.value = operation.progress;
            yield return null;
        }
    }
    #endregion

    #region Settings
    #region Audio
    public void SetMasterVolume(float volume)
    {
        //Mathf.Log10 makes it smooth & linear
        mainMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        mainMixer.GetFloat("MasterVolume", out float masterFloat);
        masterValueText.text = masterFloat.ToString("0") + " dB";
    }
    public void SetMusicVolume(float volume)
    {
        //Mathf.Log10 makes it smooth & linear
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        mainMixer.GetFloat("MusicVolume", out float musicFloat);
        musicValueText.text = musicFloat.ToString("0") + " dB";
    }
    public void SetSFXVolume(float volume)
    {
        //Mathf.Log10 makes it smooth & linear
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        mainMixer.GetFloat("SFXVolume", out float sfxFloat);
        sfxValueText.text = sfxFloat.ToString("0") + " dB";
    }
    #endregion
    #region Video
    public void TogglePostProcessing(bool enabled)
    {
        playerStats.postProcessingEnabled = enabled;
    }
    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }
    public void LimitFPS(string desiredFPS)
    {
        int defaultValue = 60;
        int fps = defaultValue;
        int.TryParse(desiredFPS, out fps);
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = fps;
    }
    #endregion
    #region Gameplay
    public void ToggleCameraShake(bool enabled)
    {
        playerStats.cameraShakeEnabled = enabled;
    }
    public void ToggleVibrations(bool enabled)
    {
        playerStats.vibrationsEnabled = enabled;
    }
    #endregion
    #endregion

    #endregion
}