using System;
using System.Collections;
using System.Collections.Generic;
using DeadNation;
using UnityEngine;

public class InventorySoundEffect : MonoBehaviour
{
    [SerializeField] private AudioSource _source;

    private void OnEnable()
    {
        _source.clip = SoundManager.Instance.GetClip(2);
        _source.Play();
    }

    private void OnDisable()
    {
        _source.clip = SoundManager.Instance.GetClip(3);
        _source.Play();
    }
}
