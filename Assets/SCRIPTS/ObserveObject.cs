using UnityEngine;
using UnityEngine.UI;

public class ObserveObject : MonoBehaviour
{
    public GameObject interactTextUI; // UI element za "Press O to observe"
    public GameObject sawUI; // UI element za prikaz slike i teksta

    private bool playerInRange;
    private bool isObserving;

    private void Start()
    {
        // Sakrij UI elemente na početku
        interactTextUI.SetActive(false);
        sawUI.SetActive(false);
        isObserving = false;
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.O) && !isObserving)
        {
            // Sakrij "Press O to observe", prikaži sawUI i zaustavi vrijeme kada igrač pritisne "O"
            interactTextUI.SetActive(false);
            sawUI.SetActive(true);
            isObserving = true;
            Time.timeScale = 0; // Zaustavi vrijeme
            Interact();
        }
        else if (isObserving && Input.GetKeyDown(KeyCode.Escape))
        {
            // Sakrij sawUI, vrati vrijeme na normalno i postavi isObserving na false kada igrač pritisne "Escape"
            sawUI.SetActive(false);
            isObserving = false;
            Time.timeScale = 1; // Vrati vrijeme na normalno
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // Prikaži "Press O to observe" kada igrač uđe u zonu
            interactTextUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // Sakrij "Press O to observe" kada igrač izađe iz zone
            interactTextUI.SetActive(false);
        }
    }

    public void Interact()
    {
        Debug.Log("Player is observing the saw.");
        // Dodaj ostale funkcije koje želiš kad igrač interagira s objektom
    }
}
