using UnityEngine;

namespace DeadNation
{
   // public class SoundEffect<T> where T : Sound{}
    public abstract class Sound : ScriptableObject
    {
        [SerializeField] protected AudioClip _clip;

        public virtual void Play(AudioSource source)
        {

        }

        public virtual void Stop(AudioSource source)
        {

        }
    }
}