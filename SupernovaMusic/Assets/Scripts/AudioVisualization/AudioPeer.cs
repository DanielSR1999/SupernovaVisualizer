using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPeer : MonoBehaviour
{
    public AudioSource _audioSource;
    static float[] _samplesLeft = new float[512];
    static float[] _samplesRight = new float[512];

    float[] _freqBand = new float[8];
    float[] _bandBuffer = new float[8];
    float[] _bufferDecrease = new float[8];
    float[] _freqBandHighest = new float[8];
    //Audio 64
    float[] _freqBand64 = new float[64];
    float[] _bandBuffer64 = new float[64];
    float[] _bufferDecrease64 = new float[64];
    float[] _freqBandHighest64 = new float[64];
    [HideInInspector]
    public static float[] _audioBand,_audioBandBuffer;
    [HideInInspector]
    public static float[] _audioBand64, _audioBandBuffer64;

    [HideInInspector]
    public static float _Amplitude, _AmplitudeBuffer;
    float _AmplitudeHighest;
    public float _audioProfile;
    private static AudioPeer instance;

    public enum _channel
    {
        Stereo,
        left,
        right
    }
    public _channel channel = new _channel();

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance!=null)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        _audioBand= new float[8];
        _audioBandBuffer = new float[8];
        _audioBand64 = new float[64];
        _audioBandBuffer64 = new float[64];
        AudioProfile(_audioProfile);
    }
    private void OnLevelWasLoaded(int level)
    {
        if(level==2)
        {
            if(_audioSource.isPlaying)
            {
                _audioSource.Pause();
            }
           
        }
        if(level==1||level==0)
        {
            if (!_audioSource.isPlaying)
                {
                    _audioSource.UnPause();
                }
            else
            {
                return;
            }
        }
            
    }
    private void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        //MakeFrequencyBands64();
        BandBuffer();
        //BandBuffer64();
        CreateAudioBands();
        //CreateAudioBands64();
        GetAmplitude();
        AudioProfile(_audioProfile);
    }
    void AudioProfile(float audioProfile)
    {
        for(int i=0;i<8;i++)
        {
            _freqBandHighest[i] = _audioProfile;
        }
    }
    void GetAmplitude()
    {
        float _CurrentAmplitude = 0;
        float _CurrentAmplitudeBuffer = 0;
        for(int i=0;i<8;i++)
        {
            _CurrentAmplitude += _audioBand[i];
            _CurrentAmplitudeBuffer += _audioBandBuffer[i];
        }
        if(_CurrentAmplitude>_AmplitudeHighest)
        {
            _AmplitudeHighest = _CurrentAmplitude;
        }
        _Amplitude = _CurrentAmplitude / _AmplitudeHighest;
        _AmplitudeBuffer = _CurrentAmplitudeBuffer / _AmplitudeHighest;
    }
    void CreateAudioBands()
    {
        for(int i=0;i<8;i++)
        {
            if(_freqBand[i]>_freqBandHighest[i])
            {
                _freqBandHighest[i] = _freqBand[i];
            }
            _audioBand[i] = (_freqBand[i] / _freqBandHighest[i]);
            _audioBandBuffer[i] = (_bandBuffer[i] / _freqBandHighest[i]);
        }
    }
    void CreateAudioBands64()
    {
        for (int i = 0; i < 64; i++)
        {
            if (_freqBand64[i] > _freqBandHighest64[i])
            {
                _freqBandHighest64[i] = _freqBand64[i];
            }
            _audioBand64[i] = (_freqBand64[i] / _freqBandHighest64[i]);
            _audioBandBuffer64[i] = (_bandBuffer64[i] / _freqBandHighest64[i]);
        }
    }
    void BandBuffer()
    {
        for(int g=0;g<8;g++)
        {
            if(_freqBand[g]>_bandBuffer[g])
            {
                _bandBuffer[g] = _freqBand[g];
                _bufferDecrease[g]= 0.005f;
            }
            if(_freqBand[g]<_bandBuffer[g])
            {
                _bandBuffer[g] -= _bufferDecrease[g];
                _bufferDecrease[g] *= 1.2f;
            }
        }
    }
    void BandBuffer64()
    {
        for (int g = 0; g < 64; g++)
        {
            if (_freqBand64[g] > _bandBuffer64[g])
            {
                _bandBuffer64[g] = _freqBand64[g];
                _bufferDecrease64[g] = 0.005f;
            }
            if (_freqBand64[g] < _bandBuffer64[g])
            {
                _bandBuffer64[g] -= _bufferDecrease64[g];
                _bufferDecrease64[g] *= 1.2f;
            }
        }
    }
    void MakeFrequencyBands()
    {
        int count = 0;

        for(int i=0;i<8;i++)
        {
            float average= 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if(i==7)
            {
                sampleCount += 2;
            }
            for(int j=0;j<sampleCount;j++)
            {
                if(channel==_channel.Stereo)
                {
                    average += (_samplesLeft[count] + _samplesRight[count]) * (count + 1);
                }
                if (channel == _channel.left)
                {
                    average += _samplesLeft[count] * (count + 1);
                }
                if (channel == _channel.right)
                {
                    average += _samplesRight[count] * (count + 1);
                }

                count++;
            }
            average /= count;

            _freqBand[i] = average * 10;
        }

    }
    void MakeFrequencyBands64()
    {
        int count = 0;
        int sampleCount = 1;
        int power = 0;

        for (int i = 0; i < 64; i++)
        {
            float average = 0;
           
            if (i == 16 || i == 32||i == 40 || i == 48|| i == 56)
            {
                power++;
                sampleCount = (int)Mathf.Pow(2,power)*2;
                if(power==3)
                {
                    sampleCount -= 2;
                }
            }
            for (int j = 0; j < sampleCount; j++)
            {
                if (channel == _channel.Stereo)
                {
                    average += (_samplesLeft[count] + _samplesRight[count]) * (count + 1);
                }
                if (channel == _channel.left)
                {
                    average += _samplesLeft[count] * (count + 1);
                }
                if (channel == _channel.right)
                {
                    average += _samplesRight[count] * (count + 1);
                }

                count++;
            }
            average /= count;

            _freqBand64[i] = average * 80;
        }

    }
    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samplesLeft,0,FFTWindow.Blackman);
        _audioSource.GetSpectrumData(_samplesRight, 0, FFTWindow.Blackman);
    }
}
