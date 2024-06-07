using UnityEngine;

public class PlayerLetter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Letter"))
        {
            // Poveži s komponentom Letter
            Letter letter = other.GetComponent<Letter>();
            if (letter != null)
            {
                // Pozovi javnu metodu Interact iz klase Letter
                letter.Interact();
            }
        }
    }
}
