using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
     public float moveSpeed = 5f;
    public float sprintMultiplier = 2f;
    public float jetpackForce = 10f;
    public float maxJetpackFuel = 5f;
    public float jetpackFuelConsumptionRate = 1f;
    public float jetpackFuelRechargeRate = 0.5f;
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;
    public float maxLookAngle = 85f;

    private float currentJetpackFuel;
    private bool isJetpackActive = false;
    private Rigidbody rb;
    private Vector3 moveInput;
    private float rotationY = 0f;
    private float rotationX = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentJetpackFuel = maxJetpackFuel;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Mouvement
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;

        // Sprint
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            currentSpeed *= sprintMultiplier;

        // Jetpack
        if (Input.GetKey(KeyCode.Space) && currentJetpackFuel > 0f)
        {
            isJetpackActive = true;
            currentJetpackFuel -= jetpackFuelConsumptionRate * Time.deltaTime;
        }
        else
        {
            isJetpackActive = false;
            if (currentJetpackFuel < maxJetpackFuel)
                currentJetpackFuel += jetpackFuelRechargeRate * Time.deltaTime;
        }

        // Rotation horizontale
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        rotationY += mouseX;
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

        // Rotation verticale (caméra)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // Appliquer vitesse
        Vector3 move = transform.TransformDirection(moveInput) * currentSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }

    void FixedUpdate()
    {
        if (isJetpackActive)
            rb.AddForce(Vector3.up * jetpackForce, ForceMode.Acceleration);
    }
}