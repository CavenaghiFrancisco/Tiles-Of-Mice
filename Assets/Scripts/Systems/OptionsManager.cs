using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private Slider sliderMUSIC;

    public void SaveMusicPlayerPrefs()
    {
        PlayerPrefs.SetFloat("SliderMusicValue", sliderMUSIC.value);

        AkSoundEngine.SetRTPCValue("MUSIC", sliderMUSIC.value * 100);

        PlayerPrefs.Save();
    }

    public void SaveSFXPlayerPrefs()
    {
        PlayerPrefs.SetFloat("SliderSFXValue", sliderSFX.value);

        AkSoundEngine.SetRTPCValue("SFX", sliderSFX.value * 100);

        PlayerPrefs.Save();
    }

    public void LoadPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("SliderSFXValue"))
        {
            PlayerPrefs.SetFloat("SliderSFXValue", 0.5f);
        }
        if (!PlayerPrefs.HasKey("SliderMusicValue"))
        {
            PlayerPrefs.SetFloat("SliderMusicValue", 0.5f);
        }

        sliderSFX.value = PlayerPrefs.GetFloat("SliderSFXValue");
        sliderMUSIC.value = PlayerPrefs.GetFloat("SliderMusicValue");

        AkSoundEngine.SetRTPCValue("MUSIC", sliderMUSIC.value * 100);
        AkSoundEngine.SetRTPCValue("SFX", sliderSFX.value * 100);
    }

    public static void SetVolumes()
    {
        AkSoundEngine.SetRTPCValue("MUSIC", PlayerPrefs.GetFloat("SliderMusicValue", 0.5f) * 100);
        AkSoundEngine.SetRTPCValue("SFX", PlayerPrefs.GetFloat("SliderSFXValue", 0.5f) * 100);
    }
}
