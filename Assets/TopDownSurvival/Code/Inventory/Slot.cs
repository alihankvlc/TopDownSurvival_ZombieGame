using System;
using System.Runtime.CompilerServices;
using _Project.Common.ItemSystem;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace _Project.Common.Inventory
{
    public enum SlotType
    {
        Inventory,
        ToolBelt
    }

    public enum SlotStatus
    {
        Occupied,
        Empty
    }

    public class Slot : MonoBehaviour, IDropHandler
    {
        [SerializeField, ReadOnly, Space] private SlotStatus _status;
        [SerializeField] private SlotType _type;
        [SerializeField] private int _slotInItemCount;
        [SerializeField, Space] private int _slotIndex;

        [SerializeField, InlineEditor] private Item _slotInItem;

        public SlotType Type => _type;
        public SlotStatus Status => _status;

        public Item SlotInItem => _slotInItem;

        public int SlotInItemCount => _slotInItemCount;
        public int Index => _slotIndex;

        private void Start()
        {
            _status = SlotStatus.Empty; // Save sisteminde kalkacak...
        }

        public void AddToSlot(Item item)
        {
            UpdateSlot(SlotStatus.Occupied, item, item.Count);
        }

        public void RemoveFromItem()
        {
            UpdateSlot(SlotStatus.Empty, null, 0);
        }

        public void UpdateSlot(SlotStatus status, Item item, int slotInItemCount = 1)
        {
            _status = status;
            _slotInItem = item;

            _slotInItemCount = slotInItemCount;
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameObject droppedObject = eventData.pointerDrag;

            if (droppedObject != null)
            {
                DraggableItem draggableDropObject = droppedObject.GetComponent<DraggableItem>();

                if (draggableDropObject != null)
                {
                    Slot parentAfterSlot = draggableDropObject.ParentAfterSlot;
                    Item parentAfterItem = parentAfterSlot?.SlotInItem;

                    if (_status == SlotStatus.Empty)
                    {
                        parentAfterSlot.RemoveFromItem();
                        draggableDropObject.ParentAfterDrag = transform;

                        AddToSlot(parentAfterItem);
                    }
                    else
                    {
                        Item previousDraggableItem = null;

                        if (transform.childCount <= 0)
                            return;

                        Transform itemInSlot = transform.GetChild(0);
                        Transform previousTransform = draggableDropObject.ParentAfterDrag;

                        if (draggableDropObject.ParentAfterSlot != null)
                            previousDraggableItem = draggableDropObject.ParentAfterSlot.SlotInItem;

                        parentAfterSlot?.RemoveFromItem();
                        parentAfterSlot?.AddToSlot(_slotInItem);
                        draggableDropObject.ParentAfterDrag = transform;

                        AddToSlot(previousDraggableItem);
                        itemInSlot.SetParent(previousTransform);
                        itemInSlot.SetAsLastSibling();
                    }
                }
            }
        }
    }
}