using Photon.Pun;
using UnityEngine;

public class PlayerMove : MonoBehaviourPun, IMove
{
    private float _currentMoveSpeed = 15f;
    private float _moveSpeed;
    private float _rotationSpeed = 50f;
    private bool _invulnerable;
    private bool _stunned;

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

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //if (!_stunned)
        //{
        Debug.Log(_moveSpeed);
            Vector3 movement = new Vector3(0f, 0f, verticalInput);
            float rotation = horizontalInput * _rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, rotation, 0f);
            transform.Translate(movement * _moveSpeed * Time.deltaTime);
        //}
    }
}
