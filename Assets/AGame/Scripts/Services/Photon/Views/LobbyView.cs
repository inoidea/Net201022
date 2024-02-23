using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

public class LobbyView : MonoBehaviourPunCallbacks
{
    const string CHARACTER_TOKEN_ID = "character_token_id";

    [SerializeField] private TMP_InputField _newRoomName;
    [SerializeField] private Button _createRoomBtn;

    [SerializeField] private GameObject _roomListView;
    [SerializeField] private RoomForConnectView _roomForConnectPrefab;

    [SerializeField] private GameObject _newCharacterPanel;
    [SerializeField] private TMP_InputField _characterNameField;
    [SerializeField] private Button _createCharacterBtn;
    [SerializeField] private List<CharacterSlotView> _characterSlots;

    private List<RoomInfo> _roomInfoList = new List<RoomInfo>();
    string[] friendList = { "Friend1", "Friend2", "Friend3" };

    private string _characterName;

    private void Start()
    {
        CloseCreateNewCharacter();

        _createRoomBtn.onClick.AddListener(CreateRoom);
        _createCharacterBtn.onClick.AddListener(CreateCharacter);

        _characterNameField.onValueChanged.AddListener(OnNameChanged);

        foreach (var slot in _characterSlots)
            slot.SlotButton.onClick.AddListener(OpenCreateNewCharacter);
    }

    private void OnNameChanged(string name)
    {
        _characterName = name;
    }

    private void CreateCharacter()
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        {
            CharacterName = _characterName,
            ItemId = CHARACTER_TOKEN_ID
        }, result =>
        {
            CloseCreateNewCharacter();
            UpdateCharacterStatistics(result.CharacterId);
            Debug.Log("Complete CreateCharacter");
        }, OnError);
    }

    private void UpdateCharacterStatistics(string characterId)
    {
        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
        {
            CharacterId = characterId,
            CharacterStatistics = new Dictionary<string, int>
            {
                { "Level", 1 },
                { "Damage", 1 },
                { "Health", 100 },
                { "Experience", 0 },
                { "Gold", 0 }
            }
        }, result =>
        {
            Debug.Log("Complete UpdateCharacterStatistics");

            GetCharacters();
        }, OnError);
    }

    public void GetCharacters()
    {
        PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(), result =>
        {
            Debug.Log($"Character count {result.Characters.Count}");
            ShowCharactersInSlot(result.Characters);
        }, OnError);
    }

    private void ShowCharactersInSlot(List<CharacterResult> characters)
    {
        if (characters.Count == 0)
        {
            foreach (var slot in _characterSlots)
                slot.ShowEmptySlot();
        }
        else if (characters.Count > 0 && characters.Count <= _characterSlots.Count)
        {
            var slotIndex = 0;

            foreach(var character in characters)
                PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
                {
                    CharacterId = character.CharacterId
                }, result =>
                {
                    var name = character.CharacterName;
                    var level = result.CharacterStatistics["Level"];
                    var damage = result.CharacterStatistics["Damage"];
                    var health = result.CharacterStatistics["Health"];
                    var experience = result.CharacterStatistics["Experience"];
                    var gold = result.CharacterStatistics["Gold"];

                    var charModel = new CharacterModel(name, level, damage, health, experience);
                    _characterSlots[slotIndex].ShowCharacterInfoSlot(charModel);
                    slotIndex++;
                }, OnError);
        }
        else
        {
            Debug.Log("Add slots for characters.");
        }
    }

    private void OpenCreateNewCharacter()
    {
        _newCharacterPanel.SetActive(true);
    }

    private void CloseCreateNewCharacter()
    {
        _newCharacterPanel.SetActive(false);
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

    private void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
