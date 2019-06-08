using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryHUD : MonoBehaviour
{
    public Inventory Inventory;

    Transform inventoryPanel;

    Transform gunPanel;


    // Start is called before the first frame update
    void Start()
    {
        Inventory.ItemAdded += InventoryScript_ItemAdded;
        Inventory.GunAdded += InventoryScript_GunAdded;
        inventoryPanel = GameObject.Find("Canvas/InventoryUI/InventorySlots").transform;
        gunPanel = GameObject.Find("Canvas/InventoryUI/GunSlots").transform;
    }

    public void InventoryScript_ItemAdded(object sender, InventoryEventArgs e)
    {
        foreach(Transform slot in inventoryPanel)
        {
            // get the image component of the movable object
            Image image = slot.GetChild(0).GetComponent<Image>();

            // check if the current slot is active
            if (!image.IsActive())
            {
                // gets the number assossiated with the gameObject that corresponds to the index in the list

                ItemDragHandler draggerRef = image.gameObject.GetComponent<ItemDragHandler>();

                // assigns data to the UI image script
                draggerRef.uiListIndex = Inventory.InventorySize()-1;
                draggerRef.itemDescription = e.Item.description;
                draggerRef.itemType = e.Item.itemType;
                image.gameObject.SetActive(true);
                image.sprite = e.Item.itemImage;

                break;
            }
        }
    }

    public void InventoryScript_GunAdded(object sender, GunEventArgs e)
    {
        /*foreach (Transform slot in inventoryPanel)
        {
            // get the image component of the movable object
            Image image = slot.GetChild(0).GetComponent<Image>();

            // check if the current slot is active
            if (!image.IsActive())
            {
                // gets the number assossiated with the gameObject that corresponds to the index in the list

                ItemDragHandler draggerRef = image.gameObject.GetComponent<ItemDragHandler>();

                // assigns data to the UI image script
                draggerRef.uiListIndex = Inventory.InventorySize() - 1;
                draggerRef.itemDescription = e.Item.description;
                draggerRef.itemType = e.Item.itemType;
                image.gameObject.SetActive(true);
                image.sprite = e.Item.itemImage;

                break;
            }
        }*/
        Transform primaryGun = gunPanel.GetChild(0);
        Transform secondaryGun = gunPanel.GetChild(1);

        Image primaryGunImg = primaryGun.GetChild(0).GetComponent<Image>();
        Image secondaryGunImg = secondaryGun.GetChild(0).GetComponent<Image>();

        // both empty // both full
        if(!primaryGunImg.isActiveAndEnabled || (primaryGunImg.isActiveAndEnabled && secondaryGunImg.isActiveAndEnabled) || (!primaryGunImg.isActiveAndEnabled && !secondaryGunImg.isActiveAndEnabled))
        {
            // when you have guns, deteremine if both are full and if they are,
            // drop the current primary weapon on the ground and fill new info
            ItemDragHandler primaryGunRef = primaryGunImg.gameObject.GetComponent<ItemDragHandler>();


            primaryGun.GetComponent<InventoryItemDescription>().itemObjectRef = e.Gun;
            primaryGunImg.sprite = e.Gun.itemImage;
            primaryGunRef.itemDescription = e.Gun.description;
            primaryGunRef.itemType = e.Gun.itemType;

            // set the ammo sprite based on the typing from a dictionary sprite
            // lookup or just save the ammo sprite in the scritable object
            // and assign it here
            // for now, just leave it

            // set UI active to show it
            primaryGunRef.gameObject.SetActive(true);
            primaryGun.GetChild(1).gameObject.SetActive(true);

        }
        // 1 empty 1 full
        else
        {
            // when you have guns, deteremine if both are full and if they are,
            // drop the current primary weapon on the ground and fill new info
            ItemDragHandler secondaryGunRef = secondaryGunImg.gameObject.GetComponent<ItemDragHandler>();


            secondaryGun.GetComponent<InventoryItemDescription>().itemObjectRef = e.Gun;
            secondaryGunImg.sprite = e.Gun.itemImage;
            secondaryGunRef.itemDescription = e.Gun.description;
            secondaryGunRef.itemType = e.Gun.itemType;

            // set the ammo sprite based on the typing from a dictionary sprite
            // lookup or just save the ammo sprite in the scritable object
            // and assign it here
            // for now, just leave it

            // set UI active to show it
            secondaryGunRef.gameObject.SetActive(true);
            secondaryGun.GetChild(1).gameObject.SetActive(true);
        }
    }
}