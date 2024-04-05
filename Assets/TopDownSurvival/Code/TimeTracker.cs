namespace DeadNation
{
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using UnityEngine;
    using System.Collections;

    public class TimeTracker
    {
        private int _days = 1;
        private int _hours;
        private int _minute;

        private float _elapsedSeconds;

        public delegate void NotifyTimeDelegate(int day, int hours, int minutes);

        public static event Action OnDayChanged;
        public static event Action OnHourChanged;
        public static event Action OnMinuteChanged;
        public static event NotifyTimeDelegate OnTimeChanged;

        public TimeTracker() => OnTimeChanged?.Invoke(_days, _hours, _minute);

        public int Day
        {
            get => _days;
            private set => SetAndInvokeIfChanged(ref _days, value, OnDayChanged);
        }

        public int Hour
        {
            get => _hours;
            private set => SetAndInvokeIfChanged(ref _hours, value, OnHourChanged);
        }

        public int Minute
        {
            get => _minute;
            private set => SetAndInvokeIfChanged(ref _minute, value, OnMinuteChanged);
        }

        internal void UpdateTime(float timeMultiplier)
        {
            _elapsedSeconds += Time.deltaTime * timeMultiplier;
            while (_elapsedSeconds > 59)
            {
                IncrementMinute();
                _elapsedSeconds -= 60;
                OnTimeChanged?.Invoke(_days, _hours, _minute);
            }
        }

        private void IncrementMinute()
        {
            Minute++;
            if (Minute > 59)
            {
                Minute = 0;
                IncrementHour();
            }
        }

        private void IncrementHour()
        {
            Hour++;

            if (Hour > 23)
            {
                Hour = 0;
                Day++;
            }
        }

        private void SetAndInvokeIfChanged(ref int field, int value, Action action)
        {
            if (field != value)
            {
                field = value;
                action?.Invoke();
            }
        }
    }
}