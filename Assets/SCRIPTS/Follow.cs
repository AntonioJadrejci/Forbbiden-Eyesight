using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        // Postavi poziciju kamere
        transform.position = target.position;

        // Postavi kameru da uvijek gleda prema naprijed
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 cameraRotation = transform.rotation.eulerAngles;
        cameraRotation.x -= mouseY * 100.0f * Time.deltaTime;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -90f, 90f);
        transform.rotation = Quaternion.Euler(cameraRotation);
    }
}
