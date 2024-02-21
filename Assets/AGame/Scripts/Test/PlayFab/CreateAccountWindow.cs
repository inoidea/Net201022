using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class CreateAccountWindow : AccountDataWindowBase
{
    [SerializeField] private Canvas _enterInGameCanvas;
    [SerializeField] private Canvas _createAccountCanvas;
    [SerializeField] private InputField _mailField;
    [SerializeField] private Button _createAccountBtn;
    [SerializeField] private Button _backBtn;

    private string _mail;

    protected override void SubscriptionsElementsUi()
    {
        base.SubscriptionsElementsUi();

        _mailField.onValueChanged.AddListener(UpdateMail);
        _createAccountBtn.onClick.AddListener(CreateAccount);
        _backBtn.onClick.AddListener(Back);
    }

    private void Back()
    {
        _enterInGameCanvas.gameObject.SetActive(true);
        _createAccountCanvas.gameObject.SetActive(false);
    }

    private void UpdateMail(string mail)
    {
        _mail = mail;
    }

    private void CreateAccount()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest 
        { 
            Username = _username,
            Password = _password,
            Email = _mail
        }, result =>
        {
            Debug.Log($"Success {_username}");
            EnterInGameScene();
            SetUserData(result.PlayFabId);
        }, error =>
        {
            Debug.LogError($"Fail {error.ErrorMessage}");
        });
    }
}
