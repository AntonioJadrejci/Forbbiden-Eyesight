using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    // ako je null je prazan
    Item item;

    
    public Image itemImage;
    public Text quantity;

    
    public Button removeButton;


    // poziva se kad se doda ili makne item
    public void UpdateSlot(Item itemInSlot, int quantityInSlot)
    {
        item = itemInSlot;


        if (itemInSlot != null && quantityInSlot !=0)
        {
            

            removeButton.enabled = true;
            itemImage.enabled = true; 
            
            itemImage.sprite = itemInSlot.itemIcon;

            if (quantityInSlot > 1)
            {
               
                quantity.enabled = true;
                quantity.text = quantityInSlot.ToString();
            }
            else
            {
                quantity.enabled = false;
                
            }

        }
        else
        {
            
            removeButton.enabled = false;
            itemImage.enabled = false;
            quantity.enabled = false;
        }
    }

    // poziva se ako misom
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        GetComponentInParent<ItemInfoUpdate>().UpdateInfoPanel(item);
    }

    
    public void OnPointerExit(PointerEventData eventData)
    {
        
        GetComponentInParent<ItemInfoUpdate>().ClosePanel();
    }

    
    public void UseItem()
    {
        
        if (item != null)
        {
            
            item.Use();
        }
    }

   
    public void RemoveItem()
    {
        
        Inventory.instance.RemoveItem(Inventory.instance.itemList[Inventory.instance.itemList.IndexOf(item)], 1);
    }
}
