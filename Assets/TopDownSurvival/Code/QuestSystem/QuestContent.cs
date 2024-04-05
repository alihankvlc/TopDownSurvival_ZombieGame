using System;
using Unity.VisualScripting;
using UnityEngine;

namespace DeadNation
{
    [System.Serializable]
    public class QuestContent
    {
        public static event Action<string> OnAddingQuestContent;

        public void AddContent(string description)
        {
            OnAddingQuestContent?.Invoke(description);
        }
    }
}