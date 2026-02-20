using System;
using UnityEngine;

public class SphereForShield : MonoBehaviour
{
    public event Action<SphereForShield> OnDestroyed;

    public void DestroyObject()
    {
        OnDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
}