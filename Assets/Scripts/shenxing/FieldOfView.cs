using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius = 5f;
    [Range(0, 360)]
    public float viewAngle = 45f;
    
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();
    
    public float meshResolution = 1f; // 分辨率，越小越精细
    public int edgeResolveIterations = 4; // 边界解析迭代次数
    public float edgeDstThreshold = 0.5f; // 边界距离阈值
    
    public float maskCutawayDst = 0.15f; // 遮罩切割距离
    
    public MeshFilter viewMeshFilter; // 用于显示视野的网格
    private Mesh viewMesh;
    
    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        
        // 设置 MeshRenderer 的排序层
        MeshRenderer mr = viewMeshFilter.GetComponent<MeshRenderer>();
        mr.sortingLayerName = "Default"; // 确保在前面
        mr.sortingOrder = 0;
        // 用 2D Shader
        // mr.material = new Material(Shader.Find("Sprites/Default"))
        // {
        //     color = new Color(1, 1, 1, 0.2f) // 半透明
        // };
        
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
        visibleTargets.Clear(); // 清空之前的可见目标列表
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
        
        foreach (Collider2D target in targetsInViewRadius)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            if (Vector2.Angle(transform.right, dirToTarget) < viewAngle / 2) // 这里的判断有问题，要用局部的
            {
                // Debug.Log("Target found: " + target.name);
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                if (!Physics2D.Raycast(transform.position, dirToTarget, distanceToTarget, obstacleMask))
                {
                    visibleTargets.Add(target.transform);
                }
            }
        }

        // 处理可见目标
        foreach (Transform visibleTarget in visibleTargets)
        {
            // Debug.Log("Visible Target: " + visibleTarget.name);
        }
    }
    
    private void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle / meshResolution);
        float stepAngle = viewAngle / stepCount;
        List <Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCastInfo = new ViewCastInfo();
        
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.z - viewAngle / 2 + i * stepAngle;
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
        
        int vertexCount = viewPoints.Count + 1; // 包括中心点
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount-2) * 3];
        
        vertices[0] = Vector3.zero;
        for (int i = 0; i < viewPoints.Count; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i] + 
                (viewPoints[i] - transform.position).normalized * maskCutawayDst); // 添加遮罩切割距离
            if (i < viewPoints.Count - 1)
            {
                triangles[i * 3] = 0; // 中心点
                triangles[i * 3 + 1] = i + 1; // 当前点
                triangles[i * 3 + 2] = i + 2; // 下一个点
            }
        }
        
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals(); // 重新计算法线
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
            angleInDegrees += transform.eulerAngles.z; // 将局部角度转换为全局角度
        }
        
        float rad = angleInDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f); // 使用于2D的 XY 平面
    }
    
    public struct ViewCastInfo
    {
        public bool hit; // 是否命中障碍物
        public Vector3 point; // 视野边界点
        public float distance; // 距离
        public float angle; // 角度

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
        public Vector3 pointA; // 边界点A
        public Vector3 pointB; // 边界点B

        public EdgeInfo(Vector3 pointA, Vector3 pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
        }
    }
}
