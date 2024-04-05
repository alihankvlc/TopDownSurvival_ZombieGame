using UnityEngine;

namespace DeadNation
{
    [System.Serializable]
    public class FirearmMuzzle
    {
        [SerializeField] private Transform _muzzleTransform;
        [SerializeField] private ParticleSystem _muzzleEffect;

        public Transform MuzzleTransform => _muzzleTransform;
        public ParticleSystem MuzzleEffect => _muzzleEffect;
    }
}