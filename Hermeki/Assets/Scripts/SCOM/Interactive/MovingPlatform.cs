using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MovingPlatform : MonoBehaviour
{
    /// <summary>
    /// 이동할 Platform
    /// </summary>
    public Transform platform;
    /// <summary>
    /// 플랫폼이 이동할 경로의 포인트, direction에 따라 이동
    /// </summary>
    public List<Transform> Points = new List<Transform>();
    /// <summary>
    /// 현재 목적지 포인트 인덱스
    /// </summary>
    private int currPointIdx;
    /// <summary>
    /// 다음 목적지 포인트 방향
    /// </summary>
    int direction = 1;
    [SerializeField]
    /// <summary>
    /// true : currPointIdx가 Points.Count에 다다른다면 Points[0].position으로 이동
    /// </summary>
    bool isRepeat;
    /// <summary>
    /// 진행 속도
    /// </summary>
    [SerializeField] float speed = 1.5f;

    private void Awake()
    {
        currPointIdx = 0;
        platform.position = currentMovementTarget();
    }

    private void FixedUpdate()
    {
        if (platform == null || Points?.Count == 0) 
            return;

        //목적지 위치 계산
        Vector2 target = currentMovementTarget();

        platform.position = Vector2.MoveTowards(platform.position, target, speed * Time.deltaTime);

        //다음 목적지와의 거리
        float dist = Vector2.Distance(target, platform.position);

        //다음 목적지에 도달했을 때
        if (dist <= 0.1f)
        {
            currPointIdx += direction;

            //Points.Count에 도달했을 때
            if (isRepeat && currPointIdx >= Points.Count)
            {
                currPointIdx = 0;
            }
            else if (!isRepeat && currPointIdx >= Points.Count)
            {
                direction *= -1;
                currPointIdx = Points.Count - 2;
            }
            //Points[0]에 도달했을 때
            else if (isRepeat && currPointIdx < 0)
            {
                currPointIdx = Points.Count - 1;
            }
            else if (!isRepeat && currPointIdx < 0)
            {
                direction *= -1;
                currPointIdx = 1;
            }
        }
    }

    private void OnDisable()
    {
        currPointIdx = 0;
        platform.position = currentMovementTarget();
    }

    /// <summary>
    /// 현재 목적지 position
    /// </summary>
    /// <returns></returns>
    Vector2 currentMovementTarget() => Points[currPointIdx].position;
    private void OnDrawGizmos()
    {
        if (platform != null && Points != null)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                if (i + 1 < Points.Count)
                {
                    Gizmos.DrawLine(Points[i].position, Points[i + 1].position);
                }
                else
                {
                    Gizmos.DrawLine(Points[i].position, Points[0].position);
                }
            }
        }
    }
}
