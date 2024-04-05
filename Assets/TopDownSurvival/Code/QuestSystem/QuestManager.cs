using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace DeadNation
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField, InlineEditor] private List<Quest> _quests = new();

        private Quest _currentActiveQuest;
        private AudioSource _audioSrc;

        private void Start()
        {
            _audioSrc = GetComponent<AudioSource>();
            StartCoroutine(StartQuest());

#if UNITY_EDITOR
            _quests.ForEach(r => r.ResetQuest());
#endif
        }

        private IEnumerator StartQuest()
        {
            yield return new WaitForSeconds(6f);
            if (_quests != null && _quests.Count > 0)
            {
                _currentActiveQuest = _quests[0];
                _currentActiveQuest.AudioSource = _audioSrc;
                _currentActiveQuest.StartQuest();
            }
        }
    }
}