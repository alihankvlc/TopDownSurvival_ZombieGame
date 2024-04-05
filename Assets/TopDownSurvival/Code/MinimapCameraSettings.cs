using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DeadNation
{
    public class MinimapCameraSettings : MonoBehaviour
    {
        [SerializeField] private Vector3 followOfset = new Vector3(0, 5, 0);
        [SerializeField] private bool onlyPosition = true;
        public Transform m_PlayerTransform;


        private void LateUpdate()
        {
             transform.position = m_PlayerTransform.position + followOfset;
           if (!onlyPosition)
               transform.rotation = Quaternion.Euler(90, m_PlayerTransform.eulerAngles.y, 0);
        }
    }
}