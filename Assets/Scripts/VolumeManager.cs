using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [Header("Referencias")]
    public AudioMixer mainMixer;

   
    public string parameterName = "MasterVolume";

    public void SetVolume(float sliderValue)
    {
       
        float volumeInDb = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20;

        mainMixer.SetFloat(parameterName, volumeInDb);
    }
}