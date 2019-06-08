using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // total number of inventory spaces
    private const int INVENTORY_NUMBER = 10;

    // total number of gun spaces
    private const int GUN_NUMBER = 2;

    // list of items in inventory
    private List<ItemObjectScript> playerInventory = new List<ItemObjectScript>();

    // list of guns in inventory
    private List<ItemObjectScript> playerGuns = new List<ItemObjectScript>();

    // base UI panel
    private Transform inventoryUI;

    // event signifiying an item has been added to the inventory
    public event EventHandler<InventoryEventArgs> ItemAdded;

    // event signifiying a gun has been added to gun slot
    public event EventHandler<GunEventArgs> GunAdded;

    public void AddItem(ItemObjectScript item)
    {
        // check if there is a slot open
        if(playerInventory.Count < INVENTORY_NUMBER)
        {
            // disable the object and child it to the player

            // add the item to the list
            playerInventory.Add(item);

            if(ItemAdded != null)
            {
                ItemAdded(this, new InventoryEventArgs(item));
            }
        }
        else
        {
            Debug.Log("<color=orange> Warning:</color> Inventory full!");
        }
    }

    public void AddGun(ItemObjectScript gun)
    {
        // always add the gun to inventory
        // if it is 'full', swap the primary with new gun
        // if primary is full, send it to secondary slot
        // if it is empty, assign it to primary
        playerGuns.Add(gun);

        if(GunAdded != null)
        {
            GunAdded(this, new GunEventArgs(gun));
        }
    }

    public int InventorySize()
    {
        return playerInventory.Count;
    }

    public void RemoveItem(int itemIndex)
    {
        // remove the item from the list
        playerInventory.RemoveAt(itemIndex);

        // updating remaining UI element list indexes
        UpdateImageIndex(itemIndex);
    }

    public void UpdateImageIndex(int itemIndexRemoved)
    {
            // loop through the UI elements
            foreach (Transform slot in inventoryUI)
            {
               // checks if a child object exists
                if(slot.childCount > 0)
                {
                    // if it does and it is in an index greater than what was deleted, update the index
                    if (slot.GetChild(0).GetComponent<ItemDragHandler>().uiListIndex > itemIndexRemoved)
                        slot.GetChild(0).GetComponent<ItemDragHandler>().uiListIndex--;
                }
            }
    }

    public void SwapItems(int index1, int index2)
    {
        // swap items in the list
        ItemObjectScript temp = playerInventory[index1];
        playerInventory[index1] = playerInventory[index2];
        playerInventory[index2] = temp;
    }

    public ItemObjectScript GetInventoryIndexInfo(int itemIndex)
    {
        // found an index and returned relevant data
        if(itemIndex >= 0 && itemIndex < INVENTORY_NUMBER && playerInventory[itemIndex])
            return playerInventory[itemIndex];

        // issue, return null
        return null;
    }

    private void Start()
    {
        inventoryUI = GameObject.Find("Canvas/InventoryUI/InventorySlots").transform;
    }
}