using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
        MASTER
    }

    [Header("Type")]
    [SerializeField] private VolumeType volumeType;

    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = GetComponent<Slider>();
    }

    public void OnSliderVolumeChanged()
    {
        AudioController.instance.masterVolume = volumeSlider.value;
    }

    private void Update()
    {
        volumeSlider.value = AudioController.instance.masterVolume;
    }
}
