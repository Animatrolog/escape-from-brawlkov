using UnityEngine;
using Photon.Realtime;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _damage = 15f;
    public Player Owner { get; private set; }

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

    public void InitializeBullet(Photon.Realtime.Player owner, Vector3 originalDirection, float lag)
    {
        Owner = owner;

        RaycastHit hit;
        transform.forward = originalDirection;
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = originalDirection * _speed;

        if (Physics.Raycast(transform.position, originalDirection, out hit))/// костыль от пролетания пуль через стены
        {
            if (hit.distance < 0.7f)
            {
                rigidbody.position += rigidbody.velocity * lag * 0.15f;
            }
            else rigidbody.position += rigidbody.velocity * lag;

            Debug.DrawRay(transform.position, originalDirection * hit.distance);
        }
        else rigidbody.position += rigidbody.velocity * lag;

    }
}
