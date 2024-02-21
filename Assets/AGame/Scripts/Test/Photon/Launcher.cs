using UnityEngine;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{
    public CharacterModel characterModel;

    [SerializeField] private ServerSettings _serverSettings;
    [SerializeField] private Transform _placeForUI;
    [SerializeField] private GameObject _loadingScreenPrefab;
    [SerializeField] private GameObject _lobbyViewPrefab;
    [SerializeField] private GameObject _roomViewPrefab;

    private GameObject loadingScreenView;
    private GameObject lobbyView;
    private GameObject roomView;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        InitView();
        Connect();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"OnCreatedRoom {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnJoinedLobby()
    {
        loadingScreenView.SetActive(false);
        lobbyView.SetActive(true);
        roomView.SetActive(false);

        if (lobbyView.TryGetComponent(out LobbyView view))
            view.GetCharacters();

        Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} Joined Lobby");
    }

    public override void OnJoinedRoom()
    {
        loadingScreenView.SetActive(false);
        lobbyView.SetActive(false);
        roomView.SetActive(true);

        if (roomView.TryGetComponent(out RoomView room))
        {
            room.RoomName = PhotonNetwork.CurrentRoom.Name;

            var playersNames = "";

            foreach (var player in PhotonNetwork.CurrentRoom.Players)
                playersNames += $"{player.Value}\n";

            room.PlayersNames = playersNames;
        }

        Debug.Log($"OnJoinedRoom {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
    }

    private void Connect()
    {
        PhotonNetwork.ConnectUsingSettings(_serverSettings.AppSettings);
        PhotonNetwork.GameVersion = Application.version;

        Debug.Log("Connect");
    }

    private void InitView()
    {
        loadingScreenView = Instantiate(_loadingScreenPrefab, _placeForUI);
        lobbyView = Instantiate(_lobbyViewPrefab, _placeForUI);
        roomView = Instantiate(_roomViewPrefab, _placeForUI);

        loadingScreenView.SetActive(true);
        lobbyView.gameObject.SetActive(false);
        roomView.gameObject.SetActive(false);
    }

    private void Disconnect()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Disconnect");
    }
}
