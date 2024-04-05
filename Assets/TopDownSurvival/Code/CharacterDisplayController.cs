using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterDisplayController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject _target;
    [SerializeField] private int _rotationAmount;

    private bool _isHovering;

    private void Update()
    {
        CheckMousePosition();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _isHovering = true;
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");

            Vector3 rotation = _target.transform.rotation.eulerAngles;
            rotation.y += mouseX * _rotationAmount * Time.deltaTime;
            _target.transform.rotation = Quaternion.Euler(rotation);
        }
    }

    private void CheckMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;

        if ((mousePosition.x <= 0 || mousePosition.x >= Screen.width ||
             mousePosition.y <= 0 || mousePosition.y >= Screen.height) && _isHovering)
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            SetCursorPos(screenCenter);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void SetCursorPos(Vector3 position)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isHovering = false;
    }

    private void OnDisable()
    {
        _target.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}