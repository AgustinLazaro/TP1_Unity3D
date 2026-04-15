using UnityEngine;

public class GrenadeLauncher : MonoBehaviour
{
    public GameObject grenadePrefab;
    public Transform firePoint;
    public LineRenderer lineRenderer;

    [Header("Physics Settings")]
    public float initialVelocity = 15f;
    public float launchAngle = 15f;
    public int resolution = 30;
    public float flightTime = 3f;
    public float explosionRadius = 5f;
    public bool showTrajectory = true;

    void Update()
    {
        Vector3[] trajectoryPoints = CalculateTrajectory();

        if (showTrajectory)
        {
            lineRenderer.positionCount = resolution;
            lineRenderer.SetPositions(trajectoryPoints);
        }
        else lineRenderer.positionCount = 0;

        if (Input.GetMouseButtonDown(1)) Launch(trajectoryPoints);
    }

    Vector3[] CalculateTrajectory()
    {
        Vector3[] points = new Vector3[resolution];
        Vector3 initialDirection = Quaternion.AngleAxis(-launchAngle, transform.right) * transform.forward;
        Vector3 velocityVector = initialDirection * initialVelocity;

        for (int i = 0; i < resolution; i++)
        {
            float t = i * (flightTime / (resolution - 1));
            Vector3 point = firePoint.position + (velocityVector * t);
            point.y += 0.5f * Physics.gravity.y * (t * t);
            points[i] = point;
        }
        return points;
    }

    void Launch(Vector3[] path)
    {
        GameObject grenade = Instantiate(grenadePrefab, firePoint.position, Quaternion.identity);
        grenade.GetComponent<Grenade>().SetPath(path);
    }

    void OnDrawGizmos()
    {
        if (firePoint == null) return;
        Vector3[] points = CalculateTrajectory();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(points[points.Length - 1], explosionRadius);
    }
}