using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponRigSettings
{
    [SerializeField] private GameObject _weaponModel;
    [SerializeField] private Transform _leftHandIKTransform;

    public void Load()
    {
        WeaponRigForLeftHand.Instance.SetRig(_leftHandIKTransform);
    }

    public void Reset()
    {
        WeaponRigForLeftHand.Instance.SetRig(null);
    }
}