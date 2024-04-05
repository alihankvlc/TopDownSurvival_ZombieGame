using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Common.Inventory
{
    public class SlotInItemDisplayManager : MonoBehaviour
    {
        [Header("Inventory Slot UI Elements")]
        [SerializeField] private GameObject _displayParent;
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemDescription;
        [SerializeField] private Image _itemIcon;

        private bool _isInteractable = true;
        private Sequence _sequence;

        private void OnEnable()
        {
            SlotVisualProvider.OnEnableInformationDisplay += ShowItemDisplay;
            SlotVisualProvider.OnShowItemInformationDisplay += (bool isActive) => SetActive(isActive);
        }

        public void ShowItemDisplay(string name, string description, Sprite icon)
        {
            SetActive(true);

            _itemName.SetText(name);
            _itemDescription.SetText(description);

            _itemIcon.sprite = icon;
        }

        private void OnDisable()
        {
            SlotVisualProvider.OnEnableInformationDisplay -= ShowItemDisplay;
            SlotVisualProvider.OnShowItemInformationDisplay -= (bool isActive) => SetActive(isActive);
        }

        private void SetActive(bool isActive)
        {
            if (!_isInteractable) return;

            _isInteractable = false;

            if (_sequence != null) _sequence.Complete();

            _sequence = DOTween.Sequence()
                .Append(_displayParent.transform.DOScale(isActive ? 1f : 0f, 0.25f))
                .OnComplete(() => _isInteractable = true);
        }
    }
}