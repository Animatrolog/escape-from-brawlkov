using UnityEngine.InputSystem;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    private PlayerInput _playerInput;

    private Vector3 _scaleOnStart;
    private PlayerChar _player;

    private void Start()
    {
        _scaleOnStart = transform.localScale;
    }

    void Update()
    {
        if (_player != null) {

            _playerInput = _player.GetComponent<PlayerInput>();

            Vector2 moveInput = _playerInput.actions["Move"].ReadValue<Vector2>();

            if (moveInput.magnitude > 0)
            {
                transform.localScale = _scaleOnStart * moveInput.magnitude;
                transform.forward = new Vector3(moveInput.x, 0f, moveInput.y);
            }
            else transform.localScale = _scaleOnStart * 0;
        }
        else _player = PlayerChar.LocalPlayerInstance;
    }
}
