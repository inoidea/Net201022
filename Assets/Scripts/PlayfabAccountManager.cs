using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleLabel;

    private UserAccountInfo _accountInfo;

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
    }

    private void OnGetAccount(GetAccountInfoResult result)
    {
        var accountInfo = result.AccountInfo;
        _accountInfo = accountInfo;
        _titleLabel.text = $"PlayFabID: {accountInfo.PlayFabId}\nMail: {accountInfo.PrivateInfo.Email}" +
            $"\nLastLogin: {accountInfo.TitleInfo.LastLogin}";
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
