using UnityEngine;

public class TrajectoryDrawer : MonoBehaviour
{
    public void DrawTrajectory(Vector3 direction, float distance)
    {
        if (direction.magnitude > 0.1f)
        {
            transform.forward = direction;
            Ray aimRay = new Ray(transform.position, direction.normalized);
            RaycastHit hit;
            float trajectoryLength = distance;

            if (Physics.Raycast(aimRay, out hit, distance))
            {
                if (hit.collider.TryGetComponent<IDamageable>(out IDamageable target))
                {
                    trajectoryLength = Vector3.Distance(transform.position, hit.point);
                }
            }
            transform.localScale = new Vector3(0.5f, 1f, 0.5f * trajectoryLength);
        }
        else transform.localScale = Vector3.zero;
    }
}
