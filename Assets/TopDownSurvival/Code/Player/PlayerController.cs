using _Project.Common.Inventory;
using Cinemachine;
using UnityEngine.AzureSky;
using UnityEngine.Diagnostics;
using Zenject;

namespace DeadNation
{
    using System;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine.Serialization;
    using UnityEngine;
    using UnityEngine.InputSystem;


    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : Singleton<PlayerController>, IDamageable
    {
        [Header("Movement Settings")] [SerializeField] private float _moveSpeed = 3f;
        [SerializeField] private float _animationDampTime;
        [SerializeField] private int _level;

        [SerializeField] private float _rotationSpeed = 10f;


        [Space] [Header("Ground Settings")] [SerializeField] private LayerMask _groundLayerMask;

        [SerializeField] private float _groundRadius = 0.12f;
        [SerializeField] private float _groundOffset = 0.08f;

        [Header("Other")] [SerializeField] private LayerMask _rotationToLayerMask;
        [Inject] private IStatHandler _statHandler;
        [Inject] private InputHandler _input;

        private const float _gravity = -25f;
        private const float _threshold = 0.1f;

        private readonly int HORIZONTAL_VELOCITY_ANIM_ID = Animator.StringToHash("HorizontalVelocity");
        private readonly int VERTICAL_VELOCITY_ANIM_ID = Animator.StringToHash("VerticalVelocity");

        private bool _isGrounded;
        private bool _isAlive;


        private Vector3 _moveDirection = new Vector3(0, 0, 0);
        private Vector3 _verticalVelocity;
        private Vector3 _mousePosition;


        private CharacterController _characterController;
        private Animator _animator;
        private CinemachineImpulseSource _cinemachineImpulseSrc;

        public Transform PlayerTransform => transform;
        public static Action<DropType, float> OnCollect;
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
            _level = 1;
            _isAlive = true;

            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _cinemachineImpulseSrc = GetComponent<CinemachineImpulseSource>();

            OnCollect += OnEnemyLootObject;
            Health.OnHealthZero += Death;
        }

        private void Update()
        {
            if (ConsoleController.Instance.IsOpen || InventoryManager.Instance.InventoryEnable)
            {
                _animator.SetFloat(HORIZONTAL_VELOCITY_ANIM_ID, 0, _animationDampTime, Time.deltaTime * 15f);
                _animator.SetFloat(VERTICAL_VELOCITY_ANIM_ID, 0, _animationDampTime, Time.deltaTime * 15f);
                return;
            }

            PlayerMovement();
        }

        #region Movement

        private void PlayerMovement()
        {
            _moveDirection.x = _input.Move.x;
            _moveDirection.z = _input.Move.y;

            PlayerRotation();


            _animator.SetFloat(HORIZONTAL_VELOCITY_ANIM_ID, _input.Move.x, _animationDampTime, Time.deltaTime * 15f);
            _animator.SetFloat(VERTICAL_VELOCITY_ANIM_ID, _input.Move.y, _animationDampTime, Time.deltaTime * 15f);

            _characterController.Move(((transform.TransformDirection(_moveDirection) * _moveSpeed) + Gravity()) *
                                      Time.deltaTime);
        }

        private void PlayerRotation()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _rotationToLayerMask))
            {
                Vector3 cursorToPlayer = transform.position - hitInfo.point;

                float distanceToPlayer = cursorToPlayer.magnitude;
                float maxDistance = 0.75f;

                if (distanceToPlayer < maxDistance)
                {
                    cursorToPlayer = cursorToPlayer.normalized * maxDistance;
                    hitInfo.point = transform.position - cursorToPlayer;
                }

                Vector3 targetPosition = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
                Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
            }
        }

        #endregion

        #region Gravity

        public Vector3 Gravity()
        {
            _verticalVelocity.y = IsGrounded() && _verticalVelocity.y < 0.0f ? -2f : _verticalVelocity.y;
            _verticalVelocity.y += Time.deltaTime * _gravity;
            return _verticalVelocity;
        }

        public bool IsGrounded()
        {
            Vector3 spherePosition = transform.position + Vector3.up * _groundOffset;
            _isGrounded = Physics.CheckSphere(spherePosition, _groundRadius, _groundLayerMask,
                QueryTriggerInteraction.Ignore);

            return _isGrounded;
        }

        #endregion

        public void TakeDamage(int amount)
        {
            _statHandler.Stat<Health>().Modify -= amount;
        }

        private void Death()
        {
        }

        private void OnEnemyLootObject(DropType type, float increaseAmount)
        {
            switch (type)
            {
                case DropType.Health:
                    _statHandler.Stat<Health>().Modify += 20;
                    break;
                case DropType.Gold:
                    break;
                case DropType.Ammo:
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            ICollectible collectible = other.transform.GetComponent<CollectibleProvider>();
            collectible?.Collect();
        }

        private void OnDisable()
        {
            OnCollect -= OnEnemyLootObject;
            Health.OnHealthZero -= Death;
        }
    }
}