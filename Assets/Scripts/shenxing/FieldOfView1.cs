using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView1 : MonoBehaviour
{
    public enum Direction { Right, Up, Left, Down }
    public Direction currentDirection = Direction.Right;

    public float viewRadius = 5f;
    [Range(0, 360)]
    public float viewAngle = 45f;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    public float meshResolution = 1f;
    public int edgeResolveIterations = 4;
    public float edgeDstThreshold = 0.5f;

    public float maskCutawayDst = 0.15f;

    public MeshFilter viewMeshFilter;
    private Mesh viewMesh;

    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        MeshRenderer mr = viewMeshFilter.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            mr.sortingLayerName = "Default";
            mr.sortingOrder = 0;

        }
        StartCoroutine(FindTargetsWithDelay(0.2f));
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    private IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        Vector3 facingDir = GetFacingDirection();

        foreach (Collider2D target in targetsInViewRadius)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;

            if (Vector2.Angle(facingDir, dirToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                if (!Physics2D.Raycast(transform.position, dirToTarget, distanceToTarget, obstacleMask))
                {
                    visibleTargets.Add(target.transform);
                }
            }
        }
    }

    private void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle / meshResolution);
        float stepAngle = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCastInfo = new ViewCastInfo();

        float baseAngle = GetBaseAngle();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = baseAngle - viewAngle / 2 + i * stepAngle;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCastInfo.distance - newViewCast.distance) > edgeDstThreshold;
                if (oldViewCastInfo.hit != newViewCast.hit || (oldViewCastInfo.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCastInfo, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }

                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCastInfo = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < viewPoints.Count; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i] +
                (viewPoints[i] - transform.position).normalized * maskCutawayDst);

            if (i < viewPoints.Count - 1)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewRadius, obstacleMask);

        if (hit)
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }

        float rad = angleInDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
    }

    // 获取当前朝向的方向向量
    public Vector3 GetFacingDirection()
    {
        switch (currentDirection)
        {
            case Direction.Up: return Vector3.up;
            case Direction.Down: return Vector3.down;
            case Direction.Left: return Vector3.left;
            default: return Vector3.right; // Right
        }
    }

    // 获取当前方向的基础角度
    private float GetBaseAngle()
    {
        switch (currentDirection)
        {
            case Direction.Up: return 90f;
            case Direction.Down: return 270f;
            case Direction.Left: return 180f;
            default: return 0f; // Right
        }
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool hit, Vector3 point, float distance, float angle)
        {
            this.hit = hit;
            this.point = point;
            this.distance = distance;
            this.angle = angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 pointA, Vector3 pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
        }
    }
}