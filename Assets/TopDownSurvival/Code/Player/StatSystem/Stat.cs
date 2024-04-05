using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DeadNation
{
    public abstract class Stat : SerializedScriptableObject
    {
        [Header("General Settings")] [SerializeField] private StatType _type = StatType.None;
        [SerializeField] private int _maxValue = 100;

        [ReadOnly, SerializeField] private int _currentValue = 100;
        [ReadOnly, SerializeField] private int _statChangedAmount;

        private List<IStatObserver> _observers = new(); //

        public StatType Type => _type;

        public int CurrentValue
        {
            get => _currentValue;
            protected set => _currentValue = value;
        }

        public int MaxValue
        {
            get => _maxValue;
            protected set => _maxValue = value;
        }

        public int ChangedAmount => _statChangedAmount;

        public virtual int Modify
        {
            get => _currentValue;
            set
            {
                if (_type == StatType.Experience)
                {
                    _currentValue = value;
                    NotifyObservers();
                    return;
                }

                int previousValue = _currentValue;
                int newValue = Mathf.Clamp(value, 0, _maxValue);

                if (newValue != _currentValue)
                {
                    bool isIncrease = newValue > previousValue;
                    _currentValue = newValue;
                    _statChangedAmount = isIncrease ? newValue - previousValue : (previousValue - newValue) * 1;
                    NotifyObservers();
                }
            }
        }

#if UNITY_EDITOR
        [Button(ButtonSizes.Medium)]
        public virtual void ResetStat()
        {
            _currentValue = _type == StatType.Radiation ? 0 : _maxValue;
        }
#endif


        public void RegisterObserver(IStatObserver observer) => _observers.Add(observer);
        protected void NotifyObservers() => _observers.ForEach(r => r.OnNotify(_type, Modify, _maxValue));
    }
}