using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VendorFunctions : MonoBehaviour
{
// Calc Vars
    public int[] shoppingList;
    public int[] itemPrices;
    public int[] itemCosts;
    //Display Vars
    public Transform canvas;
    public TextMeshProUGUI talkText;
    public TextMeshProUGUI[] pricesText;
    public TextMeshProUGUI[] shoppingListText;
    public TextMeshProUGUI[] costText;
    public TextMeshProUGUI[] inventoryText;

// Display Functions
    // Display price of items
    public void dispPrices() { 
        for(int i = 0; i< pricesText.Length; i++)
        {
            pricesText[i].text = itemPrices[i].ToString();
        }
    }

    // Display Quantity to buy
    public void dispShoppingList()
    {
        for (int i = 0; i < shoppingListText.Length; i++)
        {
            shoppingListText[i].text = shoppingList[i].ToString();
        }
    }
    // Display cost of quantity 
    public void dispCosts()
    {
        for (int i = 0; i < costText.Length; i++)
        {
            costText[i].text = itemCosts[i].ToString();
        }
    }
    // Display Inventory : get from InventoryManager
    public void dispInventory()
    {
        for (int i = 0; i < InventoryManager.Inventory.Length; i++)
        {
            inventoryText[i].text = InventoryManager.Inventory[i].ToString();
        }
    }
    public void dispSalvage()
    {
        for (int i = 0; i < InventoryManager.Salvage.Length; i++)
        {
            pricesText[i].text = InventoryManager.Salvage[i].ToString();
        }
    }
// Calculation Functions
    // Increment Buy
    public void increasePurchase(string item) {
        // Increase item to shopping list
        int index = System.Array.IndexOf(InventoryManager.instance.itemList, item);
        shoppingList[index]++;
        // Update costs
        calcCost(item);
        // Update display
        dispCosts();
        dispShoppingList();
    }
    // Decrement Buy
    public void decreasePurchase(string item) {
        // Decrease item from shopping list
        int index = System.Array.IndexOf(InventoryManager.instance.itemList, item);
        if (shoppingList[index] > 0) shoppingList[index]--;
        // Update Cost
        calcCost(item);
        // Update Display
        dispCosts();
        dispShoppingList();
    }
    // Calculate Cost : price * quantity
    public void calcCost(string item) {
        int index = System.Array.IndexOf(InventoryManager.instance.itemList, item);
        itemCosts[index] = itemPrices[index] * shoppingList[index];
    }
    // Buy
    public void buy() {
        // Check buy
        bool checkBuy = true;
        for(int i = 0; i < itemCosts.Length; i++)
        {
            if (InventoryManager.Salvage[i] < itemCosts[i]) { checkBuy = false; }
        }
        if (!checkBuy)
        {
            for (int i = 0; i < 3; i++)
            {
                Debug.Log(i + " " + InventoryManager.Salvage[i]);
            }
            Debug.Log("You don't have enough salvage to craft that");
            talkText.text = "You don't have enough salvage to craft that";
            // "You don't have enough salvage to craft that" message
            return;
        }
        for (int i = 0; i < itemCosts.Length; i++)
        {
            InventoryManager.Inventory[i] += shoppingList[i];
            InventoryManager.Salvage[i] -= itemCosts[i];
        }
        //Reset shopping
        for (int i = 0; i < itemCosts.Length; i++)
        {
            shoppingList[i] = 0;
            itemCosts[i] = 0;
        }
        //Display Reset
        dispCosts();
        dispInventory();
        dispShoppingList();
        dispSalvage();
    }

    //Talk
    public void talk()
    {
        talkText.text = "Hi, how may I help you?";
    }

    //Quit
    public void QuitToWorldMap()
    {
        GameManager.SwitchLevel(LEVELS.WORLD_MAP);
    }


    public void Awake()
    {
        shoppingList = new int[InventoryManager.instance.itemList.Length];
        itemPrices = new int[InventoryManager.instance.itemList.Length];
        itemCosts = new int[InventoryManager.instance.itemList.Length];
        pricesText = new TextMeshProUGUI[InventoryManager.instance.itemList.Length];
        shoppingListText = new TextMeshProUGUI[InventoryManager.instance.itemList.Length];
        inventoryText = new TextMeshProUGUI[InventoryManager.instance.itemList.Length];
        costText = new TextMeshProUGUI[InventoryManager.instance.itemList.Length];
        for (int i = 0; i < shoppingList.Length; i++)
        {
            shoppingList[i] = 0;
            itemPrices[i] = 1;
            itemCosts[i] = 0;
        }
        for(int i=0; i < InventoryManager.instance.itemList.Length; i++) {
            Transform parentTransform = canvas.Find(InventoryManager.instance.itemList[i]);
            pricesText[i] = parentTransform.Find("Salvage").GetComponent<TextMeshProUGUI>();
            shoppingListText[i] = parentTransform.Find("ShoppingList").GetComponent<TextMeshProUGUI>();
            costText[i] = parentTransform.Find("Cost").GetComponent<TextMeshProUGUI>();
            inventoryText[i] = parentTransform.Find("Inventory").GetComponent<TextMeshProUGUI>();
        }
        talkText = canvas.Find("Base").Find("TalkText").GetComponent<TextMeshProUGUI>();
    }
    public void Start()
    {
        dispCosts();
        dispInventory();
        dispSalvage();
        dispShoppingList();
        talk();
    }
}
