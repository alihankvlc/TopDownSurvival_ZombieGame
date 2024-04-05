using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DeadNation
{
    public abstract class Quest : ScriptableObject
    {
        [SerializeField] private QuestType _questType;
        [SerializeField] private string _questName;
        [SerializeField] private string _questContentDescription;
        [SerializeField, Multiline] private string _questSubtitles;
        [SerializeField] private AudioClip _startQuestSoundEffect; //SoundManager Sınıfıyla ilişkilendirilecek...
        [SerializeField] private AudioClip _questCompletedSoundEffect; //SoundManager Sınıfıyla ilişkilendirilecek...
        [SerializeField] private QuestContent _questContent;
        [SerializeField] private bool _questIsCompleted;
        [SerializeField] private List<Reward> _reward = new();

        public QuestType Type
        {
            get => _questType;
            protected set => _questType = value;
        }

        public string Name
        {
            get => _questName;
            protected set => _questName = value;
        }

        public string Subtitles
        {
            get => _questSubtitles;
            protected set => _questSubtitles = value;
        }

        public bool IsCompleted
        {
            get => _questIsCompleted;
            protected set => _questIsCompleted = value;
        }

        public delegate void StartQuestDelegate(string questName, string subtitles);

        public static event StartQuestDelegate OnStartQuest;

        public static event Action OnCompletedQuest;

        [HideInInspector] public AudioSource AudioSource;

        public virtual void StartQuest()
        {
            IsCompleted = false;

            _questContent.AddContent(_questContentDescription);
            SoundManager.Instance.PlaySoundEffect(0, true, 0.1f);
            AudioSource.clip = _startQuestSoundEffect;
            AudioSource.Play();

            OnStartQuest?.Invoke(_questName, _questSubtitles);
        }

        public virtual void CompleteQuest()
        {
            if (_reward != null && _reward.Count > 0)
                _reward.ForEach(r => r.Give());

            SoundManager.Instance.StopSoundEffect();
            AudioSource.clip = _questCompletedSoundEffect;
            AudioSource.Play();

            OnCompletedQuest?.Invoke();
        }
#if UNITY_EDITOR
        public virtual void ResetQuest()
        {
            IsCompleted = false;
        }
#endif
    }
}