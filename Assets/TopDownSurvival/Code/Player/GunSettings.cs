using _Project.Common.ItemSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace DeadNation
{
    [System.Serializable]
    public class GunSettings
    {
#if UNITY_EDITOR
        [SerializeField] private string _name;
#endif
        [SerializeField] private Firearm _firearm;
        [SerializeField] private FirearmRigSettings _rigSettings;
        [SerializeField] private FirearmMuzzle _muzzle;

        public Firearm Firearm => _firearm;
        public FirearmRigSettings RiggSettings => _rigSettings;
        public FirearmMuzzle Muzzle => _muzzle;

        public void Shot(GameObject bulletPrefab, Vector3 lookAtPosition, AudioClip clip)
        {
            if (_muzzle != null && _muzzle.MuzzleTransform != null)
            {
                GameObject bullet = Object.Instantiate(bulletPrefab, _muzzle.MuzzleTransform.position, Quaternion.identity);
                bullet.transform.LookAt(lookAtPosition);
                _muzzle.MuzzleEffect.Play();

                AudioSource.PlayClipAtPoint(clip, _muzzle.MuzzleTransform.position, 1f);
            }
        }

        public void SetActiveSettings(bool isActiveMode)
        {
            if (isActiveMode)
            {
                _rigSettings.Model.SetActive(isActiveMode);
                _rigSettings.Load();
                return;
            }

            _rigSettings.Model.SetActive(false);
        }

        private void Load(bool isActiveMode)
        {
            _rigSettings.Model.SetActive(isActiveMode);
            _rigSettings.Load();
        }
    }
}