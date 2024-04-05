using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Common.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform ParentAfterDrag;
    [HideInInspector] public Slot ParentAfterSlot;

    public Image Image;
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ParentAfterSlot = GetComponentInParent<Slot>();
        ParentAfterDrag = transform.parent;

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        Image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        _canvasGroup.alpha = 0.25f;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(ParentAfterDrag);
        _canvasGroup.alpha = 1f;
        
        Image.raycastTarget = true;
    }
}