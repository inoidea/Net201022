using Photon.Pun;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _playerPrefab;

    void Start()
    {
        if (MainPlayerManager.LocalPlayerInstance == null)
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

            foreach (var player in PhotonNetwork.CurrentRoom.Players)
                PhotonNetwork.Instantiate(_playerPrefab.name, new Vector3(_spawnPoint.position.x, 1, _spawnPoint.position.z), Quaternion.identity, 0);
        }
        else
        {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }
    }
}
