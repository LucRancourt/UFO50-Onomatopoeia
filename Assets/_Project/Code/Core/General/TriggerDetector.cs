using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public class TriggerDetector : MonoBehaviour
{
    // Variables
    private Collider _collider;
    private Rigidbody _rigidbody;

    public event Action<Collider> OnTriggerEnterDetected;
    public event Action<Collider> OnTriggerExitDetected;


    // Functions
    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;

        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterDetected?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitDetected?.Invoke(other);
    }
}
