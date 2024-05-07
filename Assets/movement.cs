using UnityEngine;

public class LucasMovement : MonoBehaviour
{
    public float rotationSpeed = 100.0f;
    public float moveSpeed = 5.0f;
    public float jumpForce = 2.0f;
    private bool isJumping = false;
    private Rigidbody rb;

    void Start()
    {
        // Sakrij kursor
        Cursor.lockState = CursorLockMode.Locked;

        // Dohvati Rigidbody komponentu
        rb = GetComponent<Rigidbody>();

        // Postavi ograničenja rotacije
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        // Dohvati unose za kretanje
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Kreiraj vektor kretanja
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical).normalized;

        // Primjeni kretanje na Rigidbody
        rb.MovePosition(rb.position + transform.TransformDirection(movement) * moveSpeed * Time.deltaTime);

        // Okretanje lika pomoću miša
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y += mouseX * rotationSpeed * Time.deltaTime;
        rotation.x -= mouseY * rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rotation);

        // Provjeri je li pritisnuta tipka za skakanje
        if (!isJumping && Input.GetButtonDown("Jump"))
        {
            // Resetiraj vertikalnu brzinu
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            // Primjeni silu skakanja
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            isJumping = true;
        }
    }

    // Provjeri je li Lucas na tlu
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}
