using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DeadNation
{
    /// <summary>
    /// Bu sınıf test amaçlı düzelticem...
    /// </summary>
    public class BulletConfiguration : MonoBehaviour
    {
        [SerializeField] private GameObject m_ImpactBloodEffect;
        [SerializeField] private GameObject m_ImpactDirtEffect;
        [SerializeField] private GameObject m_ImpactMetalEffect;

        private Rigidbody _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.AddForce(transform.TransformDirection(Vector3.forward) * 1200, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision other)
        {
            string tag = other.gameObject.tag;
            switch (tag)
            {
                case "Zombie":
                    IDamageable damageable = other.transform.GetComponent<IDamageable>();
                    damageable?.TakeDamage(Random.Range(15, 40));
                    BulletImpact(m_ImpactBloodEffect);
                    break;
                case "Explosive":
                    IDamageable damageable2 = other.transform.GetComponent<IDamageable>();
                    damageable2?.TakeDamage(Random.Range(15, 40));
                    BulletImpact(m_ImpactMetalEffect);
                    break;
            }

            BulletImpact(m_ImpactDirtEffect);
        }

        private void BulletImpact(GameObject prefab)
        {
            GameObject vfx = Instantiate(prefab, transform.position, Quaternion.identity);
            Destroy(vfx, 1f);
            Destroy(gameObject);
        }
    }
}