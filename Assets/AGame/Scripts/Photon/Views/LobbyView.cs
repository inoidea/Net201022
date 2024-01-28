using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using System.Collections.Generic;

public class LobbyView : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _newRoomName;
    [SerializeField] private Button _createRoomBtn;

    [SerializeField] private GameObject _roomListView;
    [SerializeField] private RoomForConnectView _roomForConnectPrefab;

    private List<RoomInfo> _roomInfoList = new List<RoomInfo>();
    string[] friendList = { "Friend1", "Friend2", "Friend3" };

    private void Start()
    {
        _createRoomBtn.onClick.AddListener(CreateRoom);
    }

    private void CreateRoom()
    {
        if (_newRoomName.text == null) return;

        PhotonNetwork.CreateRoom(_newRoomName.text);
    }

    private void CreateRoomForFriends()
    {
        PhotonNetwork.CreateRoom(_newRoomName.text, null, null, friendList);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (_roomInfoList.Count == 0)
        {
            _roomInfoList = roomList;
        }
        else
        {
            foreach (RoomInfo room in roomList)
            {
                if (_roomInfoList.Exists(r => r.Name == room.Name))
                {
                    _roomInfoList.Remove(room);

                    if (room.PlayerCount > 0)
                        _roomInfoList.Add(room);
                }
                else
                    _roomInfoList.Add(room);
            }
        }

        UpdateRoomListView();
    }

    private void UpdateRoomListView()
    {
        foreach (Transform child in _roomListView.transform)
            Destroy(child.gameObject);

        foreach (RoomInfo room in _roomInfoList)
        {
            if (room.PlayerCount > 0)
            {
                RoomForConnectView roomView = Instantiate(_roomForConnectPrefab, _roomListView.transform);
                roomView.UpdateRoomInfo(room.Name, room.PlayerCount, room.MaxPlayers);
            }
        }
    }
}
