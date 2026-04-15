using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float thrustSpeed = 15f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private float mouseSensitivity = 2f;

    [Header("Visual Tilt (Fluidity)")]
    [SerializeField] private Transform visualModel;
    [SerializeField] private float maxTiltAngle = 20f;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private float cameraRollAngle = 10f;

    [Header("Systems")]
    [SerializeField] private GameObject fpvCam;
    [SerializeField] private GameObject tpvCam;
    [SerializeField] private Slider healthBar;
    public AudioClip deathSound;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private float verticalInput, xRotation, currentHealth;
    private float maxHealth = 100f;
    private float currentCameraRoll = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        if (healthBar) { healthBar.maxValue = maxHealth; healthBar.value = currentHealth; }
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        // Recuperamos los ejes para el movimiento
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveDirection = (transform.forward * v + transform.right * h).normalized;

        verticalInput = Input.GetKey(KeyCode.Space) ? 1 : (Input.GetKey(KeyCode.LeftControl) ? -1 : 0);

        // Rotación suave de cámara y cuerpo
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity);
        xRotation = Mathf.Clamp(xRotation - Input.GetAxis("Mouse Y") * mouseSensitivity, -90f, 90f);

        // --- AQUÍ VUELVE LA FLUIDEZ VISUAL ---
        if (visualModel != null)
        {
            float targetPitch = v * maxTiltAngle;
            float targetRoll = -h * maxTiltAngle;
            Quaternion targetRot = Quaternion.Euler(targetPitch, 0f, targetRoll);
            visualModel.localRotation = Quaternion.Lerp(visualModel.localRotation, targetRot, Time.deltaTime * tiltSpeed);
        }

        // Suavizado del Roll de la cámara (el efecto de lado al doblar)
        float targetCameraRoll = -h * cameraRollAngle;
        currentCameraRoll = Mathf.Lerp(currentCameraRoll, targetCameraRoll, Time.deltaTime * tiltSpeed);
        fpvCam.transform.localRotation = Quaternion.Euler(xRotation, 0, currentCameraRoll);
        tpvCam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        if (Input.GetKeyDown(KeyCode.C)) ToggleCameras();
    }

    void FixedUpdate()
    {
        rb.AddForce(moveDirection * thrustSpeed + transform.up * (verticalInput * verticalSpeed), ForceMode.Acceleration);
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        if (healthBar) healthBar.value = currentHealth;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        if (deathSound) AudioSource.PlayClipAtPoint(deathSound, transform.position);
        GameManager.instance?.GameOver();
    }

    private void ToggleCameras()
    {
        bool fpvActive = fpvCam.activeSelf;
        fpvCam.SetActive(!fpvActive);
        tpvCam.SetActive(fpvActive);
    }
}