using UnityEngine.AzureSky;

namespace DeadNation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameManager : Singleton<GameManager>
    {
        // [SerializeField] private float _timeMultiplier;
        [SerializeField] private GameObject _playerFlashlight;
        //private TimeTracker _timeSettings = new();
        // public TimeTracker TimeSettings => _timeSettings;

        [SerializeField] private bool _isNight;

        public bool IsNight => _isNight;

        private void Start()
        {
            AzureTimeController.OnTimeChanged += NotifyTimeChanged;
        }

        private void OnDisable()
        {
            AzureTimeController.OnTimeChanged -= NotifyTimeChanged;
        }

        private void Update()
        {
            // _timeSettings.UpdateTime(_timeMultiplier);
        }

        private void NotifyTimeChanged(int day, int hour, int minute)
        {
            _isNight = hour >= 19 || hour < 6;
            _playerFlashlight.SetActive(_isNight);

            ///Gece olduğunda yaşanacak olaylar...
        }
    }
}