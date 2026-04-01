using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    [Header("Configuración de Velocidad")]
    [SerializeField] private float thrustSpeed = 15f;
    [SerializeField] private float verticalSpeed = 10f;

    [Header("Inclinación Visual")]
    [SerializeField] private Transform modeloVisual;
    [SerializeField] private float inclinacionMaxima = 20f;
    [SerializeField] private float velocidadInclinacion = 5f;
    [SerializeField] private float inclinacionCamara1ra = 10f;

    [Header("Sistema de Cámaras")]
    [SerializeField] private GameObject camara1raPersona;
    [SerializeField] private GameObject camara3raPersona;

    [Header("Rotación")]
    [SerializeField] private float mouseSensitivity = 2f;

    [Header("Sistema de Vida y UI")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float dmgMultiplier = 2f;
    [SerializeField] private TextMeshProUGUI textoVida;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private float verticalInput;
    private float inputX;
    private float inputZ;
    private float xRotation = 0f;
    private float currentHealth;
    private float targetCameraRoll = 0f;
    private float currentCameraRoll = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentHealth = maxHealth;
        ActualizarUI();
    }

    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputZ = Input.GetAxisRaw("Vertical");

        moveDirection = (transform.forward * inputZ) + (transform.right * inputX);
        moveDirection.Normalize();

        verticalInput = 0f;
        if (Input.GetKey(KeyCode.Space)) verticalInput = 1f;
        else if (Input.GetKey(KeyCode.LeftControl)) verticalInput = -1f;

        if (modeloVisual != null)
        {
            float targetPitch = inputZ * inclinacionMaxima;
            float targetRoll = -inputX * inclinacionMaxima;
            Quaternion rotacionDeseada = Quaternion.Euler(targetPitch, 0f, targetRoll);
            modeloVisual.localRotation = Quaternion.Lerp(modeloVisual.localRotation, rotacionDeseada, Time.deltaTime * velocidadInclinacion);
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        targetCameraRoll = -inputX * inclinacionCamara1ra;
        currentCameraRoll = Mathf.Lerp(currentCameraRoll, targetCameraRoll, Time.deltaTime * velocidadInclinacion);

        camara1raPersona.transform.localRotation = Quaternion.Euler(xRotation, 0f, currentCameraRoll);
        camara3raPersona.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        if (Input.GetKeyDown(KeyCode.C)) ToggleCameras();
    }

    private void FixedUpdate()
    {
        rb.AddForce(moveDirection * thrustSpeed, ForceMode.Acceleration);
        rb.AddForce(transform.up * (verticalInput * verticalSpeed), ForceMode.Acceleration);
    }

    private void ToggleCameras()
    {
        bool isFirstPersonActive = camara1raPersona.activeSelf;
        camara1raPersona.SetActive(!isFirstPersonActive);
        camara3raPersona.SetActive(isFirstPersonActive);
    }

    private void OnCollisionEnter(Collision collision)
    {
        float velocidadChoque = collision.relativeVelocity.magnitude;
        if (velocidadChoque > 2f)
        {
            RecibirDaño(Mathf.RoundToInt(velocidadChoque * dmgMultiplier));
        }
    }

    public void RecibirDaño(float daño)
    {
        currentHealth -= daño;
        if (currentHealth < 0) currentHealth = 0;
        ActualizarUI();
        if (currentHealth == 0) Muerte();
    }

    private void ActualizarUI()
    {
        if (textoVida != null) textoVida.text = "Vida: " + currentHealth.ToString();
    }

    private void Muerte()
    {
        transform.position = new Vector3(0, 10, 0);
        rb.linearVelocity = Vector3.zero;
        currentHealth = maxHealth;
        ActualizarUI();
    }
}