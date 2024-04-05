namespace DeadNation
{
    using System;
    using Sirenix.OdinInspector;
    using Unity.VisualScripting;
    using UnityEngine;

    [CreateAssetMenu(fileName = "PlayerHealthStat", menuName = "DeadNation/Player/Stat/Create Health Stat", order = 0),]
    public class Health : Stat
    {
        public static event Action OnHealthZero;

        public override int Modify
        {
            get => base.Modify;
            set
            {
                base.Modify = value;
                if (CurrentValue <= 0) OnHealthZero?.Invoke();
            }
        }
    }
}