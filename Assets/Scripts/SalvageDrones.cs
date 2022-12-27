using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalvageDrones : MonoBehaviour
{
    public string salvageType = "CLOAK";
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InventoryManager.instance.itemSalvageIncrement(salvageType); //change according to which salvage
            Destroy(gameObject);
        }
        for (int i = 0; i < 3; i++)
        {
            Debug.Log(i + " " + InventoryManager.Salvage[i]);
        }
    }
}
