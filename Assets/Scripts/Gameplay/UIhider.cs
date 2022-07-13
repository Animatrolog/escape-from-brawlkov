using UnityEngine;
using System.Collections.Generic;

public class UIhider : MonoBehaviour
{
    private List<PlayerChar> _players = new List<PlayerChar>();

    private void Update()
    {
        Vector3 ofsetPosition = new Vector3(0f, 0.4f, 0f);

        foreach (PlayerChar player in _players)
        {
            var playerMB = player as MonoBehaviour;
            if (playerMB != null)
            {
                Vector3 direction = ((playerMB.transform.position + ofsetPosition) - (transform.position + ofsetPosition)).normalized;

                if (Physics.Raycast(transform.position + ofsetPosition, direction, out RaycastHit hit))
                {
                    if (hit.collider.TryGetComponent<PlayerChar>(out PlayerChar visiblePlayer))
                    {
                        player.HideUI(false);
                    }
                    else 
                    {
                        player.HideUI(true);
                    }

                }
            }

        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<PlayerChar>(out PlayerChar player))
        {
            _players.Add(player);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent<PlayerChar>(out PlayerChar player))
        {
            player.HideUI(true);
            _players.Remove(player);

        }
    }
}
