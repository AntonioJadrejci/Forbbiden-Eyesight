using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform door; // Referenca na vrata koja se trebaju otvoriti/zatvoriti
    public Vector3 openPosition; // Pozicija vrata kada su otvorena
    public Vector3 closedPosition; // Pozicija vrata kada su zatvorena
    public float speed = 2.0f; // Brzina otvaranja/zatvaranja vrata

    private bool isOpen = false; // Da li su vrata otvorena ili zatvorena

    void Update()
    {
        // Interpoliraj između otvorene i zatvorene pozicije na temelju toga jesu li vrata otvorena ili zatvorena
        door.position = Vector3.Lerp(door.position, isOpen ? openPosition : closedPosition, Time.deltaTime * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        // Ako igrač uđe u trigger, otvori vrata
        if (other.CompareTag("Player"))
        {
            isOpen = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Ako igrač izađe iz triggera, zatvori vrata
        if (other.CompareTag("Player"))
        {
            isOpen = false;
        }
    }
}