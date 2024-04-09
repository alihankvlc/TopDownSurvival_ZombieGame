using UnityEngine;

namespace DeadNation
{
    public class MeleeSettings : WeaponSettings
    {
        public override WeaponSettingsType SettingsType { get; protected set; } = WeaponSettingsType.Melee;
    }
}