using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class PlayfabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleLabel;

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnError);
        PlayFabServerAPI.GetRandomResultTables(new PlayFab.ServerModels.GetRandomResultTablesRequest 
        { 
            TableIDs = new List<string> { "table_id" }
        }, OnGetRandomResult, OnError);
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
