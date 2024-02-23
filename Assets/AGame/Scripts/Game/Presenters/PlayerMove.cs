using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerMove : MonoBehaviourPun, IMove
{
    private float _currentMoveSpeed = 15f;
    private float _moveSpeed;
    private float _rotationSpeed = 50f;
    private bool _invulnerable;
    private bool _stunned;
    private float _skillDuration = 7f;

    public float Speed { get { return _moveSpeed; } set { _moveSpeed = (Invulnerable) ? _moveSpeed : value; } }
    public bool Invulnerable { get { return _invulnerable; } set { _invulnerable = value; if (Speed == 0) Speed = _currentMoveSpeed; } }
    public bool Stunned { get { return _stunned; } set { _stunned = value; } }
    public Transform PlayerTransform => transform;
    public int PlayerViewID => photonView.ViewID;

    private AudioSource _audioSource;

    private void Awake()
    {
        _moveSpeed = _currentMoveSpeed;
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(0f, 0f, verticalInput);
        float rotation = horizontalInput * _rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotation, 0f);
        transform.Translate(movement * _moveSpeed * Time.deltaTime);

        if (Speed > 0)
        {
            if (!_audioSource.isPlaying)
                _audioSource.Play();
        }
        else
            _audioSource.Pause();
    }

    public void UseSkillOnPlayer(int targetPlayerID, SkillTypes skillType)
    {
        // Отправляем сетевое сообщение с информацией об умении и цели (ID другого игрока)
        object[] data = new object[] { targetPlayerID };
        PhotonNetwork.RaiseEvent((byte)skillType, data, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    // Обработка сетевого сообщения
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnEvent(EventData photonEvent)
    {
        if ((photonEvent.Code == (int)SkillTypes.Slowdown) || (photonEvent.Code == (int)SkillTypes.Stun))
        {
            object[] data = (object[])photonEvent.CustomData;
            int targetPlayerID = (int)data[0];

            GameObject targetPlayer = PhotonView.Find(targetPlayerID).gameObject;

            if (targetPlayer.TryGetComponent(out IMove playerMove))
            {
                //ITimer timer = _timerPool.CreateCallBackTimer($"Slowdown {targetPlayerID}");

                var prevSpeed = Speed;
                Speed = (photonEvent.Code == (int)SkillTypes.Slowdown) ? prevSpeed / 2 : 0;
                //_timerPool.RunTimer(timer, _skillDuration, () => { playerMove.Speed = prevSpeed; });
            }
        }
    }
}
