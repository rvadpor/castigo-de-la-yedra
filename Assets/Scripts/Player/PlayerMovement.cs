using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController;
    public static bool puedeMoverse = true;
    public float speed = 10f;
    public float gravity = -9.81f;

    public Transform groundCheck;
    public float sphereRadius = 0.3f;
    public LayerMask groundMask;
    bool isGrounded;
    public float jumpHeight = 3;

    Vector3 velocity;

    public bool isSprinting;
    public float sprintingSpeedMultiplier = 1.5f;
    private float sprintSpeed = 1f;

    public float staminaUseAmount = 5;

    //private StaminaBar staminaSlider;

    private void Start()
    {
        //staminaSlider = FindObjectOfType<StaminaBar>();
    }

    void Update()
    {
        if (!puedeMoverse)
            return;
        isGrounded = Physics.CheckSphere(groundCheck.position, sphereRadius, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        JumpCheck();

        RunCheck();

        characterController.Move(move * speed * Time.deltaTime * sprintSpeed);

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }

    public void JumpCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }

    public void RunCheck()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;

            //staminaSlider.UseStamina(staminaUseAmount);

        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
            //staminaSlider.UseStamina(0);

        }

        if (isSprinting == true)
        {
            sprintSpeed = sprintingSpeedMultiplier;


        }
        else
        {
            sprintSpeed = 1;


        }
    }
}
