using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainPlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    const string USER_DATA_HEALTH = "health";

    public float Health = 1f;
    public float Mana = 1f;
    public static GameObject LocalPlayerInstance;

    [SerializeField] private GameObject playerUiPrefab;
    [SerializeField] private GameObject beams;

    bool IsFiring;
    private bool leavingRoom;

    public void Awake()
    {
        if (beams == null)
        {
            Debug.LogError("<Color=Red><b>Missing</b></Color> Beams Reference.", this);
        }
        else
        {
            beams.SetActive(false);
        }

        if (photonView.IsMine)
        {
            LocalPlayerInstance = gameObject;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        CameraWork _cameraWork = gameObject.GetComponent<CameraWork>();

        if (_cameraWork != null)
        {
            if (photonView.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><b>Missing</b></Color> CameraWork Component on player Prefab.", this);
        }

        // Create the UI
        if (this.playerUiPrefab != null)
        {
            GameObject _uiGo = Instantiate(this.playerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
        else
        {
            Debug.LogWarning("<Color=Red><b>Missing</b></Color> PlayerUiPrefab reference on player Prefab.", this);
        }

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountInfoSuccess, null);
    }

    public void Update()
    {
        if (photonView.IsMine)
        {
            this.ProcessInputs();

            if (this.Health <= 0f && !this.leavingRoom)
            {
                this.leavingRoom = GameManager.Instance.LeaveRoom();
            }
        }

        if (this.beams != null && this.IsFiring != this.beams.activeInHierarchy)
        {
            this.beams.SetActive(this.IsFiring);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }


        // We are only interested in Beamers
        // we should be using tags but for the sake of distribution, let's simply check by name.
        if (!other.name.Contains("Beam"))
        {
            return;
        }

        this.Health -= 0.1f;
    }

    public void OnTriggerStay(Collider other)
    {
        // we dont' do anything if we are not the local player.
        if (!photonView.IsMine)
        {
            return;
        }

        // We are only interested in Beamers
        // we should be using tags but for the sake of distribution, let's simply check by name.
        if (!other.name.Contains("Beam"))
        {
            return;
        }

        // we slowly affect health when beam is constantly hitting us, so player has to move to prevent death.
        this.Health -= 0.1f * Time.deltaTime;
    }

    public override void OnLeftRoom()
    {
        this.leavingRoom = false;
    }

    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }

        GameObject _uiGo = Instantiate(this.playerUiPrefab);
        _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }

    void ProcessInputs()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // we don't want to fire when we interact with UI buttons for example. IsPointerOverGameObject really means IsPointerOver*UI*GameObject
            // notice we don't use on on GetbuttonUp() few lines down, because one can mouse down, move over a UI element and release, which would lead to not lower the isFiring Flag.
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //	return;
            }

            if (!this.IsFiring)
            {
                this.IsFiring = true;
                Mana -= 0.1f;
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (this.IsFiring)
            {
                this.IsFiring = false;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(this.IsFiring);
            stream.SendNext(this.Health);
            stream.SendNext(Mana);
        }
        else
        {
            // Network player, receive data
            this.IsFiring = (bool)stream.ReceiveNext();
            this.Health = (float)stream.ReceiveNext();
            Mana = (float)stream.ReceiveNext();
        }
    }

    private void OnGetAccountInfoSuccess(GetAccountInfoResult result)
    {
        var playFabId = result.AccountInfo.PlayFabId;

        GetUserData(playFabId, USER_DATA_HEALTH);
    }

    private void GetUserData(string playFabId, string keyData)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = playFabId
        }, result =>
        {
            if (result.Data.ContainsKey(USER_DATA_HEALTH))
            {
                var health = int.Parse(result.Data[keyData].Value);
                Health = health / 100;
                Debug.Log($"{USER_DATA_HEALTH} - {Health}");
            }
        }, null);
    }
}
