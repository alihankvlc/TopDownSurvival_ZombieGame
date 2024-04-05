using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[Serializable]
public class FirearmRigSettings
{
    [SerializeField] private GameObject _weaponModel;
    [SerializeField] private Transform _leftHandIKTransform;
    [SerializeField] private Vector3 _position;
    [SerializeField] private Vector3 _rotation;
    public GameObject Model => _weaponModel;

    public void Load()
    {
        _weaponModel.transform.localPosition = _position;
        _weaponModel.transform.localRotation = Quaternion.Euler(_rotation);
        
        WeaponRigForLeftHand.Instance.SetRig(_leftHandIKTransform);
    }
}