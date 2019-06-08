using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Rewired;
using UnityEngine.UI;
using System;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler 
{
    // vars for Rewired stuffs
    public int id;
    private Player player;

    // reference to the index in the list of UI items
    public int uiListIndex;

    // item type identification (0 = gun, 1 = mod1, 2 = mod2, 3 = mod3)
    public int itemType;

    // start position of object for resetting / snapping
    public Vector3 startPos;

    // item description
    public string itemDescription;

    // reference to the slot that they are in
    private GameObject inventorySlotParent;

    // reference of the inventory script obj
    private Inventory Inventory;

    // reference to dragging parent UI element
    private GameObject draggingUIElement;

    private const int MOD_INDEX_CONST = -1;

    public void OnDrag(PointerEventData eventData)
    {
        // set the image to the position of the mouse
        transform.position = new Vector3(player.controllers.Mouse.screenPosition.x, player.controllers.Mouse.screenPosition.y, transform.position.z);
        transform.SetParent(draggingUIElement.transform);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // detect if it is over another UI inventory slot or mod slot
        // if it is, check if item exists there, if it does, swap it
        // if not, then just assign it there and deactivate other image

        // create a new point event
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
        };

        //bool isMoved = false;

        // assign the event to the mouse data
        pointerData.position = player.controllers.Mouse.screenPosition;

        // create a temp list for cast results
        List<RaycastResult> results = new List<RaycastResult>();

        // cast down to all points from mouse data
        EventSystem.current.RaycastAll(pointerData, results);

        // loop through the results and see if anything can be swapped or moved
        foreach(RaycastResult raycastUIElements in results)
        {
            #region DroppedOverInventory
            // dropping into an inventory slot or into the proper mod slot (make sure it is NOT a gun)
            if (raycastUIElements.gameObject.tag == "Inventory" && itemType != 0)
            {
                GameObject tempInvObj = raycastUIElements.gameObject;
                Image tempInvObjImg = null;

                if (tempInvObj.transform.childCount > 0)
                    tempInvObjImg = tempInvObj.transform.GetChild(0).GetComponent<Image>();

                // determine if it is dragging from an inventory or mod slot

                // mod slot
                if (uiListIndex == -1)
                {
                    // swap
                    if(tempInvObjImg.isActiveAndEnabled)
                    {
                        // get temp reference to the mod info
                        ItemObjectScript tempObj = inventorySlotParent.GetComponent<InventoryItemDescription>().itemObjectRef;

                        // index of the inventory item in the inventory list
                        int tempIndex = tempInvObjImg.gameObject.GetComponent<ItemDragHandler>().uiListIndex;

                        // assign the new info to the mod
                        inventorySlotParent.GetComponent<InventoryItemDescription>().itemObjectRef = Inventory.GetInventoryIndexInfo(tempIndex);

                        // reset the objects position
                        ResetUIPosition();

                        // update the info to the UI
                        inventorySlotParent.GetComponent<InventoryItemDescription>().UpdateModUI();

                        // remove the old item from the list
                        Inventory.RemoveItem(tempIndex);

                        // turn off the image so that the inventory can add it
                        tempInvObjImg.gameObject.SetActive(false);

                        // add the new item the list
                        Inventory.AddItem(tempObj);
                    }
                    else
                    {
                        // add the object to the list
                        Inventory.AddItem(inventorySlotParent.GetComponent<InventoryItemDescription>().itemObjectRef);

                        // remove the reference from the mod
                        inventorySlotParent.GetComponent<InventoryItemDescription>().itemObjectRef = null;

                        // deactivate the object
                        gameObject.SetActive(false);
                    }
                }
                // inventory slot
                else
                {
                    // dropped back down on the same slot, reset position
                    if (tempInvObj.transform.childCount <= 0)
                    {
                        ResetUIPosition();
                    }
                    else if (tempInvObjImg.isActiveAndEnabled)
                    {
                        // swap UI images and the objects internal indexes
                        // swap the objects in the public object list
                        ItemDragHandler tempObjRef = tempInvObj.transform.GetChild(0).GetComponent<ItemDragHandler>();

                        int tempIndex = tempObjRef.uiListIndex;
                        Sprite tempImage = tempInvObjImg.sprite;
                        string tempDesc = tempObjRef.itemDescription;
                        int tempType = tempObjRef.itemType;

                        tempInvObjImg.sprite = gameObject.GetComponent<Image>().sprite;

                        tempObjRef.itemDescription = itemDescription;
                        tempObjRef.itemType = itemType;

                        gameObject.GetComponent<Image>().sprite = tempImage;

                        // swap the items in the list
                        Inventory.SwapItems(uiListIndex, tempObjRef.uiListIndex);

                        itemDescription = tempDesc;
                        itemType = tempType;
                    }
                    else
                    {
                        // dropped into an empty slot, populate new slot and clear old one
                        ItemDragHandler tempObjRef = tempInvObj.transform.GetChild(0).GetComponent<ItemDragHandler>();

                        tempInvObjImg.sprite = gameObject.GetComponent<Image>().sprite;
                        tempObjRef.uiListIndex = uiListIndex;
                        tempObjRef.itemDescription = itemDescription;
                        tempObjRef.itemType = itemType;

                        tempInvObjImg.gameObject.SetActive(true);

                        gameObject.SetActive(false);
                    }
                }
            }
            #endregion

            #region DroppedOverMod
            else if (raycastUIElements.gameObject.tag == "Mod" && Int32.Parse(raycastUIElements.gameObject.name[3].ToString()) == itemType)
            {
                // determine if they are dragging between mod slots or if it is from the inventory
                // do this by checking if the current id is -1 or something else
                // -1 means that it is from a mod

                GameObject tempInvObj = raycastUIElements.gameObject;
                Image tempInvObjImg = null;

                if (tempInvObj.transform.childCount > 0)
                    tempInvObjImg = tempInvObj.transform.GetChild(0).GetComponent<Image>();

                // is in a mod slot, dragging to a mod slot
                if (uiListIndex == MOD_INDEX_CONST)
                {
                    // swap their scritable object
                    // call function to update their info
                    // if one of them is null, then set the obj to null
                    // and unset all data for the image and deactivate it

                    if (tempInvObj.transform.childCount <= 0)
                    {
                        ResetUIPosition();
                    }
                    else
                    {
                        ItemObjectScript tempObj = inventorySlotParent.GetComponent<InventoryItemDescription>().itemObjectRef;

                        inventorySlotParent.GetComponent<InventoryItemDescription>().itemObjectRef = tempInvObj.GetComponent<InventoryItemDescription>().itemObjectRef;
                        tempInvObj.GetComponent<InventoryItemDescription>().itemObjectRef = tempObj;

                        ResetUIPosition();

                        // update UI for the mods
                        inventorySlotParent.GetComponent<InventoryItemDescription>().UpdateModUI();
                        tempInvObj.GetComponent<InventoryItemDescription>().UpdateModUI();
                    }
                }
                else
                {
                    // determine if it is a swap with an inventory item, or if it is an open spot
                    // if it is a swap, delete the old item, and add in the new one to the inv list
                    // if it is not a swap, then set the scriptable object to null an deactivate
                    // the mod UI while adding it to the list

                    // is a swap between two items from inv->mod
                    if(tempInvObjImg.isActiveAndEnabled)
                    {
                        // get temp reference to the mod info
                        ItemObjectScript tempObj = tempInvObj.GetComponent<InventoryItemDescription>().itemObjectRef;

                        // index of the inventory item in the inventory list
                        int tempIndex = uiListIndex;

                        // assign the new info to the mod
                        tempInvObj.GetComponent<InventoryItemDescription>().itemObjectRef = Inventory.GetInventoryIndexInfo(tempIndex);

                        // update the info to the UI
                        tempInvObj.GetComponent<InventoryItemDescription>().UpdateModUI();

                        // remove the old item from the list
                        Inventory.RemoveItem(tempIndex);

                        // reset the UI position
                        ResetUIPosition();

                        // turn off the image so that the inventory can add it
                        gameObject.SetActive(false);

                        // add the new item the list
                        Inventory.AddItem(tempObj);
                    }
                    // not a swap. inv->mod(empty)
                    else
                    {
                        // index of the inventory item in the inventory list
                        int tempIndex = uiListIndex;

                        // assign the new info to the mod
                        tempInvObj.GetComponent<InventoryItemDescription>().itemObjectRef = Inventory.GetInventoryIndexInfo(tempIndex);

                        // update the mod UI
                        tempInvObj.GetComponent<InventoryItemDescription>().UpdateModUI();

                        // remove the now moved item from the inventory
                        Inventory.RemoveItem(tempIndex);

                        // reset the position and deactivate the UI object for inv
                        ResetUIPosition();
                        gameObject.SetActive(false);
                    }
                }
            }
            #endregion
            #region GunDropped
            else if (itemType == 0 && raycastUIElements.gameObject.tag == "Gun")
            {
                // three actions that could occur
                // swapped, placed in same place, moved
                GameObject tempInvObj = raycastUIElements.gameObject;
                Image tempInvObjImg = null;

                if (tempInvObj.transform.childCount > 0)
                    tempInvObjImg = tempInvObj.transform.GetChild(0).GetComponent<Image>();

                // dropped back down on the same slot, reset position
                if (tempInvObj.transform.childCount <= 0)
                {
                    ResetUIPosition();
                }
                else
                {
                    // swap items (Works for actual swap or for movement with empty slot)
                    ItemObjectScript tempObj = inventorySlotParent.GetComponent<InventoryItemDescription>().itemObjectRef;

                    inventorySlotParent.GetComponent<InventoryItemDescription>().itemObjectRef = tempInvObj.GetComponent<InventoryItemDescription>().itemObjectRef;
                    tempInvObj.GetComponent<InventoryItemDescription>().itemObjectRef = tempObj;

                    ResetUIPosition();

                    // update UI for the gun (says mod, but it works the same)
                    // will change it to gun specific if scriptable object is changed
                    // or gun has more functionality / data
                    inventorySlotParent.GetComponent<InventoryItemDescription>().UpdateGunUI();
                    tempInvObj.GetComponent<InventoryItemDescription>().UpdateGunUI();
                }
            }
            #endregion
        }

        // if no movement occured, send it back to it's origin
        ResetUIPosition();
    }

    public void ResetUIPosition()
    {
        // resets position on screen
        transform.position = startPos;

        // sets the parent back to slot or gun panel
        transform.SetParent(inventorySlotParent.transform);

        // makes sure that it is the 0th index (for gun image with ammo)
        transform.SetSiblingIndex(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(id);
        startPos = transform.position;
        inventorySlotParent = transform.parent.gameObject;
        draggingUIElement = GameObject.Find("Canvas/InventoryUI/DraggingParent");
        Inventory = GameObject.Find("Canvas/InventoryUI/InventorySlots").GetComponent<Inventory>();
    }
}