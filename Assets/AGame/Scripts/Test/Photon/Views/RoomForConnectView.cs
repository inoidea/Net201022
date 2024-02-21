using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomForConnectView : MonoBehaviour
{
    [SerializeField] private TMP_Text _roomName;
    [SerializeField] private TMP_Text _playerCount;
    [SerializeField] private Button _connectRoomBtn;

    //public string RoomName { get { return _roomName.text; } set { _roomName.text = value; } }   
    //public string PlayerCount { get { return _playerCount.text; } set { _playerCount.text = value; } }   

    private void Start()
    {
        _connectRoomBtn.onClick.AddListener(JoinRoom);
    }

    private void JoinRoom()
    {
        PhotonNetwork.JoinRoom(_roomName.text);
    }

    public void UpdateRoomInfo(string roomName, int playerCount, int maxPlayers)
    {
        _roomName.text = roomName;
        _playerCount.text = $"{playerCount}/{maxPlayers}";
    }
}
