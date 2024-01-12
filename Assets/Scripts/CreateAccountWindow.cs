using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class CreateAccountWindow : AccountDataWindowBase
{
    [SerializeField] private InputField _mailField;
    [SerializeField] private Button _createAccountBtn;

    private string _mail;

    protected override void SubscriptionsElementsUi()
    {
        base.SubscriptionsElementsUi();

        _mailField.onValueChanged.AddListener(UpdateMail);
        _createAccountBtn.onClick.AddListener(CreateAccount);
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
        }, error =>
        {
            Debug.LogError($"Fail {error.ErrorMessage}");
        });
    }
}
