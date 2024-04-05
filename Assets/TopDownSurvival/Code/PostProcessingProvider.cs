using System;
using _Project.Common.Inventory;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TopDownSurvival.Code
{
    public class PostProcessingProvider : MonoBehaviour
    {
        [SerializeField] private Volume _volume;

        private DepthOfField _depthOfField;

        private void Start()
        {
            _volume = GetComponent<Volume>();
            _volume.profile.TryGet(out _depthOfField);
            
            InventoryManager.OnToggleInventory += OnEnableInventoryVisualEffects;
        }

        private void OnDisable()
        {
            InventoryManager.OnToggleInventory -= OnEnableInventoryVisualEffects;
        }

        private void OnEnableInventoryVisualEffects(bool isActive)
        {
            _depthOfField.active = isActive;
        }
    }
}