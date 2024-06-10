using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Inventory : MonoBehaviour
{
    // itemi nai nventoryu
    public List<Item> itemList = new List<Item>();

    // kolicina
    public List<int> quantityList = new List<int>();


    //  inventoryPanel je roditeljska komponeneta
    public GameObject inventoryPanel;

    // The
  List<InventorySlot> slotList = new List<InventorySlot>();


    #region Singleton

    public static Inventory instance;

    void Awake()
    {
        instance = this;
    }

    #endregion


    public void Start()
    {
        // dodaj slotove iz panela na listu

        foreach (InventorySlot child in inventoryPanel.GetComponentsInChildren<InventorySlot>())
        {
            slotList.Add(child);
        }



    }
    // poziva se s druge skripte na add item button
    public void AddItem(Item itemAdded, int quantityAdded)
    {
        //If the Item is Stackable it checks if there is already that item in the inventory and only adds the quantity
  
        if (itemAdded.Stackable)
        {
            if (itemList.Contains(itemAdded))
            {
                quantityList[itemList.IndexOf(itemAdded)] = quantityList[itemList.IndexOf(itemAdded)] + quantityAdded;
            }
            else
            {

                if (itemList.Count < slotList.Count)
                {
                    itemList.Add(itemAdded);
                    quantityList.Add(quantityAdded);
                }
                else { }
               
            }

        }
        else
        {
            for (int i = 0; i < quantityAdded; i++)
            {
                if (itemList.Count < slotList.Count)
                {
                    itemList.Add(itemAdded);
                    quantityList.Add(1);
                }
                else {  }
               
            }
            
        }
        
        // Update Inventory kad se doda item
        UpdateInventoryUI();
    }

    
    public void RemoveItem(Item itemRemoved, int quantityRemoved)
    {
        // miče quantity if stackable, else <= 0 miče iz liste
        if (itemRemoved.Stackable)
        {
            if (itemList.Contains(itemRemoved))
            {
                quantityList[itemList.IndexOf(itemRemoved)] = quantityList[itemList.IndexOf(itemRemoved)] - quantityRemoved;

                if (quantityList[itemList.IndexOf(itemRemoved)]<= 0)
                {
                    quantityList.RemoveAt(itemList.IndexOf(itemRemoved));
                    itemList.RemoveAt(itemList.IndexOf(itemRemoved));
                }
            }
            
        }
        else
        {

            
            for (int i = 0; i < quantityRemoved; i++)
            {
                quantityList.RemoveAt(itemList.IndexOf(itemRemoved));
                itemList.RemoveAt(itemList.IndexOf(itemRemoved));
              
            }
        }
        // Update Inventory svaki put kad se makne item
        UpdateInventoryUI();
    }





    public void UpdateInventoryUI()
    {
        // računa koliko je slotova 
        int ind = 0;

      foreach(InventorySlot slot in slotList)
        {

            if (itemList.Count != 0)
            {

                if (ind < itemList.Count)
                {
                    
                    slot.UpdateSlot(itemList[ind], quantityList[ind]);
                    ind = ind + 1;
                }
                else
                {
                    //Update Empty Slot
                    slot.UpdateSlot(null, 0);
                }
            }
            else
            {
                //Update Empty Slot
                slot.UpdateSlot(null, 0);
            }

        }
    }

   
}
