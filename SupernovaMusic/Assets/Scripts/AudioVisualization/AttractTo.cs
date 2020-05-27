using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractTo : MonoBehaviour
{
    Rigidbody _rigidbody;
    public Transform _attractedTo;
    public float _strengOfAttraction, _maxMagnitude;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if(_attractedTo!=null)
        {
            Vector3 direction = _attractedTo.position - transform.position;
            _rigidbody.AddForce(_strengOfAttraction * direction);

            if(_rigidbody.velocity.magnitude>_maxMagnitude)
            {
                _rigidbody.velocity =_rigidbody.velocity.normalized * _maxMagnitude;
            }
        }
    }
}
