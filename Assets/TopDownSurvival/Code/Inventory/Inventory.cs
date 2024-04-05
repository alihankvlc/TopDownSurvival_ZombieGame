using System;
using System.Collections.Generic;
using _Project.Common.ItemSystem;
using ModestTree;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Common.Inventory
{
    public interface IProviderInventory
    {
        public void AddItem(Item item);
        public void RemoveItem(Item item);
        public void UseItem(Item item);
        public void DropItem(Item item);
    }

    public class Inventory : MonoBehaviour, IProviderInventory
    {
        [SerializeField, InlineEditor] private List<Item> _items = new();
        [SerializeField] private int _capacity;

        [SerializeField, ReadOnly] private int _currentWeight;


        public static Action<int> OnChangeWeight;
        public static Action OnInventoryCapacityOut;

        public void AddItem(Item item)
        {
            if (_currentWeight >= _capacity)
                OnInventoryCapacityOut?.Invoke();

            if (_items.Contains(item) && item.Data.Stackable)
            {
                _currentWeight += item.Data.Weight;
                item.Stack(ItemSystem.StackType.Increase);
                OnChangeWeight?.Invoke(_currentWeight);
                return;
            }

            _items.Add(item);
            _currentWeight += item.Data.Weight;
            OnChangeWeight?.Invoke(_currentWeight);
        }

        public void RemoveItem(Item item)
        {
            if (_items.Contains(item))
            {
                if (item.Data.Stackable)
                {
                    item.Stack(ItemSystem.StackType.Decrease);
                    _currentWeight -= item.Data.Weight;
                    OnChangeWeight?.Invoke(_currentWeight);

                    if (item.Count < 1)
                    {
                        _currentWeight = Mathf.Max(0, _currentWeight - item.Data.Weight);
                        OnChangeWeight?.Invoke(_currentWeight);
                        _items.Remove(item);
                        return;
                    }
                }

                _currentWeight = Mathf.Max(0, _currentWeight - item.Data.Weight);
                OnChangeWeight?.Invoke(_currentWeight);
                _items.Remove(item);
            }
        }

        public void UseItem(Item item)
        {
            /*Item kullanıldığında kaybolacak mı kaybolmıcakmı vırtı zırtı onları ayarlıcam...*/

            item.Use();
            //RemoveItem(item);
        }

        public void DropItem(Item item)
        {
            item.Drop();
            // RemoveItem(item);
        }
    }
}