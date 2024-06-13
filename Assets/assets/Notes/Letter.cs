using UnityEngine;
using UnityEngine.UI;

public class Letter : MonoBehaviour
{
    public GameObject interactTextUI; // UI element za "Press E to interact"
    public GameObject noteUI; // UI element za prikaz note

    private bool playerInRange;
    private bool isReading;

    private void Start()
    {
        // Sakrij UI elemente na početku
        interactTextUI.SetActive(false);
        noteUI.SetActive(false);
        isReading = false;
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isReading)
        {
            // Sakrij "Press E to interact", prikaži note i zaustavi vrijeme kada igrač pritisne "E"
            interactTextUI.SetActive(false);
            noteUI.SetActive(true);
            isReading = true;
            Time.timeScale = 0; // Zaustavi vrijeme
            Interact();
        }
        else if (isReading && Input.GetKeyDown(KeyCode.Escape))
        {
            // Sakrij note, vrati vrijeme na normalno i postavi isReading na false kada igrač pritisne "Escape"
            noteUI.SetActive(false);
            isReading = false;
            Time.timeScale = 1; // Vrati vrijeme na normalno
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // Prikaži "Press E to interact" kada igrač uđe u zonu
            interactTextUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // Sakrij "Press E to interact" kada igrač izađe iz zone
            interactTextUI.SetActive(false);
        }
    }

    public void Interact()
    {
        Debug.Log("Player interacted with the letter.");
        // Dodaj ostale funkcije koje želiš kad igrač interagira s pismom
    }
}