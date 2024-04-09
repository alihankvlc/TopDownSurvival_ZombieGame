using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Common.ItemSystem.Database
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "ItemSystem/Create Database")]
    public class ItemDatabase : ScriptableObject
    {
        [InlineEditor] [SerializeField] private List<Item> _itemList = new();
        private Dictionary<int, Item> _itemDatabaseDic = new();

        private static ItemDatabase _instance;

        public static ItemDatabase Instance
        {
            get
            {
                if (_instance == null)
                    _instance = Resources.Load<ItemDatabase>("ItemDatabase");

                return _instance;
            }
        }

        public void InitializeDatabase()
        {
            _itemDatabaseDic = _itemList.ToDictionary(r => r.Data.Id);
        }

        public Item GetItem(int id)
        {
            if (_itemDatabaseDic.TryGetValue(id, out Item existingItem))
                return existingItem;

            Debug.LogError($"Item with the specified {id} could not be found.");
            return null;
        }
    }
}