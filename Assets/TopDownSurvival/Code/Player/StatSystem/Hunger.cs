namespace DeadNation
{
    using System;
    using DeadNation;
    using UnityEngine;

    [CreateAssetMenu(fileName = "PlayerHungerStat", menuName = "DeadNation/Player/Stat/Create Hunger Stat", order = 1)]
    public class Hunger : Stat
    {
        public static event Action OnHungerZero;

        public override int Modify
        {
            get => base.Modify;
            set
            {
                base.Modify = value;
                if (CurrentValue <= 0) OnHungerZero?.Invoke();
            }
        }
    }
}