using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Движение")]
    [SerializeField] private float walkSpeed = 320f;
    [SerializeField] private float jumpSpeed = 268f;
    [SerializeField] private float gravity = 800f;
    [SerializeField] private float friction = 6f;
    [SerializeField] private float accelerate = 10f;
    [SerializeField] private float airAccelerate = 0.7f;
    [SerializeField] private float mouseSensitivity = 2f;
    
    private CharacterController controller;
    private Camera playerCamera;
    private Vector3 velocity = Vector3.zero;
    private float xRotation = 0f;
    private bool isGrounded;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }
    
    private void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        Vector3 wishDir = transform.right * horizontal + transform.forward * vertical;
        wishDir.y = 0;
        wishDir = wishDir.normalized;
        
        float wishSpeed = walkSpeed;
        
        if (isGrounded)
        {
            ApplyFriction();
            Accelerate(wishDir, wishSpeed, accelerate);
            
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = jumpSpeed;
            }
        }
        else
        {
            AirAccelerate(wishDir, wishSpeed, airAccelerate);
        }
        
        velocity.y -= gravity * Time.deltaTime;
        
        controller.Move(velocity * Time.deltaTime);
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }
    }
    
    private void ApplyFriction()
    {
        Vector3 horizontalVel = new Vector3(velocity.x, 0, velocity.z);
        float speed = horizontalVel.magnitude;
        
        if (speed != 0)
        {
            float drop = speed * friction * Time.deltaTime;
            horizontalVel *= Mathf.Max(speed - drop, 0) / speed;
            velocity.x = horizontalVel.x;
            velocity.z = horizontalVel.z;
        }
    }
    
    private void Accelerate(Vector3 wishDir, float wishSpeed, float accel)
    {
        float currentSpeed = Vector3.Dot(velocity, wishDir);
        float addSpeed = wishSpeed - currentSpeed;
        
        if (addSpeed <= 0) return;
        
        float accelSpeed = accel * Time.deltaTime * wishSpeed;
        if (accelSpeed > addSpeed) accelSpeed = addSpeed;
        
        velocity += accelSpeed * wishDir;
    }
    
    private void AirAccelerate(Vector3 wishDir, float wishSpeed, float accel)
    {
        float wishSpd = wishSpeed;
        if (wishSpd > 30) wishSpd = 30;
        
        float currentSpeed = Vector3.Dot(velocity, wishDir);
        float addSpeed = wishSpd - currentSpeed;
        
        if (addSpeed <= 0) return;
        
        float accelSpeed = accel * wishSpeed * Time.deltaTime;
        if (accelSpeed > addSpeed) accelSpeed = addSpeed;
        
        velocity += accelSpeed * wishDir;
    }
    
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}