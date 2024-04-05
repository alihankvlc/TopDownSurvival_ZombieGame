using System;
using UnityEngine;
using Zenject;

namespace DeadNation
{
    [System.Serializable]
    public class Reward
    {
        [SerializeField] private RewardType _type;
        [SerializeField] private int _amount;


        public void Give()
        {
            switch (_type)
            {
                case RewardType.Exp:
                    StatManager.Instance.Stat<Experience>().Modify += _amount;
                    break;
                case RewardType.Gold:
                    break;
            }
        }
    }
}