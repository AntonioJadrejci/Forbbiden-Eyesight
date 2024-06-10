using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AddItemToInventory : MonoBehaviour
{

    // item koji trrba dodat
    public Item specificItem;

    // količina - edit u inspectoru
    public int specificQuant;

   public void AddSpecificItem()
    {
        Inventory.instance.AddItem(specificItem, specificQuant);
    }

 
}
