using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Linq;

public class PlayFabLogin : MonoBehaviour
{
    private const string TITLE_ID = "F992C";
    private const string AUTH_GUID_KEY = "AUTH_GUID_KEY";

    void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            PlayFabSettings.staticSettings.TitleId = TITLE_ID;

        var needCreation = PlayerPrefs.HasKey(AUTH_GUID_KEY);
        var id = PlayerPrefs.GetString(AUTH_GUID_KEY, Guid.NewGuid().ToString());

        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
        {
            CustomId = id,
            CreateAccount = !needCreation,
        }, result =>
        {
            PlayerPrefs.SetString(AUTH_GUID_KEY, id);
            OnLoginSuccess(result);
        }, OnLoginError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Complete Login.");
        SetUserData(result.PlayFabId);
        //MakePurchase();
        GetInventory();
    }

    private void GetInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result => ShowInventory(result.Inventory), OnLoginError);
    }

    private void ShowInventory(List<ItemInstance> inventory)
    {
        var firstItem = inventory.First();
        Debug.Log($"{firstItem.ItemId}");
        // ItemInstanceId ид в инвенторе для использования.
        ConsumePotion(firstItem.ItemInstanceId);
    }

    private void ConsumePotion(string itemInstanceId)
    {
        PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
        {
            ConsumeCount = 1,
            ItemInstanceId = itemInstanceId,
        }, result =>
        {
            Debug.Log("Complete ConsumeItem");
        }, OnLoginError);
    }

    private void MakePurchase()
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            CatalogVersion = "main",
            ItemId = "health_potion_id",
            Price = 3,
            VirtualCurrency = "SC"
        }, result =>
        {
            Debug.Log("Complete PurchaseItem");
        }, OnLoginError);
    }

    private void SetUserData(string playFabId)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"time_recieve_daily_reward", DateTime.UtcNow.ToString() }
            }
        },
        result =>
        {
            Debug.Log("SetUserData");
            GetUserData(playFabId, "time_recieve_daily_reward");
        }, OnLoginError);
    }

    private void GetUserData(string playFabId, string keyData)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = playFabId
        }, result =>
        {
            if (result.Data.ContainsKey(keyData))
                Debug.Log($"{keyData} : {result.Data[keyData].Value}");
        }, OnLoginError);
    }

    private void OnLoginError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
