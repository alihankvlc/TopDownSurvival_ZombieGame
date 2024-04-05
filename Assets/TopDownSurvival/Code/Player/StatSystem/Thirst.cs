namespace DeadNation
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "PlayerThirstStat", menuName = "DeadNation/Player/Stat/Create Thirst Stat", order = 2)]
    public class Thirst : Stat
    {
        public static event Action OnThirstZero;

        public override int Modify
        {
            get => base.Modify;
            set
            {
                base.Modify = value;
                if (CurrentValue <= 0) OnThirstZero?.Invoke();
            }
        }
    }
}