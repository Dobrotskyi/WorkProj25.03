using Code.Game;
using System;
using UnityEngine;

public class DummyCollisionHandler : MonoBehaviour
{
    public event Action Entered;
    [SerializeField] private ParticleSystem _explosionEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Ghost.IsActive)
            return;
        Entered?.Invoke();
        if (_explosionEffect != null)
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
    }
}
