using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColliderEvent : MonoBehaviour
{
    public event Action<Collision> onCollisionEnter;
    public event Action<Collision> onCollisionExit;
    public event Action<Collision> onCollisionStay;
    
    public event Action<Collider> onTriggerEnter;
    public event Action<Collider> onTriggerStay;
    public event Action<Collider> onTriggerExit;

    void OnCollisionEnter(Collision collision)
    {
        onCollisionEnter?.Invoke(collision);
    }
    void OnCollisionExit(Collision collision)
    {
        onCollisionExit?.Invoke(collision);
    }
    void OnCollisionStay(Collision collision)
    {
        onCollisionStay?.Invoke(collision);
    }
    void OnTriggerEnter(Collider other)
    {
        onTriggerEnter?.Invoke(other);
    }
    void OnTriggerExit(Collider other)
    {
        onTriggerExit?.Invoke(other);
    }
    void OnTriggerStay(Collider other)
    {
        onTriggerStay?.Invoke(other);
    }
}
