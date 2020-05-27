using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TangentCircles : CircleTangent
{
    [Header("Setup")]
    public GameObject _circlePrefab;
    private GameObject _innerCircleGO, _outterCircleGO;
    private Vector4 _innerCircle, _outterCircle;
    public float _innerCircleRadius, _outterCircleRadius;
    private Vector4[] _tangentCircle;
    private GameObject[] _tangentObject;
    [Range(1, 64)]
    public int _circleAmount;

    [Header("Input")]
    [Range(0, 1)]
    public float _disOutterTangent;
    [Range(0, 1)]
    public float movement_Smooth;
    [Range(0, 10)]
    public float _radiusChangeSpeed;
    private Vector2 _tsL, _tsLSmooth;
    private float _radiusChange;

    [Header("AudioVisuals")]
    public AudioPeer _audioPeer;
    public Material _materialBase;
    private Material[] _material;
    public Gradient _gradient;
    public float _emissionMultiplier;
    public bool _emissionBuffer;
    [Range(0, 1)]
    public float _thresholdEmission;
    



    private void Start()
    {
        _innerCircle = new Vector4(0, 0, 0, _innerCircleRadius);
        _outterCircle = new Vector4(0, 0, 0, _outterCircleRadius);

        _tangentCircle= new Vector4[_circleAmount];
        _tangentObject = new GameObject[_circleAmount];
        _material = new Material[_circleAmount];

        for(int i=0;i<_circleAmount;i++)
        {
            GameObject tangentInstance = (GameObject)Instantiate(_circlePrefab);
            _tangentObject[i] = tangentInstance;
            _tangentObject[i].transform.parent=this.transform;
            _material[i] = new Material(_materialBase);
            //_material.EnabledKeyWords("_EMISSION");
            _tangentObject[i].GetComponent<MeshRenderer>().material = _material[i];
        }
    }
    void PlayerInput()
    {
        _tsL = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _tsLSmooth = new Vector2(
            _tsLSmooth.x * (1 - movement_Smooth) + _tsL.x * movement_Smooth,
            _tsLSmooth.y * (1 - movement_Smooth) + _tsL.y * movement_Smooth);

        _radiusChange = Input.GetAxis("Horizontal2") - Input.GetAxis("Vertical2");

        _innerCircle = new Vector4(
            (_tsLSmooth.x * (_outterCircle.w - _innerCircle.w) * (1 - _disOutterTangent)) + _outterCircle.x,
        0.0f,
        (_tsLSmooth.y * (_outterCircle.w - _innerCircle.w) * (1 - _disOutterTangent)) + _outterCircle.z,
        _innerCircle.w + (_radiusChange * Time.deltaTime + _radiusChangeSpeed));
        
    }
    private void Update()
    {
        PlayerInput();
        
        for (int i=0;i<_circleAmount;i++)
        {
            _tangentCircle[i] = FindTangentCircle(_outterCircle, _innerCircle, (360 / _circleAmount) * i);
            _tangentObject[i].transform.position = new Vector3(_tangentCircle[i].x, _tangentCircle[i].y, _tangentCircle[i].z);
            _tangentObject[i].transform.localScale = new Vector3(_tangentCircle[i].w, _tangentCircle[i].w, _tangentCircle[i].w) * 2;
            //if(_audioPeer._audioBandBuffer64[i]>_thresholdEmission)
            //{

            //}
            //else
            //{
            //    _material[i].SetColor("_EmissionColor", new Color(0, 0, 0));
            //}
        }
    }
}
