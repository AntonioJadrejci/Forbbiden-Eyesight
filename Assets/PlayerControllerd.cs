using UnityEngine;

public class PlayerControllerd : MonoBehaviour
{
    public float walkSpeed = 3.0f;
    public float sprintSpeed = 6.0f;
    public float crouchSpeed = 1.5f;
    public float jumpForce = 10.0f;

    private float lastJumpTime = 0;
    private float jumpCooldown = 0.5f;
    public float rotationSpeed = 10.0f;

    [SerializeField]
    GameObject big_lucas;


    private Rigidbody rb;
    private Animator animator;

    private Vector3 movement = Vector3.zero;
    private bool crouched;
    private bool jump;

    void Start()
    {
        rb = big_lucas.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        jump = false;
        crouched = false;
    }

    void Update()
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

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        
        if (movement.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
            
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

}







    
 