using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Common.ItemSystem;
using DeadNation;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Common.Inventory
{
    public interface IInventoryManager
    {
        public void AddItemToInventory(Item item);
        public void RemoveFromItem(Item item);
    }

    public class InventoryManager : Singleton<InventoryManager>, IInventoryManager
    {
        [SerializeField, InlineEditor] private List<Slot> _slots = new();
        [SerializeField] private GameObject _slotDisplay;
        [SerializeField] private Item _denemeItem;
        [SerializeField] private Item _denemeItem2;

        [SerializeField] private AudioSource _audioSrc;

        [Inject] private IProviderInventory _inventoryProvider;
        [Inject] private InputHandler _inputHandler;
        [Inject] private IWeaponHandler _weaponHandler;

        [SerializeField] private WeaponData _equippedWeapon;

        private const int _toolBeltSlotSize = 5;
        private bool _isShowInventory;
        public bool InventoryEnable => _isShowInventory;
        public static Action<bool> OnToggleInventory;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                AddItemToInventory(_denemeItem);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                AddItemToInventory(_denemeItem2);
            }

            for (int i = 1; i <= _toolBeltSlotSize; i++)
                if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i - 1)))
                {
                    Item slotInItem = _slots[i - 1]?.SlotInItem;
                    if (slotInItem != null)
                    {
                        if (slotInItem.Data is WeaponData existingData)
                        {
                            if (_equippedWeapon == null)
                            {
                                _equippedWeapon = existingData;
                                _weaponHandler.GetWeaponSettings(_equippedWeapon.Id, true);
                            }
                            else
                            {
                                if (slotInItem.Data.Id != _equippedWeapon.Id)
                                {
                                    _weaponHandler.GetWeaponSettings(_equippedWeapon.Id, false);
                                    
                                    _equippedWeapon = existingData;
                                    _weaponHandler.GetWeaponSettings(_equippedWeapon.Id, true);
                                }
                            }
                        }
                    }
                    // if (slotInItem?.Data is WeaponData existingData && _equippedWeapon != existingData)
                    // {
                    //     _equippedWeapon = existingData;
                    //     _weaponHandler.GetWeaponSettings(existingData.Id, true);
                    // }
                    //
                    // if (_equippedWeapon != null)
                    // {
                    //     if (slotInItem == null)
                    //     {
                    //         _weaponHandler.GetWeaponSettings(_equippedWeapon.Id, false);
                    //         _equippedWeapon = null;
                    //     }
                    //     else
                    //     {
                    //         if (slotInItem?.Data is WeaponData && _equippedWeapon != existingData)
                    //         {
                    //             Debug.Log("asdfasdfsdfasdf");
                    //         }
                    //     }
                    // }
                }


            if (_inputHandler.Inventory)
            {
                _isShowInventory = !_isShowInventory;
                OnToggleInventory?.Invoke(_isShowInventory);

                _audioSrc.clip =
                    _isShowInventory ? SoundManager.Instance.GetClip(2) : SoundManager.Instance.GetClip(3);

                _audioSrc.Play();
            }
        }

        public void AddItemToInventory(Item item)
        {
            Slot emptySlot = _slots.FirstOrDefault(r => r.Status == SlotStatus.Empty);

            if (emptySlot != null)
            {
                _inventoryProvider.AddItem(item);
                emptySlot.AddToSlot(item);
                Instantiate(_slotDisplay, emptySlot.transform);
            }
        }

        public void RemoveFromItem(Item item)
        {
            _inventoryProvider.RemoveItem(item);
        }
    }
}