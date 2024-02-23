using System.Collections.Generic;
using Photon.Pun;
using PlayFab.ClientModels;
using PlayFab;
using TMPro;
using UnityEngine;
using System;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject _finalPanel;
    [SerializeField] private TMP_Text _victoryPanelText;
    [SerializeField] private TMP_Text _checkPointCountText;
    [SerializeField] private int _checkPointMaxCount;
    [SerializeField] private bool _finishPoint;
    [SerializeField] private GameObject _botGO;

    private bool _winnerExists = false;

    private void Start()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEventReceived;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PhotonView playerPhotonView))
        {
            if (playerPhotonView.IsMine)
            {
                var skillProps = PhotonNetwork.LocalPlayer.CustomProperties;
                skillProps[Constants.CHECKPOINTS_PASSED] = (int)skillProps[Constants.CHECKPOINTS_PASSED] + 1;

                _checkPointCountText.text = $"{(int)skillProps[Constants.CHECKPOINTS_PASSED]}/{_checkPointMaxCount}";

                if (_finishPoint && ((int)skillProps[Constants.CHECKPOINTS_PASSED] >= _checkPointMaxCount))
                {
                    _winnerExists = true;
                }

                PhotonNetwork.LocalPlayer.SetCustomProperties(skillProps);
            }

            if (_winnerExists)
            {
                OnGameOver();

                //PhotonNetwork.Instantiate(_finalPanel.name, _containerForUI.position, Quaternion.identity);

                //SetWindowText("Game over");

                PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnUpdateAccountInfoSuccess, null);

                //PhotonView.Get(this).RPC("StopGameRPC", RpcTarget.All);
            }
        }
    }

    private void OnUpdateAccountInfoSuccess(GetAccountInfoResult result)
    {
        UpdateUserData();
    }

    private void UpdateUserData()
    {
        var oldValue = Convert.ToInt32(_victoryPanelText.text);
        var currentValue = oldValue + 1;

        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>
            {
                { Constants.USER_DATA_VICTORY, currentValue.ToString() }
            }
        }, result =>
        {
            _victoryPanelText.text = $"{currentValue}";
            Debug.Log($"UPDATE {Constants.USER_DATA_VICTORY} - {currentValue}");
        }, result => { Debug.Log($"{Constants.USER_DATA_VICTORY} - ERROR"); });
    }

    public void SetWindowText(string text)
    {
        // Отправляем событие об изменении содержимого окна всем пользователям
        //object[] content = new object[] { text };
        //PhotonNetwork.RaiseEvent(1, content, new RaiseEventOptions { Receivers = ReceiverGroup.All });
    }

    private void OnEventReceived(EventData obj)
    {
        if (obj.Code == 1)
        {
            object[] content = (object[])obj.CustomData;
            string text = (string)content[0];

            _finalPanel.SetActive(true);
        }
    }

    private void OnGameOver()
    {
        if (_botGO.TryGetComponent(out AudioSource botAudioSource)) { 
            if (botAudioSource.isPlaying)
            {
                botAudioSource.Stop();
                botAudioSource.clip = null;
            }
        }

        if (MainPlayerManager.LocalPlayerInstance.gameObject.TryGetComponent(out AudioSource playerAudioSource))
        {
            if (playerAudioSource.isPlaying)
            {
                playerAudioSource.Stop();
                playerAudioSource.clip = null;
            }
        }

        Time.timeScale = 0;
        _finalPanel.SetActive(true);
    }

    [PunRPC]
    private void StopGameRPC()
    {
        Time.timeScale = 0f;
    }

    private void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
    }
}
