using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Common.ItemSystem
{
    /// <summary>
    /// Data
    /// </summary>
    public enum ItemType
    {
        Weapon,
        Consumable
    }

    /// <summary>
    /// Sub
    /// </summary>
    public enum WeaponType
    {
        Firearm,
        Melee
    }

    public abstract class ItemData : ScriptableObject
    {
        [SerializeField] private int _itemId = 0;
        [SerializeField] private int _itemWeight;
        [SerializeField] private string _itemName = "Item";
        [SerializeField] private string _itemDescription = "Description";
        [SerializeField] private bool _isStackable;
        [SerializeField] private Sprite _itemIcon = null;
        [SerializeField, ReadOnly] private ItemType _itemType;


        public virtual ItemType ItemType
        {
            get => _itemType;
            protected set => _itemType = value;
        }

        public int Id => _itemId;
        public int Weight => _itemWeight;
        public string Name => _itemName;
        public string Description => _itemDescription;
        public bool Stackable => _isStackable;
        public Sprite Icon => _itemIcon;
    }
}