using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DeadNation
{
    public class CollectibleProvider : MonoBehaviour, ICollectible
    {
        [SerializeField] private ZombieLootSettings _zombieLoot;

        public void Collect()
        {
            PlayerController.OnCollect?.Invoke(_zombieLoot.Type, _zombieLoot.IncreaseAmount);
            gameObject.SetActive(false);
        }
    }
}