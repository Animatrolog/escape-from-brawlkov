using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Vector3 _spawnLocation;
    public static Player yourPlayer;

    void Start()
    {
        GameObject player;
        player = PhotonNetwork.Instantiate(_playerPrefab.name, _spawnLocation, Quaternion.identity) as GameObject;
    }
}
