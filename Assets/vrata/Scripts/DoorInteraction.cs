using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public GameObject canvas;

    void Start()
    {
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        
        if (canvas != null)
        {
            bool isActive = canvas.activeSelf;
            canvas.SetActive(!isActive);
        }
    }
}
