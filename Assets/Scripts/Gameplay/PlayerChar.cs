using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerChar : MonoBehaviourPunCallbacks, IShootable , IDamageable , IPunObservable
{
    [SerializeField] private float _playerSpeed = 2.0f;
    [SerializeField] private float _gravityValue = -9.81f;
    [SerializeField] float _rotationSpeed = 10f;
    [SerializeField] Animator _animator;
    [SerializeField] private Gun _gun;
    [SerializeField] private GameObject _playerFollowerPrefab;
    [SerializeField] private GameObject _cameraPrefab;
    [SerializeField] private GameObject _playerUiPrefab;
    //[SerializeField] TrajectoryDrawer _trajectoryDrawer;

    private AimFocuser _focuser;
    private PlayerUI _playerUI;
    private CharacterController _characterController;
    public Vector3 PlayerVelocity;
    private Vector3 _lastLook;
    private bool _isGrounded;
    private bool _isShooting;
    private PlayerInput _playerInput;
    private GameObject _follower;
    private UIhider _uiHider;

    public static PlayerChar LocalPlayerInstance;
    public float Health;
    public Vector3 Velocity;

    public void Awake()
    {
        
        if (photonView.IsMine)
        {
            //this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            _follower = Instantiate(this._playerFollowerPrefab);
            _follower.transform.SetParent(this.transform ,false);

            Instantiate(this._cameraPrefab).TryGetComponent<ObjectFollower>(out ObjectFollower cameraFollower);
            cameraFollower.SetObjectToFollow(this.gameObject);

            LocalPlayerInstance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _focuser = GetComponent<AimFocuser>();
        _uiHider = GetComponent<UIhider>();
        _gun.OwnerChar = this;

        if (!photonView.IsMine)
        {
            _uiHider.enabled = false;
            _focuser.enabled = false;
        }

        if (this._playerUiPrefab != null)
        {
            Instantiate(this._playerUiPrefab).TryGetComponent<PlayerUI>(out PlayerUI playerUI);
            _playerUI = playerUI;
            _playerUI.SetTarget(this);
            _playerUI.UpdateHealthData();
        }
        else
        {
            //Debug.LogWarning("<Color=Red><b>Missing</b></Color> PlayerUiPrefab reference on player Prefab.", this);
        }
    }


    void Update()
    {
        if (photonView.IsMine)
        {
            this.ProcessInputs();
        }
    }

    private void ProcessInputs()
    {
        _isGrounded = _characterController.isGrounded;
        if (_isGrounded && PlayerVelocity.y < 0)
        {
            PlayerVelocity.y = 0f;
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

        if (move.magnitude > 0.1 && look.magnitude < 0.1f && !_isShooting) 
        {
            LookAtDirection(move, _rotationSpeed);
        }

        var normalisedVelocity = Quaternion.Inverse(transform.rotation) * (_characterController.velocity / _playerSpeed);

        Velocity = _characterController.velocity;

        _animator.SetFloat("SpeedY", normalisedVelocity.z);
        _animator.SetFloat("SpeedX", normalisedVelocity.x);

        PlayerVelocity.y += _gravityValue * Time.deltaTime;
        _characterController.Move(PlayerVelocity * Time.deltaTime);

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
        if(!_isShooting)StartCoroutine(ShootCoroutine(shootDirection));
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
        return Health > 0;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.Health);
        }
        else
        {
            this.Health = (float)stream.ReceiveNext();
        }
        if (this.Health <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.LeaveRoom();
        }
        if(_playerUI != null)_playerUI.UpdateHealthData();
    }
    public void HideUI(bool hide)
    {
        if(!photonView.IsMine)_playerUI.gameObject.SetActive(hide ? false : true);
        //Debug.Log("HideUI => " + hide);
    }
}
