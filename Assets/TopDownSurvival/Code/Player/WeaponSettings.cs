using System;
using _Project.Common.ItemSystem;
using UnityEngine;

namespace DeadNation
{
    public enum WeaponSettingsType
    {
        Firearm,
        Melee
    }

    public class WeaponSettings
    {
        [SerializeField] protected WeaponData _weaponData;
        [SerializeField] protected WeaponRigSettings _rigSettings;

        private bool _isEnable;
        public WeaponData Data => _weaponData;
        public virtual WeaponSettingsType SettingsType { get; protected set; }


        public void SetActiveSettings(bool isActiveMode)
        {
            if (isActiveMode)
            {
                if (!_isEnable)
                {
                    _rigSettings.Load();
                    _isEnable = true;
                }

                return;
            }

            _rigSettings.Reset();
            _isEnable = false;
        }
    }
}