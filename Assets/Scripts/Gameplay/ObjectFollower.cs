using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
   [SerializeField] private GameObject _target;
   [SerializeField] private float _followSpeed;
   [SerializeField] private bool _absolute;

    private Vector3 _offset;
   
   void Start()
   {
       if (_target == null) _target = this.gameObject;
        ResetOffset();
   }
   
    private void ResetOffset()
    {
        _offset = transform.position - _target.transform.position;
    }

    public void SetVisibility(bool visibility)
    {
        GetComponent<Renderer>().enabled = visibility;
    }

   public void SetObjectToFollow(GameObject target)
   {
        _target = target;
   }
   
   void LateUpdate()
   {
        if (_target == null) _target = this.gameObject;
        if (_absolute)
        {
            transform.position = _target.transform.position + _offset;
        }
        else transform.position = Vector3.Lerp(transform.position, _target.transform.position + _offset, Time.deltaTime * _followSpeed);
   }
}