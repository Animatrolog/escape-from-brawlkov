using UnityEngine;
using System.Collections.Generic;

public class AimFocuser : MonoBehaviour
{
    [SerializeField] private GameObject _targetPointerPrefab;
    private ObjectFollower _targetPointer;
    private IShootable _target;
    private List<IShootable> _targets = new List<IShootable>();

    private void Start()
    {
        _targetPointer = Instantiate(_targetPointerPrefab).GetComponent<ObjectFollower>();
    }
 
    private void Update()
    {
        Vector3 ofsetPosition = new Vector3(0f, 0.4f, 0f);
        float minDistance = float.MaxValue;
        List<IShootable> validTargets = new List<IShootable>();

        foreach (IShootable target in _targets)
        {
            var targetMB = target as MonoBehaviour;
            Vector3 direction = ((targetMB.transform.position + ofsetPosition) - (transform.position + ofsetPosition)).normalized;
            Ray aimRay = new Ray(transform.position + ofsetPosition, direction);
            RaycastHit hit;

            Debug.DrawRay(transform.position + ofsetPosition, direction * 15f);

            if (Physics.Raycast(aimRay, out hit))
            {
                if (hit.collider.TryGetComponent<IShootable>(out IShootable visibleTarget))
                {
                    if (hit.collider.GetComponent<IShootable>() == target)
                    {
                        validTargets.Add(target);
                    }
                }
                else
                {
                    validTargets.Remove(target);
                }
            }

        }

        if(validTargets.Count < 1)
        {
            _targetPointer.gameObject.SetActive(false);
            _target = null;
            return;
        }

        foreach (IShootable target in validTargets)
        {
            var targetMB = target as MonoBehaviour;
            Vector3 directionToTarget = ((targetMB.transform.position + ofsetPosition) - (transform.position + ofsetPosition));
            float distanceFromTarget = directionToTarget.sqrMagnitude;

            if (minDistance > distanceFromTarget)
            {
                minDistance = distanceFromTarget;
                _target = target;
            }
        }

        if (_target != null)
        {
            MonoBehaviour targetMB = _target as MonoBehaviour;

            if (targetMB.isActiveAndEnabled)
            {

                _targetPointer.SetObjectToFollow(targetMB.gameObject);
                _targetPointer.gameObject.SetActive(true);
            }
        }
        else
        {
            _targetPointer.gameObject.SetActive(false);
            _targets.Remove(_target);
            _target = null;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<IShootable>(out IShootable target))
        {
            _targets.Add(target);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent<IShootable>(out IShootable target))
        {
            _targets.Remove(target);
           // _targetPointer.gameObject.SetActive(false);
        }
    }

    public bool HasTarget()
    {
        return _target != null;
    }

    public Vector3 GetTargetPosition()
    {
        if (_target != null)
        {
            return (_target as MonoBehaviour).transform.position;
        }
        else return transform.position;
    }
}
