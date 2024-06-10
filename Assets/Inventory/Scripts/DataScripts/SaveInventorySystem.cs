using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



[System.Serializable]
public class InventoryDataStr
{
    public List<Item> itemList;
    public List<int> intList;
}


public class SaveInventorySystem : MonoBehaviour
{

    //spremi trenutni inventory - mali bug
    public void SaveInventory()
    {

        InventoryDataStr saveData = new InventoryDataStr();
        saveData.itemList = Inventory.instance.itemList;
        saveData.intList = Inventory.instance.quantityList;

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("InventoryData", json);
        PlayerPrefs.Save();
    }

    //load inventory
    public void LoadInventory()
    {
        
        if (PlayerPrefs.HasKey("InventoryData"))
        {
            string json = PlayerPrefs.GetString("InventoryData");
            InventoryDataStr saveData = JsonUtility.FromJson<InventoryDataStr>(json);

            Inventory.instance.itemList = saveData.itemList;
            Inventory.instance.quantityList = saveData.intList;
            Inventory.instance.UpdateInventoryUI();
        }
        else
        {
            
        }
    }

  

}
