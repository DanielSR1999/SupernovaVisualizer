using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVisualsController : MonoBehaviour
{
    public ScaleOnAmplitude _scaleOnAmplitude;
    public Slider _audioProfileSlider;
    public Color enable, disable;
    public Image buttonImage;
    private void Start()
    {
        AudioPeer._audioProfile= _audioProfileSlider.value;
    }
    public void Enable_DisableScaleEffect()
    {
        if(_scaleOnAmplitude.enabled==true)
        {
            _scaleOnAmplitude.enabled = false;
            buttonImage.color = disable;
        }
        else
        {
            _scaleOnAmplitude.enabled = true;
            buttonImage.color = enable;
        }
    }
    public void ChangeAudioProfile()
    {
        AudioPeer._audioProfile = _audioProfileSlider.value;
    }
}
