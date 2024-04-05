using Sirenix.Utilities;

namespace DeadNation
{
    using System.Collections.Generic;
    using System.Linq;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public interface IStatObserver
    {
        public void OnNotify(StatType type, int value, int maxValue);
    }

    public interface IStatHandler
    {
        public T Stat<T>() where T : Stat;
    }

    public class StatManager : Singleton<StatManager>, IStatObserver, IStatHandler
    {
        [InlineEditor] [SerializeField] private List<Stat> _statList = new();

        public delegate void OnNotifyDelegate(StatType type, int value, int maxValue);

        public static event OnNotifyDelegate OnNotifyStat;

        private void Start()
        {
            _statList.ForEach(r => r.RegisterObserver(this));

#if UNITY_EDITOR
            _statList.ForEach(r => r.ResetStat());
#endif
        }

        public void GetStat(Stat stat, int value)
        {
            Stat existingStat = _statList.FirstOrDefault(r => r == stat);
            if (existingStat != null) existingStat.Modify += value;
        }

        public T Stat<T>() where T : Stat
        {
            return _statList.FirstOrDefault(stat => stat.GetType() == typeof(T)) as T;
        }

        public void OnNotify(StatType type, int value, int maxValue)
        {
            OnNotifyStat?.Invoke(type, value, maxValue);
        }
    }
}