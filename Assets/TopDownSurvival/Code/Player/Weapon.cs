using System;
using UnityEngine;

namespace DeadNation
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private FirearmSettings _settings;

        public FirearmSettings Settings
        {
            get => _settings;
        }

        private void OnEnable()
        {
            _settings.SetActiveSettings(true);
        }

        private void OnDisable()
        {
            _settings.SetActiveSettings(false);
        }
    }
}