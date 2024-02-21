using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System;
using UniRx;
using UnityEngine;

public class MainPlayerManager : MonoBehaviourPunCallbacks, IPunObservable, IPlayer, IDisposable
{
    const string USER_DATA_HEALTH = "health";

    public static GameObject LocalPlayerInstance;

    [SerializeField] private GameObject playerUiPrefab;

    bool IsFiring;
    private bool leavingRoom;

    private List<IDisposable> _disposables = new();

    //public ReactiveProperty<bool> BoostEffectExists { get; set; }
    //public ReactiveProperty<bool> InvulnEffectExists { get; set; }
    //public ReactiveProperty<bool> SlowdownEffectExists { get; set; }
    //public ReactiveProperty<bool> StunEffectExists { get; set; }

    public void Awake()
    {
        if (photonView.IsMine)
        {
            LocalPlayerInstance = gameObject;
        }

        //BoostEffectExists = new ReactiveProperty<bool>(false);

        DontDestroyOnLoad(gameObject);
        Subscribes();
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

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountInfoSuccess, null);
    }

    public void Update()
    {
        if (photonView.IsMine)
        {

        }
    }

    private void Subscribes()
    {
        //_disposables.AddRange(new List<IDisposable> {
        //    BoostEffectExists.Subscribe(flag => Debug.Log($"BoostEffectExists {flag}"))
        //});
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(this.IsFiring);
            //stream.SendNext(this.Health);
            //stream.SendNext(Mana);
        }
        else
        {
            // Network player, receive data
            IsFiring = (bool)stream.ReceiveNext();
            //Health = (float)stream.ReceiveNext();
            //Mana = (float)stream.ReceiveNext();
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
            //if (result.Data.ContainsKey(USER_DATA_HEALTH))
            //{
            //    var health = int.Parse(result.Data[keyData].Value);
            //    Health = health / 100;
            //    Debug.Log($"{USER_DATA_HEALTH} - {Health}");
            //}
        }, null);
    }

    public void Dispose() => _disposables.ForEach(d => d.Dispose());
    private void OnDestroy() => Dispose();
}
