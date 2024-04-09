using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Common.Inventory;
using _Project.Common.ItemSystem;
using Cinemachine;
using DeadNation;
using UnityEngine;
using Zenject;

public interface IEquippableWeapon
{
    public void EquipWeapon(Weapon weapon);
}

public class PlayerCombat : Singleton<PlayerCombat>, IEquippableWeapon
{
    [SerializeField] private Weapon _equippedWeapon;
    [SerializeField] private Transform _cursor;
    [SerializeField] private GameObject _bulletPrefab;
    private CinemachineImpulseSource _cinemachineImpulse;

    private Animator _animator;
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
        _animator = GetComponent<Animator>();
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

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            _cursor.transform.position = hitInfo.point;
            _cursor.gameObject.SetActive(_input.Aim);
        }

        if (_equippedWeapon != null)
        {
            if (_equippedWeapon.Settings is FirearmSettings existingFirearm)
            {
                HandleGunAttack(existingFirearm, hitInfo.point);
            }
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        _equippedWeapon = weapon;
        _animator.SetBool("OnEquip_Rifle", weapon != null);
    }

    private void HandleGunAttack(FirearmSettings firearmSettings, Vector3 targetPosition)
    {
        Firearm firearm = firearmSettings.Data as Firearm;
        if (Time.time - _lastShootTime >= 1.0f / firearm.FireRate && _input.Attack && _input.Aim)
        {
            _lastShootTime = Time.time;
            firearmSettings.Shot(_bulletPrefab, targetPosition, firearm.SoundEffect);
        }
    }

    private void OnDisable()
    {
        EnemyController.OnEnemyKilledEvent -= OnEnemyKilled;
    }
}