using UnityEngine;

namespace DeadNation.SubClass
{
    [CreateAssetMenu(fileName = "AmbianceSoundEffect", menuName = "DeadNation/Sound/Create AmbianceSound")]
    public class AmbianceSound : Sound
    {
        public override void Play(AudioSource source)
        {
            Debug.Log("Hey");
        }

        public override void Stop(AudioSource source)
        {
            
        }
    }
}