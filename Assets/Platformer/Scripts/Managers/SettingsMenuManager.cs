using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class SettingsMenuManager : MonoBehaviour
{
    
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Music, Effects and UI")]
    [SerializeField] private GameObject MusicSliderOn;
    [SerializeField] private GameObject MusicSliderOff;
    [SerializeField] private GameObject EffectsSliderOn;
    [SerializeField] private GameObject EffectsSliderOff;
    [SerializeField] private GameObject UISliderOn;
    [SerializeField] private GameObject UISliderOff;

    private Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;

            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        #region LoadSettings

        //Load Master Volume
        if(PlayerPrefs.HasKey("Volume"))
        {
            audioMixer.SetFloat("Volume", PlayerPrefs.GetFloat("Volume"));
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        }
        else
        {
            volumeSlider.value = 0f;
        }

        //Load Quality Level
        if(PlayerPrefs.HasKey("Quality"))
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality"));
            qualityDropdown.value = PlayerPrefs.GetInt("Quality");
        }
        else
        {
            qualityDropdown.value = 5;
        }

        //Load Fullscreen
        if(PlayerPrefs.HasKey("Fullscreen"))
        {
            Screen.fullScreen = bool.Parse(PlayerPrefs.GetString("Fullscreen"));
            fullscreenToggle.isOn = bool.Parse(PlayerPrefs.GetString("Fullscreen"));
        }
        else
        {
            Screen.fullScreen = true;
            fullscreenToggle.isOn = true;
        }

        //Load Music Slider
        if(PlayerPrefs.HasKey("MusicSounds"))
        {
            audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicSounds"));

            if(PlayerPrefs.GetFloat("MusicSounds") == 0f)
            {
                MusicSliderOn.SetActive(true);
                MusicSliderOff.SetActive(false);
            }
            else
            {
                MusicSliderOn.SetActive(false);
                MusicSliderOff.SetActive(true);
            }
        }
        else
        {
            MusicSliderOn.SetActive(true);
            MusicSliderOff.SetActive(false);
        }

        //Load Effects Slider
        if(PlayerPrefs.HasKey("EffectsSounds"))
        {
            audioMixer.SetFloat("EffectsVolume", PlayerPrefs.GetFloat("EffectsSounds"));

            if(PlayerPrefs.GetFloat("EffectsSounds") == 0f)
            {
                EffectsSliderOn.SetActive(true);
                EffectsSliderOff.SetActive(false);
            }
            else
            {
                EffectsSliderOn.SetActive(false);
                EffectsSliderOff.SetActive(true);
            }
        }
        else
        {
            EffectsSliderOn.SetActive(true);
            EffectsSliderOff.SetActive(false);
        }

        //Load UI Slider
        if(PlayerPrefs.HasKey("UISounds"))
        {
            audioMixer.SetFloat("UIVolume", PlayerPrefs.GetFloat("UISounds"));

            if(PlayerPrefs.GetFloat("UISounds") == 0f)
            {
                UISliderOn.SetActive(true);
                UISliderOff.SetActive(false);
            }
            else
            {
                UISliderOn.SetActive(false);
                UISliderOff.SetActive(true);
            }
        }
        else
        {
            UISliderOn.SetActive(true);
            UISliderOff.SetActive(false);
        }
        #endregion

    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetString("Fullscreen", isFullscreen.ToString());
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

#region Sounds
    public void MusicSoundsOn()
    {
        audioMixer.SetFloat("MusicVolume", 0f);
        PlayerPrefs.SetFloat("MusicSounds", 0f);
    }

    public void MusicSoundsOff()
    {
        audioMixer.SetFloat("MusicVolume", -80f);
        PlayerPrefs.SetFloat("MusicSounds", -80f);
    }

    public void EffectSoundsOn()
    {
        audioMixer.SetFloat("EffectsVolume", 0f);
        PlayerPrefs.SetFloat("EffectsSounds", 0f);
    }

    public void EffectSoundsOff()
    {
        audioMixer.SetFloat("EffectsVolume", -80f);
        PlayerPrefs.SetFloat("EffectsSounds", -80f);
    }

    public void UISoundsOn()
    {
        audioMixer.SetFloat("UIVolume", 0f);
        PlayerPrefs.SetFloat("UISounds", 0f);
    }

    public void UISoundsOff()
    {
        audioMixer.SetFloat("UIVolume", -80f);
        PlayerPrefs.SetFloat("UISounds", -80f);
    }
#endregion
}
