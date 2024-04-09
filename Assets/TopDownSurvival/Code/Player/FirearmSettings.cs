using _Project.Common.ItemSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace DeadNation
{
    [System.Serializable]
    public class FirearmSettings : WeaponSettings
    {
#if UNITY_EDITOR
        [SerializeField] private string _name;
#endif
        [SerializeField] private FirearmMuzzle _muzzle;
        public FirearmMuzzle Muzzle => _muzzle;

        public override WeaponSettingsType SettingsType { get; protected set; } = WeaponSettingsType.Firearm;

        public void Shot(GameObject bulletPrefab, Vector3 lookAtPosition, AudioClip clip)
        {
            if (_muzzle != null && _muzzle.MuzzleTransform != null)
            {
                GameObject bullet =
                    Object.Instantiate(bulletPrefab, _muzzle.MuzzleTransform.position, Quaternion.identity);
                bullet.transform.LookAt(lookAtPosition);
                _muzzle.MuzzleEffect.Play();

                AudioSource.PlayClipAtPoint(clip, _muzzle.MuzzleTransform.position, 1f);
            }
        }
    }
}