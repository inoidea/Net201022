using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomView : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _roomName;
    [SerializeField] private Button _startGameBtn;
    [SerializeField] private Button _leaveRoomBtn;

    public string RoomName { get { return _roomName.text; } set { _roomName.text = value; } }

    private void Start()
    {
        _startGameBtn.onClick.AddListener(StartGame);
        _leaveRoomBtn.onClick.AddListener(LeaveRoom);
    }

    private void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;

        Debug.Log("START GAME");
    }
}
