using UnityEngine;

public class push_object : MonoBehaviour
{
    public AudioClip pushSound;
    public float objectMass = 500f;
    public float pushAtMass = 100f;
    public float pushingTime = 3f;
    public float forceToPush = 300f;

    private AudioSource audioSource;
    private Animator lucasAnimator;
    public GameObject Lucas;

    private bool isPushing = false;
    private bool playerNearby = false;
    private Rigidbody rb;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        if (Lucas != null)
        {
            lucasAnimator = Lucas.GetComponentInChildren<Animator>();
            if (lucasAnimator == null)
            {
                Debug.LogError("Animator component not found on the Lucas GameObject or its children.");
            }
        }
        else
        {
            Debug.LogError("Lucas GameObject is not assigned in the Inspector.");
        }

        // Set the box to be kinematic initially
        rb.isKinematic = true;
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            isPushing = true;
            if (lucasAnimator != null)
            {
                lucasAnimator.SetBool("Push", true);
            }
            rb.isKinematic = false;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            isPushing = false;
            if (lucasAnimator != null)
            {
                lucasAnimator.SetBool("Push", false);
                lucasAnimator.SetFloat("Speed", 0f); // Set to idle when E is released
                lucasAnimator.SetBool("Walk", false); // Ensure Walk is false
            }
            rb.isKinematic = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == Lucas)
        {
            playerNearby = true;
            if (isPushing && lucasAnimator != null && lucasAnimator.GetBool("Push")) // Check if the push animation is active
            {
                Vector3 pushDirection = Lucas.transform.forward;

                rb.AddForce(pushDirection * forceToPush * Time.deltaTime, ForceMode.VelocityChange);

                if (pushSound != null && audioSource != null && !audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(pushSound);
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == Lucas)
        {
            playerNearby = false;
            if (lucasAnimator != null)
            {
                lucasAnimator.SetBool("Push", false);
                lucasAnimator.SetFloat("Speed", 0f); // Set to idle when collision ends
                lucasAnimator.SetBool("Walk", false); // Ensure Walk is false
            }
        }
    }
}
