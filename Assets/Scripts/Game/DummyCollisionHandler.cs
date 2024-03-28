using System;
using UnityEngine;

public class DummyCollisionHandler : MonoBehaviour
{
    public event Action Entered;
    [SerializeField] private ParticleSystem _explosionEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entered?.Invoke();
        if (_explosionEffect != null)
            Instantiate(_explosionEffect, collision.transform.position, Quaternion.identity);
    }
}
