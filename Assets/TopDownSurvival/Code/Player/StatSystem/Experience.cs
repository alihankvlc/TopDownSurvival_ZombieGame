namespace DeadNation
{
    using System;
    using UnityEngine;
    using Unity.Collections;

    [CreateAssetMenu(fileName = "PlayerExperienceStat", menuName = "DeadNation/Player/Stat/Create Experience Stat",
        order = 4)]
    public class Experience : Stat
    {
        public delegate void ExperineceInfoDelegate(int level, int currentExp, int requiredExp);

        public static event ExperineceInfoDelegate OnNotifyExperience;
        public static event Action OnLevelUp;
        public static event Action<int, int> OnChangeExperience;

        [ReadOnly, SerializeField] private int _currentExp = 0;
        [ReadOnly, SerializeField] private int _requiredExp = 45;
        [ReadOnly, SerializeField] private int _level = 1;

        private int _nextRequiredExp;

        public override int Modify
        {
            get => base.Modify;
            set
            {
                base.Modify = value;
                if (CurrentValue >= _requiredExp)
                {
                    _level++;
                    CurrentValue %= _requiredExp;
                    _requiredExp = _level * 45 + 150;
                    OnLevelUp?.Invoke();
                }
                OnNotifyExperience?.Invoke(_level, CurrentValue, _requiredExp);
            }
        }

#if UNITY_EDITOR
        public override void ResetStat()
        {
            base.ResetStat();
            _level = 1;
            _requiredExp = 45;
        }
#endif
    }
}