using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private Slider sliderMUSIC;

    private void OnEnable()
    {
        LoadPlayerPrefs();
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetFloat("SliderMusicValue", sliderMUSIC.value);
        PlayerPrefs.SetFloat("SliderSFXValue", sliderSFX.value);

        AkSoundEngine.SetRTPCValue("MUSIC", sliderMUSIC.value);
        AkSoundEngine.SetRTPCValue("SFX", sliderSFX.value);

        PlayerPrefs.Save();
    }

    public void LoadPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("SliderSFXValue"))
        {
            PlayerPrefs.SetFloat("SliderSFXValue", 0.5f);
        }

        sliderSFX.value = PlayerPrefs.GetFloat("SliderSFXValue");

        if (!PlayerPrefs.HasKey("SliderMusicValue"))
        {
            PlayerPrefs.SetFloat("SliderMusicValue", 0.5f);
        }

        sliderMUSIC.value = PlayerPrefs.GetFloat("SliderMusicValue");

        AkSoundEngine.SetRTPCValue("MUSIC", sliderMUSIC.value);
        AkSoundEngine.SetRTPCValue("SFX", sliderSFX.value);
    }
}
