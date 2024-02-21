using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private GameObject _playerPrefab;

    [Header("Skills")]
    [SerializeField] private List<Transform> _effectSpawnPoints;
    [SerializeField] private GameObject _effectPrefab;

    void Start()
    {
        if (MainPlayerManager.LocalPlayerInstance == null)
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

            //Debug.Log(PhotonNetwork.CurrentRoom.Players.Count);
            //foreach (var player in PhotonNetwork.CurrentRoom.Players)
                PhotonNetwork.Instantiate(_playerPrefab.name, new Vector3(_playerSpawnPoint.position.x, 1, _playerSpawnPoint.position.z), Quaternion.identity, 0);
            //Instantiate(_playerPrefab, new Vector3(_spawnPoint.position.x, 1, _spawnPoint.position.z), Quaternion.identity); 
        }
        else
        {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var spawnPoint in _effectSpawnPoints)
                PhotonNetwork.Instantiate(_effectPrefab.name, spawnPoint.position, Quaternion.identity);
        }
    }
}
