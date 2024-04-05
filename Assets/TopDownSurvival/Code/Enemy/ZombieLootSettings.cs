namespace DeadNation
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "ZombieDrop", menuName = "DeadNation/Zombie/Create Drop")]
    public class ZombieLootSettings : ScriptableObject
    {
        public DropType Type;
        public GameObject DropObject;
        public int Rate;
        public float DisappearDelay;
        public float IncreaseAmount;
    }
}