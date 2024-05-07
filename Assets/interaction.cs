using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float interactionDistance;
    public GameObject interactionText;
    public LayerMask interactionLayers;

    private GameObject currentInteractable; // Objekt s kojim trenutno možemo interagirati
    private LucasMovement lucasMovement; // Referenca na skriptu LucasMovement

    void Start()
    {
        // Dohvati komponentu LucasMovement iz istog objekta
        lucasMovement = GetComponent<LucasMovement>();
    }

    void Update()
    {
        RaycastHit hit;

        //If the raycast hits something,
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, interactionLayers))
        {
            //If the object it hit contains the letter script,
            if (hit.collider.gameObject.GetComponent<letter>())
            {
                //The interaction text will enable
                interactionText.SetActive(true);
                currentInteractable = hit.collider.gameObject;

                //If the E key is pressed,
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //The letter component is accessed and the letter will open or close
                    hit.collider.gameObject.GetComponent<letter>().openCloseLetter();
                    // Hide interaction text after interaction
                    interactionText.SetActive(false);
                    // Disable player movement
                    if(lucasMovement != null)
                        lucasMovement.enabled = false;
                }
            }
            //else, the interaction text is set false.
            else
            {
                interactionText.SetActive(false);
                currentInteractable = null;
            }
        }
        //else, the interaction text is set false.
        else
        {
            interactionText.SetActive(false);
            currentInteractable = null;
        }

        // Hide interaction text if the player is looking at the interactable object
        if (currentInteractable != null && currentInteractable == hit.collider.gameObject)
        {
            interactionText.SetActive(false);
        }
    }
}
