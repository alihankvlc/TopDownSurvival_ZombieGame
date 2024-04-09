using System;
using DeadNation;
using UnityEngine;

namespace _Project.Common.ItemSystem
{
    public abstract class WeaponData : ItemData
    {
        [SerializeField] protected GameObject _model;
        [SerializeField] protected AudioClip _soundEffect;
        [SerializeField] protected GameObject _bullet;

        public AudioClip SoundEffect => _soundEffect;
        public GameObject Bullet => _bullet;

        public virtual WeaponType WeaponType { get; protected set; }
        public override ItemType ItemType { get; protected set; } = ItemType.Weapon;
    }
}