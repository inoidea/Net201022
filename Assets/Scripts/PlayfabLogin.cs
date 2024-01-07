using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;

public class PlayfabLogin : MonoBehaviour
{
    const string TITLE_ID = "F992C";

    [SerializeField] private TMP_Text _connectLabel;
    [SerializeField] private Image _connectionIndicator;

    void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            PlayFabSettings.staticSettings.TitleId = TITLE_ID;

        var request = new LoginWithCustomIDRequest
        {
            CustomId = "Player 1",
            CreateAccount = true,
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        _connectLabel.text = "Connected";
        _connectionIndicator.color = Color.green;

        Debug.Log("Complete Login.");
    }

    private void OnLoginError(PlayFabError error)
    {
        _connectLabel.text = "Not connected";
        _connectionIndicator.color = Color.red;

        var errorMessage = error.GenerateErrorReport();
        Debug.LogError(errorMessage);
    }
}
