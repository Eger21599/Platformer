using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class SettingsMenuManager : MonoBehaviour
{
    
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Dropdown resolutionDropdown;

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
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

#region Sounds
    public void EffectSoundsOn()
    {
        audioMixer.SetFloat("EffectsVolume", 0f);
    }

    public void EffectSoundsOff()
    {
        audioMixer.SetFloat("EffectsVolume", -80f);
    }

    public void MusicSoundsOn()
    {
        audioMixer.SetFloat("MusicVolume", 0f);
    }

    public void MusicSoundsOff()
    {
        audioMixer.SetFloat("MusicVolume", -80f);
    }

    public void UISoundsOn()
    {
        audioMixer.SetFloat("UIVolume", 0f);
    }

    public void UISoundsOff()
    {
        audioMixer.SetFloat("UIVolume", -80f);
    }
#endregion
}
