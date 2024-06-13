using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 100f;
    private bool isRotating = false;
    public RectTransform objectToRotate; // Referenca na UI element koji sadrži sprite

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Provjera je li lijeva tipka miša pritisnuta
        {
            isRotating = true;
        }
        if (Input.GetMouseButtonUp(0)) // Provjera je li lijeva tipka miša otpuštena
        {
            isRotating = false;
        }

        if (isRotating && objectToRotate != null)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            objectToRotate.Rotate(Vector3.forward, -mouseX); // Koristimo Vector3.forward za 2D rotaciju
        }
    }
}
