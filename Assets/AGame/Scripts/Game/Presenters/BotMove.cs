using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotMove : MonoBehaviourPun, IMove
{
    [SerializeField] private NavMeshAgent _meshAgent;
    [SerializeField] private List<Transform> _movePoints;

    private float _currentMoveSpeed = 15f;
    private float _moveSpeed;
    private float _rotationSpeed = 50f;
    private bool _invulnerable;
    private bool _stunned;
    private int _pointIndex = 0;
    private AudioSource _audioSource;

    public float Speed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public bool Invulnerable { get { return _invulnerable; } set { _invulnerable = value; } }
    public bool Stunned { get { return _stunned; } set { _stunned = value; } }
    public Transform PlayerTransform => transform;
    public int PlayerViewID => photonView.ViewID;

    private void Awake()
    {
        _moveSpeed = _currentMoveSpeed;
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;

        _meshAgent.speed = _moveSpeed;

        Move();
    }

    private void Move()
    {
        if (_movePoints.Count > 0 && _pointIndex < _movePoints.Count)
        {
            if (Speed > 0)
            {
                if (!_audioSource.isPlaying)
                    _audioSource.Play();
            }
            else
                _audioSource.Pause();

            var newPoint = _movePoints[_pointIndex].position;
            float distance = Vector3.Distance(transform.position, newPoint);

            if (distance > 0.5f)
                _meshAgent.destination = newPoint;
            else
                _pointIndex++;
        }
    }

    public void UseSkillOnPlayer(int targetPlayerID, SkillTypes skillType) { }
}
