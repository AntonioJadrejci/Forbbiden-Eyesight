using UnityEngine;

public class DoorController : MonoBehaviour
{
    public string doorName = "Door_2";
    public float openAngle = 180f;
    public Vector3 closedRotation = new Vector3(-94.388f, -12.919f, -267.925f);
    public float smooth = 5f;
    private bool isOpen = false;
    private Quaternion targetRotation;

    void Start()
    {
        targetRotation = Quaternion.Euler(closedRotation);
        transform.rotation = targetRotation; // osiguraè da su vrata u poèetnoj pozicji
        Debug.Log("Initial rotation set to closed position: " + closedRotation);
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smooth);
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            // 
            targetRotation = Quaternion.Euler(closedRotation + new Vector3(0, openAngle, 0));
            Debug.Log("Opening door to rotation: " + (closedRotation + new Vector3(0, openAngle, 0)));
        }
        else
        {
            // reset na zatvorena
            targetRotation = Quaternion.Euler(closedRotation);
            Debug.Log("Closing door to rotation: " + closedRotation);
        }
    }

    void OnMouseDown()
    {
        if (gameObject.name == doorName)
        {
            Debug.Log("Door clicked: " + doorName);
            ToggleDoor();
        }
        else
        {
            Debug.Log("Clicked object is not the door: " + gameObject.name);
        }
    }
}
