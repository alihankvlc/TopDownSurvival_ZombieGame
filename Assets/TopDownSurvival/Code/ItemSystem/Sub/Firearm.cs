using System;
using DeadNation;
using UnityEngine;

namespace _Project.Common.ItemSystem
{
    [CreateAssetMenu(fileName = "New_Item", menuName = "ItemSystem/Create Data/Gun")]
    public sealed class Firearm : WeaponData
    {
        [SerializeField] private int _fireRate = 9;
        [SerializeField] private int _bulletForce = 1200;


        public override WeaponType WeaponType { get; protected set; } = WeaponType.Firearm;

        public GameObject Bullet => _bullet;
        public int BulletForce => _bulletForce;
        public int FireRate => _fireRate;
    }
}