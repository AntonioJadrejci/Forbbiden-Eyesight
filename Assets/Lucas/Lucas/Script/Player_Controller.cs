using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 3.0f;
    public float sprintSpeed = 6.0f;
    public float crouchSpeed = 1.5f;
    public float jumpForce = 10.0f;
    private float lastJumpTime = 0;
    private float jumpCooldown = 0.5f;
    public float rotationSpeed = 10.0f;
    [SerializeField]
    GameObject Lucas;
    private Rigidbody rb;
    private Animator animator;
    private Vector3 movement = Vector3.zero;
    private bool crouched;
    private bool jump;
    private bool isPushing;
    private bool climbingLadder; // New variable for climbing

    void Start()
    {
        rb = Lucas.GetComponent<Rigidbody>();
        animator = Lucas.GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the Lucas GameObject or its children.");
        }
        jump = false;
        crouched = false;
        isPushing = false;
        climbingLadder = false; // Initialize climbingLadder
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (Input.GetKeyDown(KeyCode.E))
        {
            isPushing = true;
            animator.SetBool("Push", true);
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            isPushing = false;
            animator.SetBool("Push", false);
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Walk", false);
        }

        if (!isPushing && !climbingLadder)
        {
            if (movement.magnitude != 0.0f)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    animator.SetFloat("Speed", movement.magnitude);
                    animator.SetBool("Walk", false);
                }
                else
                {
                    animator.SetFloat("Speed", 0.0f);
                    animator.SetBool("Walk", true);
                }
            }
            else
            {
                animator.SetFloat("Speed", 0.0f);
                animator.SetBool("Walk", false);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                crouched = !crouched;
                animator.SetBool("Crouch", crouched);
            }

            if (crouched)
            {
                animator.SetFloat("Crouched_Walking", movement.magnitude * crouchSpeed);
            }
            else
            {
                animator.SetFloat("Crouched_Walking", 0.0f);
            }

            bool isGrounded = IsGrounded();
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !crouched && (Time.time - lastJumpTime > jumpCooldown))
            {
                Vector3 jumpDirection = Vector3.up * jumpForce;
                if (movement.magnitude > 0)
                {
                    jumpDirection += transform.forward * 0.5f;
                }
                rb.AddForce(jumpDirection, ForceMode.Impulse);
                animator.SetBool("Jump", true);
                jump = true;
                lastJumpTime = Time.time;
            }
            else if (!isGrounded)
            {
                animator.SetBool("Jump", false);
                jump = false;
            }
        }

        if (climbingLadder)
        {
            if (Input.GetKey(KeyCode.W))
            {
                float climbSpeed = 2.0f;
                movement = new Vector3(0, climbSpeed, 0);
                rb.velocity = movement;
                animator.SetBool("Climbing_Ladder", true); // Trigger climbing animation
            }
            else
            {
                rb.velocity = Vector3.zero;
                animator.SetBool("Climbing_Ladder", false); // Stop climbing animation if not pressing W
            }
        }
        else
        {
            animator.SetBool("Climbing_Ladder", false);
        }
    }

    private void FixedUpdate()
    {
        if (climbingLadder)
        {
            rb.useGravity = false;
            rb.velocity = movement;
        }
        else
        {
            rb.useGravity = true;

            if (!isPushing && movement.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
            }
        }

        if (jump)
            rb.useGravity = false;
        else
            rb.useGravity = true;
    }

    private bool IsGrounded()
    {
        float distanceToGround = 0.5f;
        Vector3 offset = new Vector3(0, 0.1f, 0);
        Vector3 start = transform.position + offset;
        Vector3 direction = Vector3.down;
        Debug.DrawLine(start, start + direction * distanceToGround, Color.red);
        return Physics.Raycast(start, direction, distanceToGround);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            climbingLadder = true;
            rb.velocity = Vector3.zero; // Stop any previous movement
            rb.isKinematic = true; // Set Rigidbody to kinematic
            Debug.Log("Entered Ladder Trigger");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            climbingLadder = false;
            rb.isKinematic = false; // Reset Rigidbody to non-kinematic
            Debug.Log("Exited Ladder Trigger");
        }
    }
}
