using DG.Tweening;

namespace DeadNation
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.AI;

    [RequireComponent(typeof(AudioSource))]
    public class EnemyController : MonoBehaviour, IDamageable
    {
        [Header("Settings"), SerializeField] private EnemySettings _settings;

        private int _currentHp;
        private float _canAttakAttackInterval;

        private bool _isAlive;

        private NavMeshAgent _agent;
        private Animator _animator;
        private CapsuleCollider _capsuleCollider;
        private AudioSource _audioSource;

        public static event Action<int> OnEnemyKilledEvent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            if (_capsuleCollider == null)
                _capsuleCollider = GetComponent<CapsuleCollider>();

            if (_settings.ZombieTypes == ZombieType.Infected)
                _animator.runtimeAnimatorController = _settings.SetRandomAnimatorController();

            _capsuleCollider.enabled = true;

            _isAlive = true;
            _currentHp = _settings.MaxHealth;
        }

        private void Update()
        {
            if (_isAlive)
            {
                PlayerChase();
                EnemyAttack();
            }
        }

        public void TakeDamage(int amount)
        {
            string damageInfo = amount switch
            {
                0 => "<color=red>MISS</color>",
                < 30 => $"<color=yellow>{amount.ToString()}</color>",
                _ => "<color=red>CRITICAL HIT</color>"
            };


            _animator.SetTrigger(_settings.AnimGetHitHashID);

            if (_isAlive)
            {
                _currentHp = Mathf.Max(0, _currentHp - amount);

                //    _healthBar.value = _currentHp;
                if (_currentHp == 0) Death();
            }
        }

        private void Death()
        {
            _isAlive = false;
            _capsuleCollider.enabled = false;
            _animator.SetBool(_settings.AnimIsAliveHashID, false);
            StartCoroutine(DisableGameObjectAfterDelay(_settings.GameObjectDisableDuration));

            OnEnemyKilledEvent?.Invoke(_settings.Experience);
        }

        private void PlayerChase()
        {
            _agent.SetDestination((_settings.GetPlayerTransform.position));

            if (_agent.hasPath)
            {
                Vector3 direction = (_agent.steeringTarget - transform.position).normalized;
                Vector3 animDirection = transform.InverseTransformDirection(direction);
                bool isFacingDirection = Vector3.Dot(direction, transform.forward) > 0.5f;

                _animator.SetFloat(_settings.AnimHorizontalVelocityHashID, isFacingDirection ? animDirection.x : 0,
                    0.5f,
                    Time.deltaTime);
                _animator.SetFloat(_settings.AnimVerticalVelocityHashID, isFacingDirection ? animDirection.z : 0, 0.5f,
                    Time.deltaTime);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction),
                    180 * Time.deltaTime);

                if (Vector3.Distance(transform.position, _agent.destination) < _agent.radius)
                    _agent.ResetPath();
            }
            else
            {
                _animator.SetFloat("Horizontal", 0, 0.25f, Time.deltaTime);
                _animator.SetFloat("Vertical", 0, 0.25f, Time.deltaTime);
            }
        }

        private void EnemyAttack()
        {
            float distance = Vector3.Distance(transform.position, _settings.GetPlayerTransform.position);
            bool canAttack = distance <= _settings.AttackDistance && _isAlive;

            _agent.isStopped = canAttack && !_animator.GetBool(_settings.AnimAttackHashID);
            _animator.SetBool(_settings.AnimAttackHashID, canAttack);

            if (canAttack)
            {
                _animator.SetFloat("Horizontal", 0, 0.25f, Time.deltaTime);
                _animator.SetFloat("Vertical", 0, 0.25f, Time.deltaTime);
            }

            if (canAttack && Time.time > _canAttakAttackInterval)
            {
                _canAttakAttackInterval = Time.time + _settings.AttackInterval;
                _settings.PlayerDamage.TakeDamage(_settings.Damage);
            }
        }

        private IEnumerator DisableGameObjectAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _settings.Drop(transform.position + transform.up * 0.5f);
            gameObject.SetActive(false);
        }

        public void EnemyGameObjectDisable() => gameObject.SetActive(false);

        public void PlayChaseSoundEffect(AnimationEvent arg)
        {
            AudioClip clip = _settings.GetZombieChaseSoundEffect();
            if (clip != null)
            {
                _audioSource.clip = clip;
                _audioSource.Play();
            }
        }

        public void PlayAttackSoundEffect(AnimationEvent arg)
        {
            AudioClip clip = _settings.GetZombieAttackSoundEffect();
            if (clip != null)
            {
                _audioSource.clip = clip;
                _audioSource.Play();
            }
        }

        public void PlayZombieFootStepsSoundEffect(AnimationEvent arg)
        {
            //....
        }
    }
}