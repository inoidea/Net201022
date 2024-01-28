using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class SignInWindow : AccountDataWindowBase
{
    [SerializeField] private Canvas _enterInGameCanvas;
    [SerializeField] private Canvas _signInCanvas;
    [SerializeField] private Button _signInBtn;
    [SerializeField] private Button _backBtn;
    [SerializeField] private TMP_Text _loadingLabel;

    protected override void SubscriptionsElementsUi()
    {
        base.SubscriptionsElementsUi();

        _signInBtn.onClick.AddListener(SignIn);
        _backBtn.onClick.AddListener(Back);
    }
    private void Back()
    {
        _enterInGameCanvas.gameObject.SetActive(true);
        _signInCanvas.gameObject.SetActive(false);
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
            SetUserData(result.PlayFabId);
        }, error =>
        {
            Debug.LogError($"Fail {error.ErrorMessage}");
        });
    }
}
