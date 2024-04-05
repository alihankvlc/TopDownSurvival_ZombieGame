using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeadNation
{
    public interface IDamageable
    {
        public void TakeDamage(int amount);
    }

    public interface ICollectible
    {
        public void Collect();
    }
}