using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float maxSpeed;
    float horizontalSpeed;
    private float acceleration;
    private float brakeForce;
    public float currentSpeed;
    private bool isStationary = true;

    private float nitroTimer;
    private float nitroCooldown;
    private bool nitroIsActive = false;
    private bool nitroIsCoolingDown = true;

    private bool isOnGround = true;
    private float jumpForce = 9000.0f;
    private float gravityScale = 5.0f;
    private Rigidbody rb;

    private float rotationSpeed = 100.0f;

    public TextMeshProUGUI nitroManager;
    public GameObject tutorialText;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            tutorialText.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            restartGame();
        }

        nitroManager.text = "Nitro Available";
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
            nitroManager.text = "Nitro is being used!";
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
            nitroManager.text = "Nitro is cooling down...";
        }
    }

    public void restartGame()
    {
        SceneManager.LoadScene(0);
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
    }

    void deactivateNitro()
    {
        acceleration -= 40.0f;
        maxSpeed -= 20.0f;
        nitroIsActive = false;
        nitroTimer = 3.0f;
    }
}
