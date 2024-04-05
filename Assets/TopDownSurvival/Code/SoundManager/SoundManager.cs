using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DeadNation.SubClass;
using JetBrains.Annotations;
using UnityEngine;

namespace DeadNation
{
    [Serializable]
    public class SoundEffect
    {
        public int ID;
        public AudioClip Clip;
        [Multiline] public string Description;
    }

    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : Singleton<SoundManager>
    {
        private AudioSource _audioSource;
        [SerializeField] private List<SoundEffect> _soundEffects = new();

        private Dictionary<int, SoundEffect> _soundCache = new();

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _soundCache = _soundEffects.ToDictionary(r => r.ID);
        }

        public void PlaySoundEffect(int id, bool isLoop = false, float volume = 0.5f)
        {
            AudioClip clip = GetClip(id);

            _audioSource.loop = isLoop;
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public void StopSoundEffect()
        {
            _audioSource.Stop();
        }

        public void PlayOneShot(int id, Vector3 position, float volume = 1)
        {
            AudioSource.PlayClipAtPoint(GetClip(id), position, volume);
        }

        public AudioClip GetClip(int id)
        {
            if (_soundCache.TryGetValue(id, out SoundEffect existingSoundEffect))
                return existingSoundEffect.Clip;

            return null;
        }
    }
}