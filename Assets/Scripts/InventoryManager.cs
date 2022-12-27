using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public string[] itemList = { "CLOAK", "DECOY", "EMP"};
    public static int[] Inventory = new int[3]; // remember to change if more items are added
    public static int[] Salvage = new int[3];
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this.gameObject); }
        
    }
    
    public int getItemCount(string item)
    {
        int index = System.Array.IndexOf(itemList, item);
        if (index < 0) { return -1; }
        return (int)Inventory[index];
    }

    public void setItemCount(string item,int count)
    {
        int index = System.Array.IndexOf(itemList, item);
        if (index >= 0) { 
            Inventory[index] = count;
        }
        else
        {
            throw new System.Exception("No such Item in Inventory");
        }
    }

    public bool itemDecrement(string item)
    {
        int index = System.Array.IndexOf(itemList, item);
        if (index >= 0)
        {
            if (Inventory[index] - 1 > 0)
            {
                Inventory[index] -= 1;
                return true;
            }
            else { return false; }

        }
        else
        {
            throw new System.Exception("No such Item in Inventory");
        }
    }
    public int getItemSalvageCount(string item)
    {
        int index = System.Array.IndexOf(itemList, item);
        if (index < 0) { return -1; }
        return (int)Salvage[index];
    }

    public void setItemSalvageCount(string item, int count)
    {
        int index = System.Array.IndexOf(itemList, item);
        if (index >= 0)
        {
            Salvage[index] = count;
        }
        else
        {
            throw new System.Exception("No such Item in Inventory");
        }
    }
    public void itemSalvageIncrement(string item)
    {
        int index = System.Array.IndexOf(itemList, item);
        if (index >= 0)
        {
            Salvage[index] += 1;
        }
        else
        {
            throw new System.Exception("No such Item in Inventory");
        }
    }


}
