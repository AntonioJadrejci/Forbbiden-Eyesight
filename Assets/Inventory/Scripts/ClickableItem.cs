using UnityEngine;

public class ClickableItem : MonoBehaviour
{
    public Item itemToAdd;
    public int quantityToAdd = 1;

    void OnMouseDown()
    {
        // Find the AddItemToInventory component in the scene
        AddItemToInventory addItemToInventory = FindObjectOfType<AddItemToInventory>();
        if (addItemToInventory != null)
        {
            // Set the item and quantity in AddItemToInventory
            addItemToInventory.specificItem = itemToAdd;
            addItemToInventory.specificQuant = quantityToAdd;
            // Add the item to the inventory
            addItemToInventory.AddSpecificItem();
            // Optionally destroy the game object after adding to the inventory
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("AddItemToInventory script not found in the scene!");
        }
    }
}
