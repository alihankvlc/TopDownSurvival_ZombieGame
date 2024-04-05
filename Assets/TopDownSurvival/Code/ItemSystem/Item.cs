using System;
using _Project.Common.ItemSystem.Database;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Common.ItemSystem
{
    public enum StackType
    {
        Increase,
        Decrease
    }

    [CreateAssetMenu(fileName = "New_Item", menuName = "ItemSystem/Create Item")]
    public class Item : ScriptableObject
    {
        [SerializeField, InlineEditor] private ItemData _itemData;
        [SerializeField] private int _itemCount = 1;

        public int Count => _itemCount;
        public ItemData Data => _itemData;

        public void Stack(StackType stackType, int stackAmount = 1)
        {
            if (_itemData.Stackable)
                _itemCount = stackType == StackType.Decrease ? --_itemCount : ++_itemCount;
        }

        public void Use()
        {
            Debug.Log($"{_itemData.Name} Use...");
        }

        public void Drop()
        {
            Debug.Log($"{_itemData.Name} Drop...");
        }
    }
}