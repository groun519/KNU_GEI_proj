using UnityEngine;

public class SwordHit : MonoBehaviour
{
    [Header("SectorSettings")]
    [SerializeField] private float radius;
    public float angle;
    public LayerMask targetLayer;

    [SerializeField] private Enemy target;
    [SerializeField] private PlayerController player;

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    public void CheckSectorHitBox(float _coe)
    {
        Vector3 position = transform.position;
        Vector3 forward = transform.forward;

        Collider[] hits = Physics.OverlapSphere(position, radius, targetLayer);

        foreach (Collider hit in hits)
        {
            Vector3 targetDir = (hit.transform.position - position).normalized;

            if(Vector3.Angle(forward, targetDir) < angle / 2)
            {
                target = hit.gameObject.GetComponent<Enemy>();
                target.CalculateDamage((12 + player.Rpoint / 300.0f * 488.0f) * _coe);
                Debug.Log("Damage : " + (12 + player.Rpoint / 300.0f * 488.0f) * _coe);
            }
        }
    }

    public void SetRadius(float _rad)
    {
        radius = _rad;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 position = transform.position;
        Vector3 forward = transform.forward;

        // 부채꼴의 양 끝 방향 계산
        Vector3 rightBoundary = Quaternion.Euler(0, angle / 2, 0) * forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -angle / 2, 0) * forward;

        // 부채꼴을 원호로 그리기
        int segments = 20;
        float segmentAngle = angle / segments;
        Vector3 previousPoint = position + leftBoundary * radius;
        for (int i = 1; i <= segments; i++)
        {
            Vector3 nextPoint = position + (Quaternion.Euler(0, segmentAngle * i - angle / 2, 0) * forward) * radius;
            Gizmos.DrawLine(previousPoint, nextPoint);
            previousPoint = nextPoint;
        }

        // 중심에서 부채꼴의 끝점으로 선 그리기
        Gizmos.DrawLine(position, position + leftBoundary * radius);
        Gizmos.DrawLine(position, position + rightBoundary * radius);
    }
}
