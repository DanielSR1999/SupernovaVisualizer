using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnAmplitude : MonoBehaviour
{
    public float _startScale, _maxScale;
    public bool useBufer;
    Material _material;
    public float _red, _green, _blue;

    private void Start()
    {
        if(_material!=null)
            _material = GetComponent<MeshRenderer>().material;
    }
    private void Update()
    {
        if (!useBufer)
        {
            transform.localScale = new Vector3((AudioPeer._Amplitude * _maxScale) + _startScale, (AudioPeer._Amplitude * _maxScale)+_startScale,(AudioPeer._Amplitude*_maxScale)+_startScale);
            Color color = new Color(_red * AudioPeer._Amplitude, _green * AudioPeer._Amplitude, _blue* AudioPeer._Amplitude);
            if (_material != null)
                _material.SetColor("_EmissionColor", color);
        }
        if (useBufer)
        {
            transform.localScale = new Vector3((AudioPeer._AmplitudeBuffer * _maxScale) + _startScale, (AudioPeer._AmplitudeBuffer * _maxScale) + _startScale, (AudioPeer._AmplitudeBuffer * _maxScale)+_startScale);
            Color color = new Color(_red * AudioPeer._AmplitudeBuffer, _green * AudioPeer._AmplitudeBuffer, _blue * AudioPeer._AmplitudeBuffer);
            if (_material != null)
                _material.SetColor("_EmissionColor", color);
        }
    }
}
