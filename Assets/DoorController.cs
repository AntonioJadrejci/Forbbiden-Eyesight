using UnityEngine;

public class DoorController : MonoBehaviour
{
    public string doorName = "Door_2";
    public float openAngle = 90f;
    public float closedAngle = 0f;
    public float smooth = 2f;
    private bool isOpen = false;
    private Quaternion targetRotation;

    void Start()
    {
        targetRotation = Quaternion.Euler(0, closedAngle, 0);
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smooth);
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        targetRotation = isOpen ? Quaternion.Euler(0, openAngle, 0) : Quaternion.Euler(0, closedAngle, 0);
    }

    void OnMouseDown()
    {
        if (gameObject.name == doorName)
        {
            ToggleDoor();
        }
    }
}
