using Photon.Pun;
using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game : MonoBehaviourPun
{
    [Header("UI")]
    [SerializeField] private Transform _containerForUI;
    [SerializeField] private SkillsView _skillsView;

    [Header("Player")]
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private GameObject _playerPrefab;

    [Header("Skills")]
    [SerializeField] private List<Transform> _skillSpawnPoints;
    [SerializeField] private GameObject _skillPrefab;

    [SerializeField] private TMP_Text _victoryPanelText;

    private void Awake()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountInfoSuccess, null);
    }

    void Start()
    {
        if (MainPlayerManager.LocalPlayerInstance == null)
        {
            var player = PhotonNetwork.Instantiate(_playerPrefab.name, new Vector3(_playerSpawnPoint.position.x, 0, _playerSpawnPoint.position.z), Quaternion.identity, 0);
            player.transform.Rotate(0, -90 ,0);
            player.name = PhotonNetwork.LocalPlayer.NickName;
        }
        else
        {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }

        //// skills view
        //SkillsView skillsView = Instantiate(_skillsView, _containerForUI);
        //SkillsPresenter skillsPresenter = skillsView.gameObject.GetComponent<SkillsPresenter>();

        ////if (photonView.IsMine)
        ////{
        //    // spawn skills
        //    foreach (var spawnPoint in _skillSpawnPoints)
        //    {
        //        GameObject skillGO = PhotonNetwork.Instantiate(_skillPrefab.name, spawnPoint.position, Quaternion.identity);
        //        Skill skill = skillGO.GetComponent<Skill>();
        //        skill.SkillsPresenter = skillsPresenter;
        //    }
        ////}
    }

    private void OnGetAccountInfoSuccess(GetAccountInfoResult result)
    {
        GetUserData(result.AccountInfo.PlayFabId, Constants.USER_DATA_VICTORY);
    }

    private void GetUserData(string playFabId, string keyData)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = playFabId
        }, result =>
        {
            if (result.Data.ContainsKey(Constants.USER_DATA_VICTORY))
            {
                _victoryPanelText.text = result.Data[keyData].Value;
                Debug.Log($"GET {Constants.USER_DATA_VICTORY} - {result.Data[keyData].Value}");
            }
        }, result => { Debug.Log($"{Constants.USER_DATA_VICTORY} - ERROR"); });
    }
}
