using UnityEngine;
using Photon.Realtime;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    public Photon.Realtime.Player Owner { get; private set; }

    public void Start()
    {
        Destroy(gameObject, 3.0f);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    public void InitializeBullet(Photon.Realtime.Player owner, Vector3 originalDirection, float lag)
    {
        Owner = owner;

        transform.forward = originalDirection;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = originalDirection * _speed;
        rigidbody.position += rigidbody.velocity * lag;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<IDamageable>(out IDamageable target))
        {
            target.TakeDamage(25f);
            Destroy(gameObject);
        }
        if (collider.CompareTag("SolidObject"))
            Destroy(gameObject);
    }
}
