using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class Building : MonoBehaviourPunCallbacks
{
    [SerializeField] AlphaFader[] _objectsToHide;
    private PhotonView _photonView;
    private Player _player;
    private int _hidebleLayer;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (_player == null)
            _player = PlayerSpawner.yourPlayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player target))
        {
            other.gameObject.layer = LayerMask.NameToLayer("Hideble");
            ChangeChildsLayer(other.gameObject, LayerMask.NameToLayer("Hideble"));
            if (target != _player) return;
            foreach (AlphaFader objectToHide in _objectsToHide)
            {
                objectToHide.FadeOut();
            }
        }     
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player target))
        {
            other.gameObject.layer = 0;
            ChangeChildsLayer(other.gameObject, 0);
            if (target != _player) return;
            foreach (AlphaFader objectToHide in _objectsToHide)
            {
                objectToHide.FadeIn();
            }
        }
    }

    private void ChangeChildsLayer(GameObject gameObj, int layeNumber)
    {
        foreach (Transform child in gameObj.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = layeNumber;
        }
    }
}
