using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighOnAudio : MonoBehaviour
{
    public int _band;
    public float _maxIntensity, _minIntensity;
    Light _light;
    ReflectionProbe _reflectionProbe;
    public enum function { _Light,_ReflectionProbe}
    public function _function;
     
    private void Start()
    {
        switch (_function)
        {
            case function._Light:
                {
                    _light = GetComponent<Light>();
                    break;
                }
            case function._ReflectionProbe:
                {
                    _reflectionProbe = GetComponent<ReflectionProbe>();
                    break;
                }
        }   
    }
    private void Update()
    {
        switch (_function)
        {
            case function._Light:
                {
                    _light.intensity = (AudioPeer._audioBandBuffer[_band] * (_maxIntensity - _minIntensity)) + _minIntensity;
                    break;
                }
            case function._ReflectionProbe:
                {
                    _reflectionProbe.intensity = (AudioPeer._audioBandBuffer[_band] * (_maxIntensity - _minIntensity)) + _minIntensity;
                    break;
                }
        }
        
    }
}
