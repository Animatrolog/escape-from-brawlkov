using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]

public class DestroyIFNotOwn : MonoBehaviour
{
    private PhotonView _photonView;

    void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!_photonView.IsMine) Destroy(gameObject);    
    }
}
