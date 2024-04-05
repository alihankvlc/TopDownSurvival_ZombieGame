using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeadNation
{
    public class Compass : MonoBehaviour
    {
        [SerializeField] private Transform _playerTranfsorm;
        [SerializeField] private RawImage _compassObject;

        private void Update()
        {
            _compassObject.uvRect = new Rect(_playerTranfsorm.localEulerAngles.y / 360f, 0f, 1f, 1f);
        }
    }
}