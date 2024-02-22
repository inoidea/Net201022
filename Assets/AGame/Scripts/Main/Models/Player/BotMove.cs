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

    public float Speed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public bool Invulnerable { get { return _invulnerable; } set { _invulnerable = value; } }
    public bool Stunned { get { return _stunned; } set { _stunned = value; } }
    public Transform PlayerTransform => transform;

    private void Awake()
    {
        _moveSpeed = _currentMoveSpeed;
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
            var newPoint = _movePoints[_pointIndex].position;
            float distance = Vector3.Distance(transform.position, newPoint);

            if (distance > 0.5f)
                _meshAgent.destination = newPoint;
            else
                _pointIndex++;
        }
    }
}
