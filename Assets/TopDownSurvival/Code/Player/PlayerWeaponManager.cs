using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DeadNation
{
    public interface IWeaponHandler
    {
        public GunSettings GetWeaponSettings(int id);
    }

    public class PlayerWeaponManager : Singleton<PlayerWeaponManager>, IWeaponHandler
    {
        [SerializeField] private List<GunSettings> _weapons = new();

        private Dictionary<int, GunSettings> _weaponCache = new();


        private void Start()
        {
            _weaponCache = _weapons.ToDictionary(r => r.Firearm.Id);
        }

        public GunSettings GetWeaponSettings(int id)
        {
            if (_weaponCache.TryGetValue(id, out GunSettings existingGunSettings))
            {
                return existingGunSettings;
            }

            Debug.Log("Null...");
            return null;
        }
    }
}