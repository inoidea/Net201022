using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

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
    }

    private void OnLoginError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
