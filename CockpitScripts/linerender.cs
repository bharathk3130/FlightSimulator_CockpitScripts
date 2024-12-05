using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Transform point0, point1, point2, point3; // Control points
    public LineRenderer lineRenderer;
    public int curveResolution = 20;  // Number of points on the curve

    void Start()
    {
        lineRenderer.positionCount = curveResolution;
        DrawBezierCurve();
    }
    
    void Update()
    {
        DrawBezierCurve();
    }

    void DrawBezierCurve()
    {
        for (int i = 0; i < curveResolution; i++)
        {
            float t = i / (float)(curveResolution - 1);
            Vector3 pointOnCurve = CalculateBezierPoint(t, point0.position, point1.position, point2.position, point3.position);
            lineRenderer.SetPosition(i, pointOnCurve);
        }
    }

    // Calculate point on Bezier curve
    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0; // (1-t)^3 * P0
        p += 3 * uu * t * p1; // 3(1-t)^2 * t * P1
        p += 3 * u * tt * p2; // 3(1-t) * t^2 * P2
        p += ttt * p3;        // t^3 * P3

        return p;
    }
}

