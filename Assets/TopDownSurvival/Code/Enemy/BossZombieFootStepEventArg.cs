//using Cinemachine;
using UnityEngine;

//[RequireComponent(typeof(CinemachineImpulseSource))]
public class BossZombieFootStepEventArg : MonoBehaviour
{
   // private CinemachineImpulseSource _cinemachineImpulseSrc;

    private void Start()
    {
     //   _cinemachineImpulseSrc = GetComponent<CinemachineImpulseSource>();
    }

    public void FootStepAnimationEvent(AnimationEvent arg)
    {
       // _cinemachineImpulseSrc.GenerateImpulse();
    }
}