using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Common.Inventory;
using _Project.Common.ItemSystem;
using Cinemachine;
using DeadNation;
using UnityEngine;
using Zenject;

public class PlayerCombat : Singleton<PlayerCombat>
{
    [SerializeField] private Transform _cursor;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _weaponPlaceHolder;
    private CinemachineImpulseSource _cinemachineImpulse;

    [SerializeField] private WeaponData _currentWeapon;
    private Camera _mainCamera;

    private float _lastShootTime;
    private int _bulletForce = 1200;

    [Inject] private InputHandler _input;
    [Inject] private IStatHandler _statHandler;
    [Inject] private IWeaponHandler _weaponHandler;
    private int _killCounter;

    public int KillCounter => _killCounter;

    private void Start()
    {
        _mainCamera = Camera.main;
        _cinemachineImpulse = GetComponent<CinemachineImpulseSource>();
    }

    private void OnEnable()
    {
        EnemyController.OnEnemyKilledEvent += OnEnemyKilled;
    }

    private void OnEnemyKilled(int xp)
    {
        _killCounter++;
        _statHandler.Stat<Experience>().Modify += xp;
    }


    private void Update()
    {
        if (InventoryManager.Instance.InventoryEnable)
            return;


        Attack();
    }


    private void Attack()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            _cursor.transform.position = hitInfo.point;
            HandleGunAttack(hitInfo.point);
        }
    }


    private void HandleGunAttack(Vector3 targetPosition)
    {
        GunSettings gunSettings = PlayerWeaponManager.Instance.GetWeaponSettings(25);

        if (Time.time - _lastShootTime >= 1.0f / gunSettings.Firearm.FireRate && _input.Attack)
        {
            _lastShootTime = Time.time;
            gunSettings.SetActiveSettings(true);
            gunSettings.Shot(_bulletPrefab, targetPosition, gunSettings.Firearm.SoundEffect);
        }
    }

    private void OnDisable()
    {
        EnemyController.OnEnemyKilledEvent -= OnEnemyKilled;
    }
}