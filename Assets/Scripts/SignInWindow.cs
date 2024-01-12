using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class SignInWindow : AccountDataWindowBase
{
    [SerializeField] private Button _signInBtn;
    [SerializeField] private TMP_Text _loadingLabel;

    protected override void SubscriptionsElementsUi()
    {
        base.SubscriptionsElementsUi();

        _signInBtn.onClick.AddListener(SignIn);
    }

    private void SignIn()
    {
        _loadingLabel.gameObject.SetActive(true);

        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _username,
            Password = _password
        }, result =>
        {
            Debug.Log($"Success {_username}");
            EnterInGameScene();
        }, error =>
        {
            Debug.LogError($"Fail {error.ErrorMessage}");
        });

        //_loadingLabel.gameObject.SetActive(false);
    }
}
