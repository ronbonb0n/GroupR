using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalvageDrones : MonoBehaviour
{
    public int salvageAmount = 1;
    private void Start()
    {
        gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_EmissionColour", Color.red);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InventoryManager.SalvageIncrement(salvageAmount); //change according to which salvage
            Destroy(gameObject);
        }
    }
}
