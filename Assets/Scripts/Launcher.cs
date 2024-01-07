using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _disconnectBtn;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        _disconnectBtn.onClick.AddListener(Disconnect);
    }

    private void Start()
    {
        Connect();
    }

    private void Connect()
    {
        if (!PhotonNetwork.IsConnected) return;

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = Application.version;

        Debug.Log("Connect");
    }

    private void Disconnect()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Disconnect");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
    }
}
