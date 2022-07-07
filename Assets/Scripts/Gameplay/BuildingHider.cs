using UnityEngine;

[RequireComponent(typeof(AlphaFader))]
public class BuildingHider : MonoBehaviour
{
    [SerializeField] private float _zOffset;
    private PlayerChar _player;
    private AlphaFader _aphaFader;

    void Start()
    {
        _aphaFader = GetComponent<AlphaFader>();
        _player = PlayerChar.LocalPlayerInstance;
    }

    void Update()
    {
        if (_player != null)
        {
            Debug.Log("its happening");
            if (_player.transform.position.z >= transform.position.z - _zOffset)
            {
                _aphaFader.FadeIn();
            }
            else
            {
                _aphaFader.FadeOut();
            }
        }
        else _player = PlayerChar.LocalPlayerInstance;
    }
}
