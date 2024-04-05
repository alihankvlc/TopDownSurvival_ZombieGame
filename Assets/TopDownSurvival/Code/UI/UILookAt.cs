using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeadNation
{
    public class UILookAt : MonoBehaviour
    {
        public void Update()
        {
            Camera camera = Camera.main;
            transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        }

    }
}
