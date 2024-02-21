using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System.Linq;

public class PlayfabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleLabel;
    [SerializeField] private GameObject _newCharacterPanel;
    [SerializeField] private Button _createCharacterBtn;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private List<SlotCharacterView> _slots;

    private string _characterName;

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnError);
        PlayFabServerAPI.GetRandomResultTables(new PlayFab.ServerModels.GetRandomResultTablesRequest 
        { 
            TableIDs = new List<string> { "table_id" }
        }, OnGetRandomResult, OnError);

        GetCharacters();

        foreach (var slot in _slots)
            slot.SlotButton.onClick.AddListener(OpenCreateNewCharacter);

        _inputField.onValueChanged.AddListener(OnNameChanged);
        _createCharacterBtn.onClick.AddListener(CreateCharacter);
    }

    private void CreateCharacter()
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        {
            CharacterName = _characterName,
            ItemId = "character_token_id"
        }, result =>
        {
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
                { "Gold", 0 }
            }
        }, result =>
        {
            Debug.Log("Complete UpdateCharacterStatistics");
            CloseCreateNewCharacter();
            GetCharacters();
        }, OnError);
    }

    private void OnNameChanged(string name)
    {
        _characterName = name;
    }

    private void OpenCreateNewCharacter()
    {
        _newCharacterPanel.SetActive(true);
    }

    private void CloseCreateNewCharacter()
    {
        _newCharacterPanel.SetActive(false);
    }

    private void GetCharacters()
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
            foreach (var slot in _slots) 
                slot.ShowEmptySlot();
        }  
        else if (characters.Count > 0 && characters.Count <= _slots.Count)
        {
            PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
            {
                CharacterId = characters.First().CharacterId
            }, result =>
            {
                var level = result.CharacterStatistics["Level"].ToString();
                var gold = result.CharacterStatistics["Gold"].ToString();

                _slots.First().ShowInfoCharacterSlot(characters.First().CharacterName, level, gold);
            }, OnError);
        }
        else
        {
            Debug.Log("Add slots for characters.");
        }
    }

    private void OnGetAccount(GetAccountInfoResult result)
    {
        var accountInfo = result.AccountInfo;
        _titleLabel.text += $"\n\nPlayerData:\nPlayFabID: {accountInfo.PlayFabId}\nMail: {accountInfo.PrivateInfo.Email}" +
            $"\nLastLogin: {accountInfo.TitleInfo.LastLogin}";
    }

    private void OnGetCatalogSuccess(GetCatalogItemsResult result)
    {
        Debug.Log("OnGetCatalogSuccess");
        ShowItems(result.Catalog);
    }

    private void ShowItems(List<CatalogItem> catalog)
    {
        _titleLabel.text += $"\n\nCatalogItem";

        foreach (var item in catalog)
        {
            _titleLabel.text += $"\nItemId: {item.ItemId}\tName: {item.DisplayName}";
        }
    }

    private void OnGetRandomResult(PlayFab.ServerModels.GetRandomResultTablesResult result)
    {
        Debug.Log($"DropTable{result.Tables.Count}");
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
