using UnityEngine;


public class MovingPlatform : MonoBehaviour
{
    public Transform platform;
    public Transform StartPoint;
    public Transform EndPoint;

    int direction = 1;
    [SerializeField] float speed = 1.5f;

    private void FixedUpdate()
    {
        if (platform == null || StartPoint == null || EndPoint == null)
            return;

        Vector2 target = currentMovementTarget();

        platform.transform.position = Vector2.MoveTowards(platform.position, target, speed * Time.deltaTime);

        float dist = Vector2.Distance(target, platform.position);
        if (dist <= 0.1f)
        {
            direction *= -1;
        }
    }


    Vector2 currentMovementTarget()
    {
        if (direction == 1)
        {
            return StartPoint.position;
        }
        else
        {
            return EndPoint.position;
        }
    }

    private void OnDrawGizmos()
    {
        if (platform != null && StartPoint != null && EndPoint != null)
        {
            Gizmos.DrawLine(platform.transform.position, StartPoint.position);
            Gizmos.DrawLine(platform.transform.position, EndPoint.position);
        }
    }
}
