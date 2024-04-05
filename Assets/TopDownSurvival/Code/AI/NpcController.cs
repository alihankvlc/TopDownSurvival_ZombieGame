using DeadNation;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private float _playerDetectionRadius;
    [SerializeField] private float _talkingDuration = 3f;

    private AudioSource _audioSource;

    [SerializeField] private AudioClip[] _npcVoices;
    private Animator _animator;
    private int _focusToPlayerAnimHashId;
    private bool _canTalk = true;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _focusToPlayerAnimHashId = Animator.StringToHash("Focus");
    }

    private void Update()
    {
        TalkToPlayer();
    }

    private void TalkToPlayer()
    {
        Vector3 targetPosition = PlayerController.Instance.PlayerTransform.position;
        Vector3 position = transform.position;

        Vector3 directionToTarget = (targetPosition - position).normalized;

        float dot = Vector3.Dot(transform.forward, directionToTarget);
        float distance = Vector3.Distance(position, targetPosition);

        if (dot > 0 && distance < 3 && _canTalk)
        {
            int randomIndex = Random.Range(0, _npcVoices.Length);
            _audioSource.clip = _npcVoices[randomIndex];
            _audioSource.Play();

            _canTalk = false;
        }
        else if (distance > 5)
        {
            _canTalk = true;
        }

        _animator.SetBool(_focusToPlayerAnimHashId, dot > 0 && distance < 3);
    }
}