using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AccountDataWindowBase : MonoBehaviour
{
    const string USER_DATA_HEALTH = "health";

    [SerializeField] private InputField _usernameField;
    [SerializeField] private InputField _passwordField;

    protected string _username;
    protected string _password;

    private void Start()
    {
        SubscriptionsElementsUi();
    }

    protected virtual void SubscriptionsElementsUi()
    {
        _usernameField.onValueChanged.AddListener(UpdateUsername);
        _passwordField.onValueChanged.AddListener(UpdatePassword);
    }

    private void UpdateUsername(string username)
    {
        _username = username;
    }

    private void UpdatePassword(string password)
    {
        _password = password;
    }

    protected void EnterInGameScene()
    {
        PhotonNetwork.LocalPlayer.NickName = _username;

        SceneManager.LoadScene("PhotonLobby");
    }

    protected void SetUserData(string playFabId)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {USER_DATA_HEALTH, "100" }
            }
        },
        result =>
        {
            Debug.Log("SetUserData");
        }, null);
    }
}
