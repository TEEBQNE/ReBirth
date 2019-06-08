using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Rewired;
using System;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    // vars for Rewired stuffs
    public int id;
    private Player player;

    public Inventory Inventory;

    private GameObject draggingUIRef;

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;

        if(!RectTransformUtility.RectangleContainsScreenPoint(invPanel, 
        player.controllers.Mouse.screenPosition))
        {

            if (draggingUIRef.gameObject.transform.GetChild(0).GetComponent<ItemDragHandler>().itemType != 0)
            {
                Debug.Log("Dropped item");

                // need to remove based on an updating list index

                // determine if it is dragged out from the mod slot or not (-1 == mod slot, 0 - X = inv)
                if (draggingUIRef.transform.gameObject.transform.GetChild(0).GetComponent<ItemDragHandler>().uiListIndex != -1)
                    Inventory.RemoveItem(draggingUIRef.transform.gameObject.transform.GetChild(0).GetComponent<ItemDragHandler>().uiListIndex);
                else
                    print("Mod dropped");

                // reset the position (Need the dragging ref here as it has not been reset yet)
                draggingUIRef.transform.GetChild(0).gameObject.GetComponent<ItemDragHandler>().ResetUIPosition();
            }
            else
            {
                Debug.Log("Gun dropped");
                // reset the position (Need the dragging ref here as it has not been reset yet)
                draggingUIRef.transform.GetChild(0).gameObject.GetComponent<ItemDragHandler>().ResetUIPosition();

                // hide the ammo as well
                eventData.selectedObject.transform.GetChild(1).gameObject.SetActive(false);

            }

            // not item type specific calls

            // deactivate the image UI (Need the selected object here as it has now been reset)
            eventData.selectedObject.transform.GetChild(0).gameObject.SetActive(false);

            // wiping data from the mods
            eventData.selectedObject.GetComponent<InventoryItemDescription>().itemObjectRef = null;
            
        }
    }

    private void Start()
    {
        player = ReInput.players.GetPlayer(id);
        draggingUIRef = GameObject.Find("Canvas/InventoryUI/DraggingParent");
    }
}