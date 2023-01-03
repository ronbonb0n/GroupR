using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public string[] itemList = { "CLOAK", "DECOY", "EMP"};
    // public static int[] Inventory = new int[3]; // remember to change if more items are added
    public static int[] Inventory = { 10,10,10}; // remember to change if more items are added

    //public static int[] SalvageList = new int[3];
    public static int Salvage = 10;
    //public bool debug;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this.gameObject); }

        if (PlayerPrefs.HasKey("CLOAK")) Inventory[0] = PlayerPrefs.GetInt("CLOAK");
        if (PlayerPrefs.HasKey("DECOY")) Inventory[1] = PlayerPrefs.GetInt("DECOY");
        if (PlayerPrefs.HasKey("EMP")) Inventory[2] = PlayerPrefs.GetInt("EMP");
        if (PlayerPrefs.HasKey("SALVAGE")) Salvage = PlayerPrefs.GetInt("SALVAGE");
        
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
            if (Inventory[index] - 1 >= 0)
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
    //public int getItemSalvageCount(string item)
    //{
    //    int index = System.Array.IndexOf(itemList, item);
    //    if (index < 0) { return -1; }
    //    return (int)SalvageList[index];
    //}

    //public void setItemSalvageCount(string item, int count)
    //{
    //    int index = System.Array.IndexOf(itemList, item);
    //    if (index >= 0)
    //    {
    //        SalvageList[index] = count;
    //    }
    //    else
    //    {
    //        throw new System.Exception("No such Item in Inventory");
    //    }
    //}
    //public void itemSalvageIncrement(string item)
    //{
    //    int index = System.Array.IndexOf(itemList, item);
    //    if (index >= 0)
    //    {
    //        SalvageList[index] += 1;
    //    }
    //    else
    //    {
    //        throw new System.Exception("No such Item in Inventory");
    //    }
    //}
    public int getSalvage()
    {
        return Salvage;
    }
    public void setSalvage(int count)
    {
        Salvage = count;
    }
    public void SalvageIncrement(int increment)
    {
        Salvage += increment;
    }
    private void OnDestroy()
    {
        PlayerPrefs.SetInt("CLOAK", Inventory[0]);
        PlayerPrefs.SetInt("DECOY", Inventory[1]);
        PlayerPrefs.SetInt("EMP", Inventory[2]);
        PlayerPrefs.SetInt("SALVAGE", Salvage);
    }
}
