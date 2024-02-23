using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateAndJoinRandomLb : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
{
    private const string GAME_MODE_KEY = "gm";
    private const string AI_MODE_KEY = "ai";

    private const string MAP_PROP_KEY = "C0";
    private const string GOLD_PROP_KEY = "C1";

    [SerializeField] private ServerSettings _serverSettings;
    [SerializeField] private TMP_Text _stateUiText;
    [SerializeField] private GameObject _roomsUi;
    [SerializeField] private RoomForConnectView _roomPrefab;

    private LoadBalancingClient _lbc;

    private TypedLobby _sqlLobby = new TypedLobby("customSqlLobby", LobbyType.SqlLobby);

    private void Start()
    {
        _lbc = new LoadBalancingClient();
        _lbc.AddCallbackTarget(this);
        _lbc.ConnectUsingSettings(_serverSettings.AppSettings);
    }

    private void Update()
    {
        if (_lbc == null) return;

        // Подключение к комнатам, передача информации.
        _lbc.Service();

        _stateUiText.text = _lbc.State.ToString();
    }

    private void OnDestroy()
    {
        _lbc.RemoveCallbackTarget(this);
    }

    public void OnConnected()
    {
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");

        var roomOptions = new RoomOptions
        {
            MaxPlayers = 4,
            PublishUserId = true,
            CustomRoomPropertiesForLobby = new[] { MAP_PROP_KEY, GOLD_PROP_KEY },
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { GOLD_PROP_KEY, 400 }, { MAP_PROP_KEY, "Map3" } }
        };

        var enterRoomParams = new EnterRoomParams 
        { 
            RoomName = "red room", 
            RoomOptions = roomOptions,
            // ИД пользователей, для которых резервируется место в комнате.
            ExpectedUsers = new [] {"3454"},
            Lobby = _sqlLobby
        };

        _lbc.OpCreateRoom(enterRoomParams);
    }

    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
    }

    public void OnDisconnected(DisconnectCause cause)
    {
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
    }

    public void OnJoinedLobby()
    {
        var sqlLobbyFilter = $"{MAP_PROP_KEY} = 'Map3' AND {GOLD_PROP_KEY} BETWEEN 300 AND 500";
        var opJoinRandomRoomParams = new OpJoinRandomRoomParams
        {
            SqlLobbyFilter = sqlLobbyFilter
        };

        _lbc.OpJoinRandomRoom(opJoinRandomRoomParams);
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        //_lbc.CurrentRoom.Players.Values.First().UserId
        //_lbc.CurrentRoom.IsOpen = false;
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
        _lbc.OpCreateRoom(new EnterRoomParams());
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
    }

    public void OnLeftLobby()
    {
    }

    public void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //roomList.First().Name
        Debug.Log($"OnRoomListUpdate {roomList.Count}");
    }
}
