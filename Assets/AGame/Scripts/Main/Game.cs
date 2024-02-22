using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        if (MainPlayerManager.LocalPlayerInstance == null)
        {
            PhotonNetwork.LocalPlayer.CustomProperties.Clear();
            PhotonNetwork.Instantiate(_playerPrefab.name, new Vector3(_playerSpawnPoint.position.x, 0, _playerSpawnPoint.position.z), Quaternion.identity, 0);
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
}
