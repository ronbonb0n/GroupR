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
    public int totalCost;
    public float timeSinceTalked;
//Display Vars
    public Transform canvas;
    public TextMeshProUGUI talkText;
    public TextMeshProUGUI salvageText;
    public TextMeshProUGUI totalCostText;
    public TextMeshProUGUI[] pricesText;
    public TextMeshProUGUI[] shoppingListText;
    public TextMeshProUGUI[] costText;
    public TextMeshProUGUI[] inventoryText;
    // Talking Vars 
    public string[] dialogues = {"Hi there! Wow are you another player? Haven’t seen one of you in a while, I’ve seen a few come and go in the past some came back a few times, others I only got to see once.",
                                 "Don’t worry I won’t hurt you, not sure why but I want to help you. One of others said he had to clear the dungeons to get out. Why would he want to get out?",
                                 "Anyway, if you come back make sure to bring me some bits and pieces – I’m good at recycling and can make you some useful tools to play with!"};
    public int dialogueNumber = 0;
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
        //for (int i = 0; i < InventoryManager.Salvage.Length; i++)
        //{
        //    pricesText[i].text = InventoryManager.Salvage[i].ToString();
        //}
        salvageText.text = InventoryManager.Salvage.ToString();
    }
    public void dispTotalCost()
    {
        totalCostText.text = totalCost.ToString();
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
        dispTotalCost();
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
        dispTotalCost();
        dispShoppingList();
    }
    // Calculate Cost : price * quantity
    public void calcCost(string item) {
        int index = System.Array.IndexOf(InventoryManager.instance.itemList, item);
        itemCosts[index] = itemPrices[index] * shoppingList[index];
        calcTotalCost();
    }
    // Calculate TotalCost : sum of itemCosts
    public void calcTotalCost()
    {
        totalCost = 0;
        for (int i = 0; i < itemCosts.Length; i++)
        {
            totalCost += itemCosts[i];
        }
    }

    // Buy
    public void buy() {
        // Check buy
        //bool checkBuy = true;
        //for(int i = 0; i < itemCosts.Length; i++)
        //{
        //    if (InventoryManager.Salvage[i] < itemCosts[i]) { checkBuy = false; }
        //}
        bool checkBuy = totalCost <= InventoryManager.Salvage;

        if (!checkBuy)
        {
            Debug.Log("You don't have enough salvage to craft that");
            talk("You don't have enough salvage to craft that");
            // "You don't have enough salvage to craft that" message
            return;
        }
        for (int i = 0; i < itemCosts.Length; i++)
        {
            InventoryManager.Inventory[i] += shoppingList[i];
            //InventoryManager.Salvage[i] -= itemCosts[i];
        }
        InventoryManager.Salvage -= totalCost;
        //Reset shopping
        for (int i = 0; i < itemCosts.Length; i++)
        {
            shoppingList[i] = 0;
            itemCosts[i] = 0;
        }
        totalCost = 0;

        //Display Reset
        dispCosts();
        dispInventory();
        dispShoppingList();
        dispSalvage();
        dispTotalCost();
        dispPrices();

        // Save
        PlayerPrefs.SetInt("CLOAK", InventoryManager.Inventory[0]);
        PlayerPrefs.SetInt("DECOY", InventoryManager.Inventory[1]);
        PlayerPrefs.SetInt("EMP", InventoryManager.Inventory[2]);
        PlayerPrefs.SetInt("SALVAGE", InventoryManager.Salvage);
    }


    //Talk
    public void talk(string text)
    {
        talkText.text = text;
        timeSinceTalked = Time.timeSinceLevelLoad;
    }
    //
    public void talkBtn()
    {
        talk(dialogues[dialogueNumber]);
        dialogueNumber = (int) (dialogueNumber + 1) % 3;
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
            itemPrices[i] = i+1;
            itemCosts[i] = 0;
        }
        for(int i=0; i < InventoryManager.instance.itemList.Length; i++) {
            Transform parentTransform = canvas.Find(InventoryManager.instance.itemList[i]);
            pricesText[i] = parentTransform.Find("Price").GetComponent<TextMeshProUGUI>();
            shoppingListText[i] = parentTransform.Find("ShoppingList").GetComponent<TextMeshProUGUI>();
            costText[i] = parentTransform.Find("Cost").GetComponent<TextMeshProUGUI>();
            inventoryText[i] = parentTransform.Find("Inventory").GetComponent<TextMeshProUGUI>();
        }
        talkText = canvas.Find("Base").Find("TalkText").GetComponent<TextMeshProUGUI>();
        salvageText = canvas.Find("SalvageTxt").Find("Salvage").GetComponent<TextMeshProUGUI>();
        totalCostText = canvas.Find("TotalCostTxt").Find("TotalCost").GetComponent<TextMeshProUGUI>();
        dialogueNumber = 0;
    }

    public void Start()
    {
        dispCosts();
        dispInventory();
        dispSalvage();
        dispPrices();
        dispTotalCost();
        dispShoppingList();
        talk("Hi!, How may I help you?");
    }
    private void Update()
    {
        if (Time.timeSinceLevelLoad-timeSinceTalked > 10)
        {
            talkText.text = "How may I help you?"; // replace this by story text strings
        }
    }
}
