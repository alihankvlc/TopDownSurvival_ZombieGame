using System;
using System.Collections;
using System.Collections.Generic;
using DeadNation;
using DG.Tweening;
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

        float startWeight = _constraint.weight;
        float endWeight = 1f;

        DOTween.To(() => startWeight, x => startWeight = x, endWeight, 0.5f)
            .SetEase(Ease.Linear)
            .OnUpdate(() => { _constraint.weight = startWeight; });
    }
}