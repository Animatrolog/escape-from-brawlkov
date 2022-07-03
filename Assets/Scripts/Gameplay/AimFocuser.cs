using UnityEngine;
using System.Collections.Generic;

public class AimFocuser : MonoBehaviour
{
    [SerializeField] private ObjectFollower _targetMarker;

    private IShootable _target;
    private List<IShootable> _targets;

    private void Start()
    {
        _targets = new List<IShootable>();
    }
 
    private void Update()
    {
        Vector3 ofsetPosition = new Vector3(0f, 0.4f, 0f);
        float minDistance = 999f;
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
            _targetMarker.SetVisibility(false);
            _target = null;
            return;
        }

        foreach (IShootable target in validTargets)
        {
            var targetMB = target as MonoBehaviour;
            float distanceFromTarget = Vector3.Distance(transform.position + ofsetPosition, targetMB.transform.position + ofsetPosition);
            Vector3 direction = ((targetMB.transform.position + ofsetPosition) - (transform.position + ofsetPosition)).normalized;

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

                _targetMarker.SetObjectToFollow(targetMB.gameObject);
                _targetMarker.SetVisibility(true);
            }
        }
        else
        {
            _targetMarker.SetVisibility(false);
            _targets.Remove(_target);
            _target = null;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        IShootable target = collider.GetComponent<IShootable>();
        if (target != null)
        {
             _targets.Add(target);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent<IShootable>(out IShootable target))
        {
            RemoveFtomTargets(target);
        }
    }

    private void RemoveFtomTargets(IShootable target)
    {
        _targets.Remove(target);
        _targetMarker.SetVisibility(false);
        _target = null;
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
