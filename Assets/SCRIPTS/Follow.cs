using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;

    void Start()
    {
        // Izračunaj početni offset
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        // Postavi poziciju kamere
        transform.position = target.position + offset;

        // Postavi kameru da uvijek gleda prema liku
        transform.LookAt(target);
    }
}