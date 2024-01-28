using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomView : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _roomName;
    [SerializeField] private TMP_Text _playersNames;
    [SerializeField] private Button _startGameBtn;
    [SerializeField] private Button _leaveRoomBtn;

    public string RoomName { get { return _roomName.text; } set { _roomName.text = value; } }
    public string PlayersNames { get { return _playersNames.text; } set { _playersNames.text = value; } }

    private void Start()
    {
        _startGameBtn.onClick.AddListener(StartGame);
        _leaveRoomBtn.onClick.AddListener(LeaveRoom);

        Player roomOwner = PhotonNetwork.PlayerList[0];
        _startGameBtn.interactable = (PhotonNetwork.LocalPlayer.UserId == roomOwner.UserId);
    }

    private void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        SceneManager.LoadScene("Game");
    }
}
