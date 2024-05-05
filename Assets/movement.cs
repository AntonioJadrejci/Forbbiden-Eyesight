using UnityEngine;

public class LucasMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 2.0f;
    private bool isJumping = false;
    private Rigidbody rb;

    void Start()
    {
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
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        // Primjeni kretanje na Rigidbody
        rb.velocity = movement * speed;

        // Provjeri je li pritisnuta tipka za skakanje
        if (!isJumping && Input.GetButtonDown("Jump"))
        {
            // Resetiraj vertikalnu brzinu
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            // Primjeni silu skakanja
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange);
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