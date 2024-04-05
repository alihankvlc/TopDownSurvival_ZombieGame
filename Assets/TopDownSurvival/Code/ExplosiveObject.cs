using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace DeadNation
{
    public class ExplosiveObject : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _explosionMinForce;
        [SerializeField] private float _explosionMaxForce;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private int _damage;
        [SerializeField] private float _maxHealth;

        [SerializeField] private GameObject _destroyObjectForm;
        [SerializeField] private GameObject _explosiveObject;
        [SerializeField] private GameObject _burnObjectEffect;

        [SerializeField] private CinemachineImpulseSource _cinemachineImpulseSrc;
        private Rigidbody _rigidbody;

        private float _currentHealth;
        private float _explosionForce;

        private bool _isAlive;

        private const int _explosionDuration = 3;

        private void OnEnable()
        {
            _currentHealth = _maxHealth;
            _explosionForce = Random.Range(_explosionMinForce, _explosionMaxForce);
            _isAlive = true;
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _cinemachineImpulseSrc = GetComponent<CinemachineImpulseSource>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                TakeDamage(20);
            }
        }

        public void TakeDamage(int amount)
        {
            _currentHealth = Mathf.Max(0, _currentHealth - amount);
            if (_isAlive)
            {
                if (_currentHealth == 0) Explosion();
            }
        }

        private void Explosion()
        {
            _isAlive = false;
            _burnObjectEffect.SetActive(true);
            StartCoroutine(ExplosionDelay());
        }

        private IEnumerator ExplosionDelay()
        {
            yield return new WaitForSeconds(_explosionDuration);

            _rigidbody.isKinematic = false;

            _rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, 0f,
                ForceMode.Impulse);


            _cinemachineImpulseSrc.GenerateImpulse();
            _explosiveObject.SetActive(false);
            _destroyObjectForm.SetActive(true);

            DamageableObject();
        }

        private void DamageableObject()
        {
            Collider[] _collider = Physics.OverlapSphere(transform.position, _explosionRadius);

            if (_collider.Length != 0 && _collider != null)
            {
                foreach (var col in _collider)
                {
                    IDamageable damageable = col.GetComponent<IDamageable>();
                    
                    if (damageable != null)
                        damageable.TakeDamage(_damage);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
    }
}