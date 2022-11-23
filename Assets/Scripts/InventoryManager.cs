using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public string[] itemList = { "CLOAK", "DECOY", "EMP"};
    public int[] Inventory = new int[3]; // remember to change if more items are added
    public int[] Salvage = new int[3];
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            for (int i = 0; i < itemList.Length; i++)
            {
                instance.Inventory[i] = 0;
                instance.Salvage[i] = 0;
            }
        }
        else { Destroy(this.gameObject); }
        
    }
    public int getItemCount(string item)
    {
        int index = System.Array.IndexOf(itemList, item);
        if (index < 0) { return -1; }
        return (int)instance.Inventory[index];
    }

    public void setItemCount(string item,int count)
    {
        int index = System.Array.IndexOf(itemList, item);
        if (index >= 0) { 
            instance.Inventory[index] = count;
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
            if (instance.Inventory[index] - 1 > 0)
            {
                instance.Inventory[index] -= 1;
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
        return (int)instance.Salvage[index];
    }

    public void setItemSalvageCount(string item, int count)
    {
        int index = System.Array.IndexOf(itemList, item);
        if (index >= 0)
        {
            instance.Salvage[index] = count;
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
            instance.Salvage[index] += 1;
        }
        else
        {
            throw new System.Exception("No such Item in Inventory");
        }
    }


}
