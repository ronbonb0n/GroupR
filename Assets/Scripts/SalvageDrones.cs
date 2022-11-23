using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalvageDrones : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Salvage"))
        {
            if (true) // Check which Salvage it is here.
            { 
                InventoryManager.instance.itemSalvageIncrement("CLOAK"); //change according to which salvage
            }
            Destroy(other.gameObject);
        }
    }
}
