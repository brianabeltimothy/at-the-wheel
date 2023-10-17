using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    float horizontalSpeed;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float brakeForce;
    private float currentSpeed;
    private bool isStationary = true;

    private float nitroTimer;
    private float nitroCooldown;
    private bool nitroIsActive = false;
    private bool nitroIsCoolingDown = true;

    private bool isOnGround = true;
    private float jumpForce = 9000.0f;
    private float gravityScale = 5.0f;
    private Rigidbody rb;

    [SerializeField]
    private float rotationSpeed = 90.0f;

    void Awake()
    {
        maxSpeed = 30.0f;
        horizontalSpeed = 40.0f;
        acceleration = 15.0f;
        brakeForce = 10.0f;
        currentSpeed = 0.0f;

        nitroTimer = 3.0f;
        nitroCooldown = 4.0f;

        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //move the player
        moveForwardBackward();

        //turn vehicle
        isStationary = (currentSpeed == 0);
        if (!isStationary)
        {
            turnVehicle();
        }

        //jump
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            jump();
            Debug.Log("is jumping");
        }

        //nitro
        if (Input.GetKeyDown(KeyCode.N) && !nitroIsActive && !nitroIsCoolingDown)
        {
            nitroIsActive = true;
            activateNitro();
        }

        if (nitroIsActive)
        {
            //start nitro timer
            nitroTimer -= Time.deltaTime;

            if (nitroTimer <= 0)
            {
                deactivateNitro();
                nitroIsCoolingDown = true;
            }

            Debug.Log("nitro timer:" + nitroTimer);
        }

        if (!nitroIsActive && nitroIsCoolingDown)
        {
            // start cooldown
            nitroCooldown -= Time.deltaTime;
            if (nitroCooldown <= 0)
            {
                nitroCooldown = 4.0f;
                nitroIsCoolingDown = false;
            }
            //Debug.Log("Nitro cooldown: " + nitroCooldown);
        }
    }

    public void jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
        isOnGround = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            Debug.Log(isOnGround);
        }
    }

    public void turnVehicle()
    {
        float h = horizontalSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;

        if (currentSpeed < 0)
        {
            h *= -1.0f;
        }

        Quaternion targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + h, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void moveForwardBackward()
    {
        float input = Input.GetAxis("Vertical");
        float speedIncrement = acceleration * Time.deltaTime;
        float speedDecrement = brakeForce * Time.deltaTime;

        // Update the current speed gradually.
        if (input > 0 && currentSpeed <= maxSpeed)
        {
            currentSpeed += speedIncrement;
        }
        else if (input < 0 && currentSpeed >= -maxSpeed * 0.5f)
        {   
            currentSpeed -= speedDecrement;
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0.0f, speedIncrement);
        }

        transform.Translate(0.0f, 0.0f, currentSpeed * Time.deltaTime);
    }

    void activateNitro()
    {
        acceleration += 40.0f;
        maxSpeed += 20.0f;

        Debug.Log("Nitro is active");
    }

    void deactivateNitro()
    {
        acceleration -= 40.0f;
        maxSpeed -= 20.0f;
        nitroIsActive = false;
        nitroTimer = 3.0f;

        Debug.Log("Nitro is deactivated");
    }
}
