using System;
using UnityEngine;

namespace _Project.Common.ItemSystem.Database
{
    public class DatabaseInitializer : MonoBehaviour
    {
        private void Start()
        {
            ItemDatabase database = ItemDatabase.Instance;
            ItemDatabase.Instance.InitializeDatabase();
        }
    }
}