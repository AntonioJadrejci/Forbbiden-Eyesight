using UnityEngine;

public class ladder : MonoBehaviour
{
    void Start()
    {
        // Ensure the ladder has a trigger collider
        Collider[] colliders = GetComponentsInChildren<Collider>();
        bool hasTrigger = false;
        foreach (Collider collider in colliders)
        {
            if (collider.isTrigger)
            {
                hasTrigger = true;
                break;
            }
        }

        if (!hasTrigger)
        {
            Debug.LogError("Ladder GameObject must have a Collider component set as a trigger.");
        }
    }

    void Update()
    {
        // Additional functionality if needed
    }
}
