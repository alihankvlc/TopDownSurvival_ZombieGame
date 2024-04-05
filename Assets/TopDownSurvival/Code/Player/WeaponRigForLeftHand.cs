using System;
using System.Collections;
using System.Collections.Generic;
using DeadNation;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponRigForLeftHand : Singleton<WeaponRigForLeftHand>
{
    [SerializeField] private RigBuilder _rigBuilder;
    [SerializeField] private TwoBoneIKConstraint _constraint;

    public void SetRig(Transform target)
    {
        _constraint.data.target = target;
        _rigBuilder.Build();
    }
}