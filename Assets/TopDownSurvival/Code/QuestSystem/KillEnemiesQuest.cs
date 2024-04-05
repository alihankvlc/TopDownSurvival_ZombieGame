using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace DeadNation
{
    [CreateAssetMenu(fileName = "KillEnemiesQuest", menuName = "DeadNation/Quest/Create/Kill Enemies")]
    public class KillEnemiesQuest : Quest
    {
        [SerializeField] private int _numberOfKillZombies;

        private int _requiredKillZombies;

        public override void StartQuest()
        {
            int playerPreviousKillZombieCount = PlayerCombat.Instance.KillCounter;
            _requiredKillZombies = playerPreviousKillZombieCount + _numberOfKillZombies;
            
            SpawnManager.Instance.SpawnZombie(_numberOfKillZombies, 1f, 1f);
            EnemyController.OnEnemyKilledEvent += OnEnemyKilled;
            
            base.StartQuest();
        }

        public void OnEnemyKilled(int expreience)
        {
            if (!IsCompleted)
            {
                int playerZombieKillCount = PlayerCombat.Instance.KillCounter;
                if (playerZombieKillCount == _requiredKillZombies)
                {
                    ResetKillZombieQuest();
                    CompleteQuest();
                    
                    IsCompleted = true;
                }
            }
        }

        private void ResetKillZombieQuest()
        {
            EnemyController.OnEnemyKilledEvent -= OnEnemyKilled;
            _requiredKillZombies = 0;
        }
#if UNITY_EDITOR
        [Button("Uns-Event")]
        public override void ResetQuest()
        {
            base.ResetQuest();
        }
#endif
    }
}