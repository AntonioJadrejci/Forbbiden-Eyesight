using UnityEngine;

public class Letter : MonoBehaviour
{
    private bool playerInRange;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void Interact()
    {
        Debug.Log("Player interacted with the letter.");
        // Dodaj ostale funkcije koje želiš kad igrač interagira s pismom
    }
}
