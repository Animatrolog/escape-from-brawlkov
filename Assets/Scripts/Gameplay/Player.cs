using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Photon.Pun;

public class Player : MonoBehaviour , IShootable
{
    [SerializeField] private float _playerSpeed = 2.0f;
    [SerializeField] private float _gravityValue = -9.81f;
    [SerializeField] float _rotationSpeed = 10f;
    [SerializeField] Animator _animator;
    [SerializeField] private Gun _gun;
    [SerializeField] private float _health;
    //[SerializeField] TrajectoryDrawer _trajectoryDrawer;

    private AimFocuser _focuser;
    private CharacterController _characterController;
    private Vector3 _playerVelocity;
    private Vector3 _lastLook;
    private bool _isGrounded;
    private bool _isShooting;
    private PlayerInput _playerInput;
    private PhotonView _photonView;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _focuser = GetComponent<AimFocuser>();
        if (_photonView.IsMine) PlayerSpawner.yourPlayer = this;
        else Destroy(_focuser);
    }

    void Update()
    {
        if (!_photonView.IsMine) return;

        _isGrounded = _characterController.isGrounded;
        if (_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        Vector2 moveInput = _playerInput.actions["Move"].ReadValue<Vector2>();
        Vector2 lookInput = _playerInput.actions["Look"].ReadValue<Vector2>();
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 look = new Vector3(lookInput.x, 0, lookInput.y);

        if (_lastLook.magnitude > 0.5f)
        {
            LookAtDirection(_lastLook, _rotationSpeed);
        }
        //_trajectoryDrawer.DrawTrajectory(look, 20f);

        if (_playerInput.actions["Shoot"].triggered)
        {
            if (_lastLook.magnitude > 0)
            {
                Shoot(_lastLook.normalized);
            }
            else if (_focuser.HasTarget())
            {
                Shoot(_focuser.GetTargetPosition() - transform.position);
            }
            else Shoot(transform.forward);
        }

        _characterController.Move(move * Time.deltaTime * _playerSpeed);

        if (move.magnitude > 0.1 && !_isShooting && look.magnitude < 0.1f)
        {
            LookAtDirection(move, _rotationSpeed);
        }

        var normalisedVelocity = Quaternion.Inverse(transform.rotation) * (_characterController.velocity / _playerSpeed);

        _animator.SetFloat("SpeedY", normalisedVelocity.z);
        _animator.SetFloat("SpeedX", normalisedVelocity.x);

        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);

        _lastLook = look;
    }

    private bool LookAtDirection(Vector3 target, float speed)
    {
        var quatTarget = Quaternion.LookRotation(target, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, quatTarget, Time.deltaTime * speed);
        if (Quaternion.Angle(transform.rotation, quatTarget) < 2.5f) return true;
        else return false;
    }

    private void Shoot(Vector3 shootDirection)
    {
        StartCoroutine(ShootCoroutine(shootDirection));
    }

    private IEnumerator ShootCoroutine(Vector3 shootDirection)
    {
        _isShooting = true;

        while (!LookAtDirection(shootDirection.normalized, _rotationSpeed * 4))
        {
            yield return null;
        }

        _gun.Shoot();
        yield return new WaitWhile(() => _gun.IsShooting);
        _isShooting = false;
    }

    public bool IsAlive()
    {
        return _health > 0;
    }
}
