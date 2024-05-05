using UnityEngine;

public class LucasInteraction : MonoBehaviour
{
    private bool isNearObject = false;
    private GameObject nearbyObject;

    void Update()
    {
        if (isNearObject && Input.GetKeyDown(KeyCode.F))
        {
            // Ovdje pokrenite interakciju
            // Na primjer, prikažite bilješku na ekranu
            Debug.Log("Interacting with " + nearbyObject.name);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        isNearObject = true;
        nearbyObject = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        isNearObject = false;
        nearbyObject = null;
    }
}