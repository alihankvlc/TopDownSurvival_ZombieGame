namespace DeadNation
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "PlayerRadiationStat", menuName = "DeadNation/Player/Stat/Create Radiation Stat",
        order = 3)]
    public class Radiation : Stat
    {
        public static event Action OnRadiationZero;

        public override int Modify
        {
            get => base.Modify;
            set
            {
                base.Modify = value;
                if (CurrentValue <= 0) OnRadiationZero?.Invoke();
            }
        }
    }
}