using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform doorTransform;
    public Vector3 openPositionOffset;  // Offset za otvorenu poziciju
    public Vector3 closePositionOffset; // Offset za zatvorenu poziciju
    private Vector3 initialPosition;    // Početna pozicija vrata
    public float openSpeed = 2f;        // Brzina otvaranja/zatvaranja vrata

    private bool isMoving = false;

    void Start()
    {
        if (doorTransform == null)
        {
            doorTransform = transform; // Koristi transform objekta ako nije postavljen
        }

        initialPosition = doorTransform.localPosition;

        // Debug poruke za provjeru početnih pozicija
        Debug.Log("Initial Position: " + initialPosition);
        Debug.Log("Open Position Offset: " + openPositionOffset);
        Debug.Log("Close Position Offset: " + closePositionOffset);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isMoving)
        {
            StopAllCoroutines();
            StartCoroutine(MoveDoor(initialPosition + openPositionOffset));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isMoving)
        {
            StopAllCoroutines();
            StartCoroutine(MoveDoor(initialPosition + closePositionOffset));
        }
    }

    IEnumerator MoveDoor(Vector3 targetPosition)
    {
        isMoving = true;

        // Debug: Print target positions
        Debug.Log("Moving to Position: " + targetPosition);

        while (Vector3.Distance(doorTransform.localPosition, targetPosition) > 0.01f)
        {
            doorTransform.localPosition = Vector3.Lerp(doorTransform.localPosition, targetPosition, Time.deltaTime * openSpeed);
            yield return null;
        }
        doorTransform.localPosition = targetPosition;
        isMoving = false;
    }
}
