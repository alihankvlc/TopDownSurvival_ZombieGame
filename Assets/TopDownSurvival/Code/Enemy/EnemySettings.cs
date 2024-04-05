using UnityEngine.Serialization;

namespace DeadNation
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [CreateAssetMenu(fileName = "EnemySettings", menuName = "DeadNation/Zombie/EnemySettings", order = 0)]
    public class EnemySettings : ScriptableObject
    {
        [SerializeField] private ZombieType _zombieType;
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private int _damageAmount = 15;
        [SerializeField] private int _experience = 20;
        [SerializeField] private List<ZombieLootSettings> _dropList = new List<ZombieLootSettings>();
        [SerializeField] private RuntimeAnimatorController[] _animatorControllers;
        [SerializeField] private AudioClip[] _zombieChaseSoundEffect;
        [SerializeField] private AudioClip[] _zombieAttackSoundEffect;

        public readonly float AttackDistance = 1;
        public readonly float AttackInterval = 1.5f;
        public readonly float GameObjectDisableDuration = 2.5f;

        public readonly int AnimIsAliveHashID = Animator.StringToHash("IsAlive");
        public readonly int AnimGetHitHashID = Animator.StringToHash("GetHit");
        public readonly int AnimAttackHashID = Animator.StringToHash("OnAttack");
        public readonly int AnimHorizontalVelocityHashID = Animator.StringToHash("Horizontal");
        public readonly int AnimVerticalVelocityHashID = Animator.StringToHash("Vertical");

        public ZombieType ZombieTypes => _zombieType;
        public int MaxHealth => _maxHealth;
        public int Damage => _damageAmount;
        public int Experience => _experience;
        public IDamageable PlayerDamage => PlayerController.Instance.GetComponent<IDamageable>();
        public Transform GetPlayerTransform => PlayerController.Instance.PlayerTransform;

        public void Drop(Vector3 position)
        {
            int randomIndex = UnityEngine.Random.Range(0, _dropList.Count);
            bool probality = Calculating.Probability(_dropList[randomIndex].Rate);

            if (_dropList[randomIndex] == null || !probality)
                return;

            GameObject spawnLootObject = Instantiate(_dropList[randomIndex].DropObject, position, Quaternion.identity);
        }

        public RuntimeAnimatorController SetRandomAnimatorController()
        {
            if (_animatorControllers != null && _animatorControllers.Length > 0)
            {
                return _animatorControllers[GenerateRandomIndex(_animatorControllers.Length)];
            }

            return null;
        }

        public AudioClip GetZombieChaseSoundEffect()
        {
            return _zombieChaseSoundEffect[GenerateRandomIndex(_zombieChaseSoundEffect.Length)];
        }

        public AudioClip GetZombieAttackSoundEffect()
        {
            return _zombieAttackSoundEffect[GenerateRandomIndex(_zombieChaseSoundEffect.Length)];
        }

        private int GenerateRandomIndex(int arguman) => UnityEngine.Random.Range(0, arguman);
    }
}