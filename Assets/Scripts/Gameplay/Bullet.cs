using UnityEngine;
using Photon.Realtime;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _damage = 15f;
    
    public Player Owner { get; private set; }
    private PlayerChar _ownerChar;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void Start()
    {
        Destroy(gameObject, 3.0f);
    }

    public void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable target))
        {
            target.TakeDamage(_damage);
        }
        Destroy(gameObject);

    }

    public void SetOwner(PlayerChar owner)
    {
        _ownerChar = owner;
    }

    public void InitializeBullet(Photon.Realtime.Player owner, Vector3 originalDirection, float lag)
    {
        Vector3 CharVelocity = PlayerChar.LocalPlayerInstance.Velocity;

        Physics.IgnoreCollision(_ownerChar.GetComponent<Collider>(), _collider);
        Owner = owner;
        
        transform.forward = originalDirection;
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = (CharVelocity / _speed) + (originalDirection * _speed);
        rigidbody.position += (CharVelocity / _speed) + (originalDirection * lag);

    }
}
