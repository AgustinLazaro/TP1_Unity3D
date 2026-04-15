using UnityEngine;

public class LaserSystem : MonoBehaviour
{
    public AudioSource droneAudio; // Si lo dejás vacío en el Inspector, no pasa nada
    public AudioClip fireSound;
    public Transform fpvCam, tpvCam, muzzlePoint;
    public LineRenderer laserLine;

    [Header("Settings")]
    public float laserDistance = 100f;
    public float fireRate = 0.2f;
    public GameObject bulletPrefab;

    private bool isLaserActive = true;
    private float nextFireTime;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) isLaserActive = !isLaserActive;

        if (isLaserActive) UpdateLaser();
        else laserLine.enabled = false;

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    void UpdateLaser()
    {
        Transform cam = GetActiveCamera();
        laserLine.enabled = true;
        laserLine.SetPosition(0, muzzlePoint.position);

        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, laserDistance))
            laserLine.SetPosition(1, hit.point);
        else
            laserLine.SetPosition(1, cam.position + cam.forward * laserDistance);
    }

    void Fire()
    {
        Transform cam = GetActiveCamera();
        Vector3 targetPoint = Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, laserDistance)
            ? hit.point : cam.position + cam.forward * laserDistance;

        Vector3 fireDirection = (targetPoint - muzzlePoint.position).normalized;
        Instantiate(bulletPrefab, muzzlePoint.position, Quaternion.LookRotation(fireDirection));

     
        if (droneAudio != null && fireSound != null)
        {
            droneAudio.PlayOneShot(fireSound);
        }
    }

    Transform GetActiveCamera() => fpvCam.gameObject.activeInHierarchy ? fpvCam : tpvCam;
}