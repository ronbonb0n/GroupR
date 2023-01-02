using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalvageDrones : MonoBehaviour
{
    public int salvageAmount = 1;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InventoryManager.instance.SalvageIncrement(salvageAmount); //change according to which salvage
            Destroy(gameObject);
        }
    }
}
