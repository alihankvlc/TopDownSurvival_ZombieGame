using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Common.Inventory;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotVisualProvider : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI _countTextMeshPro;
    [SerializeField] private Image _icon;
    [SerializeField] private GameObject _uiHighLightedEffect;
    [SerializeField] private GameObject _uiSelectedEffect;

    public static event Action<string, string, Sprite> OnEnableInformationDisplay;
    public static event Action<bool> OnShowItemInformationDisplay;

    private void Start()
    {
        Slot slot = GetComponentInParent<Slot>();

        if (slot != null)
        {
            SetCount(slot.SlotInItem.Count);
            SetIcon(slot.SlotInItem.Data.Icon);
        }
    }

    public void SetCount(int count)
    {
        _countTextMeshPro.SetText(count.ToString());
    }

    public void SetIcon(Sprite icon)
    {
        _icon.sprite = icon;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        _uiHighLightedEffect.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject targetObject = eventData.pointerEnter;
        Slot slot = targetObject.GetComponentInParent<Slot>();

        if (slot != null && slot.SlotInItem != null)
        {
            _uiSelectedEffect.SetActive(true);
            _uiHighLightedEffect.SetActive(false);

            _icon.transform.DOScale(1.1f, 0.1f).OnComplete(() => { _icon.transform.DOScale(1f, 0.05f); });
            OnShowItemInformationDisplay?.Invoke(true);

            string itemName = slot.SlotInItem.Data.Name;
            string itemDescription = slot.SlotInItem.Data.Description;
            Sprite itemIcon = slot.SlotInItem.Data.Icon;

            OnEnableInformationDisplay?.Invoke(itemName, itemDescription, itemIcon);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnShowItemInformationDisplay?.Invoke(false);

        _uiSelectedEffect.SetActive(false);
        _uiHighLightedEffect.SetActive(false);
    }
}