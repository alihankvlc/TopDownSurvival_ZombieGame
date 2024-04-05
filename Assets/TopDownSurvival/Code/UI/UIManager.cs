using _Project.Common.Inventory;
using Sirenix.OdinInspector;
using UnityEngine.AzureSky;

namespace DeadNation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using DG.Tweening;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Serialization;
    using UnityEngine.UI;

    public class UIManager : SerializedMonoBehaviour
    {
        [Header("UI Player Stat Elements")] [SerializeField] private Slider _healthSlider;
        [SerializeField] private Slider _thirstSlider;
        [SerializeField] private Slider _hungerSlider;
        [SerializeField] private Slider _radiationSlider;
        [SerializeField] private GameObject _playerLevelInfoObject;
        [SerializeField] private TextMeshProUGUI _playerLevelInfoTextMesh;
        [SerializeField] private TextMeshProUGUI _playerInventoryDisplayLevelTextMesh;
        [SerializeField] private TextMeshProUGUI _playerInventoryDisplaySliderText;
        [SerializeField] private Slider _playerInventoryDisplayExpSlider;

        [Header("Quest UI Elements")] [SerializeField] private TextMeshProUGUI _questSubtitlesTextMesh;
        [SerializeField] private GameObject _questSubtitlesParentObject;
        [SerializeField] private GameObject _radioAssistantObject;
        [SerializeField] private GameObject _uiQuestCompletedEffect;
        [Header("Time UI Elements")] [SerializeField] private TextMeshProUGUI _timeInfTextMesh;

        [Header("Inventory UI Elements")] [SerializeField] private GameObject _uiInventory;
        [SerializeField] private TextMeshProUGUI _inventoryWeightTextMesh;
        [Header("Other")] [SerializeField] private GameObject _canvas;

        [Header("Inventory Slot UI Elements")] [SerializeField] private GameObject _displayParent;
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemDescription;
        [SerializeField] private Image _itemIcon;

        private bool _isInteractable = true;
        private Sequence _sequence;

        private bool m_LevelUpCompleteEffect;
        private Dictionary<StatType, Slider> _uiStatCache = new();

        private void Start()
        {
            InitializeUIStatCache();

            Experience.OnNotifyExperience += NotifyPlayerExp;
            Experience.OnLevelUp += TriggerLevelUpDisplay;


            Quest.OnStartQuest += UIStartQuest;
            Quest.OnCompletedQuest += UICompeletedQuest;

            InventoryManager.OnToggleInventory += ToggleInventory;

            // TimeTracker.OnTimeChanged += NotifyTimeChanged;

            StatManager.OnNotifyStat += UINotifyStat;

            AzureTimeController.OnTimeChanged += NotifyTimeChanged;
            AzureTimeController.OnTimeChanged += NotifyTimeChanged;

            SlotVisualProvider.OnEnableInformationDisplay += ShowItemDisplay;
            SlotVisualProvider.OnShowItemInformationDisplay += (bool isActive) => SetActive(isActive);
            Inventory.OnChangeWeight += (int weight) => _inventoryWeightTextMesh.SetText($"{weight}.0 lbs");
        }

        private void OnDisable()
        {
            Experience.OnNotifyExperience -= NotifyPlayerExp;
            Experience.OnLevelUp -= TriggerLevelUpDisplay;


            Quest.OnStartQuest -= UIStartQuest;
            Quest.OnCompletedQuest -= UICompeletedQuest; // Tamamlanınca removelıcam...


            // TimeTracker.OnTimeChanged -= NotifyTimeChanged;
            StatManager.OnNotifyStat -= UINotifyStat;
            AzureTimeController.OnTimeChanged -= NotifyTimeChanged;
            InventoryManager.OnToggleInventory -= ToggleInventory;

            SlotVisualProvider.OnEnableInformationDisplay -= ShowItemDisplay;
            SlotVisualProvider.OnShowItemInformationDisplay -= (bool isActive) => SetActive(isActive);
            Inventory.OnChangeWeight -= (int weight) => _inventoryWeightTextMesh.SetText($"{weight}.0 lbs");
        }

        #region UIPlayerStat

        private void InitializeUIStatCache()
        {
            _uiStatCache.Add(StatType.Health, _healthSlider);
            _uiStatCache.Add(StatType.Hunger, _hungerSlider);
            _uiStatCache.Add(StatType.Thirst, _thirstSlider);
            _uiStatCache.Add(StatType.Radiation, _radiationSlider);
        }

        private void UINotifyStat(StatType type, int value, int maxValue)
        {
            if (_uiStatCache.TryGetValue(type, out Slider slider))
            {
                slider.maxValue = maxValue;
                slider.value = value;

                Color newColor = DetermineSliderColor(type, value);
                SetSliderFillColor(slider, newColor);
            }
        }

        private Color DetermineSliderColor(StatType type, int value)
        {
            return type == StatType.Radiation
                ? (value > 50 ? Color.red : Color.white)
                : (value < 20 ? Color.red : Color.white);
        }


        private void SetSliderFillColor(Slider slider, Color color)
        {
            Image fillImage = slider.fillRect.GetComponent<Image>();
            fillImage.color = color;
        }

        private void NotifyPlayerExp(int level, int currentExp, int requiredExp)
        {
            _playerLevelInfoTextMesh.SetText(level.ToString());
            _playerInventoryDisplayLevelTextMesh?.SetText(level.ToString());
            _playerInventoryDisplaySliderText.SetText($"{currentExp} / {requiredExp}");

            _playerInventoryDisplayExpSlider.maxValue = requiredExp;
            _playerInventoryDisplayExpSlider.value = currentExp;
        }

        private void TriggerLevelUpDisplay()
        {
            StartCoroutine(PlayerLevelInfoCoroutine());
        }

        private IEnumerator PlayerLevelInfoCoroutine()
        {
            _playerLevelInfoObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            _playerLevelInfoObject.SetActive(false);
        }

        #endregion

        #region UIQuest

        private void UIStartQuest(string questName, string subtitles)
        {
            UIShowQuestInfo(true);
            StartCoroutine(TypeTextCoroutine(_questSubtitlesTextMesh, subtitles,
                () => { _questSubtitlesParentObject.SetActive(false); }, 1.5f));
        }

        private void UICompeletedQuest()
        {
            StartCoroutine(UIShowQuestCompleted());
        }

        private void UIShowQuestInfo(bool param)
        {
            _questSubtitlesParentObject.SetActive(param);
            _radioAssistantObject.SetActive(param);
        }

        private IEnumerator UIShowQuestCompleted()
        {
            _uiQuestCompletedEffect.SetActive(true);
            _questSubtitlesParentObject.SetActive(true);
            string info = "Great job! The mission has been successfully completed!";
            StartCoroutine(TypeTextCoroutine(_questSubtitlesTextMesh, info,
                () => { _questSubtitlesParentObject.SetActive(false); }, 1.5f));

            yield return new WaitForSeconds(3.5f);

            _uiQuestCompletedEffect.transform.DOScale(0, 0.15f).OnComplete(() =>
            {
                _uiQuestCompletedEffect.SetActive(false);
                _uiQuestCompletedEffect.transform.DOScale(1, 0f);
            });

            yield return new WaitForSeconds(1f);

            UIShowQuestInfo(false);
        }

        #endregion

        #region UITime

        private void NotifyTimeChanged(int day, int hour, int minute)
        {
            bool isNight = hour >= 19 || hour < 6;
            string timeInfo = isNight
                ? $"DAY:{day} <color=red>{hour:D2}:{minute:D2}</color>"
                : $"DAY:{day} {hour:D2}:{minute:D2}";

            _timeInfTextMesh.SetText(timeInfo);
        }

        #endregion

        private void ToggleInventory(bool isActive)
        {
            _uiInventory.SetActive(isActive);
            _canvas.SetActive(!isActive);

            if (!isActive && _displayParent.activeSelf)
            {
                _displayParent.transform.DOScale(0f, 0f);
            }
        }

        public void ShowItemDisplay(string name, string description, Sprite icon)
        {
            SetActive(true);

            _itemName.SetText(name);
            _itemDescription.SetText(description);

            _itemIcon.sprite = icon;
        }

        private void SetActive(bool isActive)
        {
            if (!_isInteractable) return;

            _isInteractable = false;

            if (_sequence != null) _sequence.Complete();

            _sequence = DOTween.Sequence()
                .Append(_displayParent.transform.DOScale(isActive ? 1f : 0f, 0.25f))
                .OnComplete(() =>
                {
                    _isInteractable = true;
                    if (!isActive)
                        _displayParent.SetActive(false);
                });
        }

        private IEnumerator TypeTextCoroutine(TextMeshProUGUI tmp, string text, Action onComplete = null,
            float showDuration = 0f)
        {
            tmp.text = "";
            foreach (var c in text)
            {
                tmp.text += c;
                yield return null;
            }

            yield return new WaitForSeconds(showDuration);
            onComplete?.Invoke();
        }

        private IEnumerator GameObjectSetActive(GameObject obj, bool param, float duration)
        {
            WaitForSeconds wait = new WaitForSeconds(duration);

            yield return wait;

            obj.SetActive(param);
        }

        private string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute =
                (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute?.Description ?? value.ToString();
        }
    }
}