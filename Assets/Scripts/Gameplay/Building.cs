using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class Building : MonoBehaviourPunCallbacks
{
    [SerializeField] AlphaFader[] _objectsToHide;
    private PlayerChar _player;

    private void Update()
    {
        if (_player == null)
            _player = PlayerChar.LocalPlayerInstance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerChar>(out PlayerChar target))
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
        if (other.TryGetComponent<PlayerChar>(out PlayerChar target))
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
