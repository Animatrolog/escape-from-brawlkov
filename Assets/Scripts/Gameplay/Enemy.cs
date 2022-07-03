using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable, IShootable
{
    [SerializeField] private float _health;


    private NavMeshAgent _navMeshAgent;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
       // _navMeshAgent.SetDestination(_player.transform.position); 
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if(_health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public bool IsAlive()
    {
        return _health > 0;
    }
}
