using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class MainPlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameObject LocalPlayerInstance;

    bool IsFiring;
    private bool leavingRoom;

    public void Awake()
    {
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

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountInfoSuccess, null);
    }

    public void Update()
    {
        if (photonView.IsMine)
        {

        }
    }

    public override void OnLeftRoom()
    {
        this.leavingRoom = false;
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

        //GetUserData(playFabId, USER_DATA_HEALTH);
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
}
