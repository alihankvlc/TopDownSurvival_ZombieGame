using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Common.ItemSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DeadNation
{
    public interface IWeaponHandler
    {
        public Weapon GetWeaponSettings(int id, bool isEquip);
    }

    public class PlayerWeaponManager : Singleton<PlayerWeaponManager>, IWeaponHandler
    {
        [SerializeField] private List<Weapon> _weapons = new();

        [Inject] private IEquippableWeapon _equippableWeaponHandler;
        private Dictionary<int, Weapon> _weaponSettingsCache = new();


        private void Start()
        {
            _weaponSettingsCache = _weapons.ToDictionary(r => r.Settings.Data.Id);
        }

        public Weapon GetWeaponSettings(int id, bool isEquip)
        {
            if (_weaponSettingsCache.TryGetValue(id, out Weapon existingGunSettings))
            {
                existingGunSettings.gameObject.SetActive(isEquip);
                _equippableWeaponHandler.EquipWeapon(isEquip ? existingGunSettings : null);
                return existingGunSettings;
            }

            return null;
        }
    }
}