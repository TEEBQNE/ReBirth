using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryItemDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // reference to the UI element that allows items to be over other items
    private GameObject draggingUIRef;

    // reference to the description text object
    private TextMeshProUGUI descriptionText;

    // reference to scriptable gameobject of the item in its place
    public ItemObjectScript itemObjectRef;

    public void OnPointerEnter(PointerEventData eventData)
    {
        // if it has an item, is an active item and not currently dragging another object
        if(transform.childCount > 0 && transform.GetChild(0).gameObject.activeInHierarchy && draggingUIRef.transform.childCount == 0)
        {
            // display the description of the object that it is hovering
            descriptionText.text = gameObject.transform.GetChild(0).GetComponent<ItemDragHandler>().itemDescription;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionText.text = "";

        // fix to weird UI issue where highlight on button no longer works?
        gameObject.GetComponent<Button>().enabled = false;
        gameObject.GetComponent<Button>().enabled = true;
    }

    public void UpdateModUI()
    {
        // if it is null, just deactivate the object, if it is not, update info and make sure it
        // is activated
        if(itemObjectRef == null)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            ItemDragHandler scriptRef = transform.GetChild(0).gameObject.GetComponent<ItemDragHandler>();

            scriptRef.itemDescription = itemObjectRef.description;
            scriptRef.itemType = itemObjectRef.itemType;
            scriptRef.uiListIndex = -1;
            scriptRef.gameObject.GetComponent<Image>().sprite = itemObjectRef.itemImage;
        }
    }

    public void UpdateGunUI()
    {
        // if it is null, just deactivate the object, if it is not, update info and make sure it
        // is activated
        if (itemObjectRef == null)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            ItemDragHandler scriptRef = transform.GetChild(0).gameObject.GetComponent<ItemDragHandler>();

            scriptRef.itemDescription = itemObjectRef.description;
            scriptRef.itemType = itemObjectRef.itemType;
            scriptRef.uiListIndex = -1;
            scriptRef.gameObject.GetComponent<Image>().sprite = itemObjectRef.itemImage;
        }
    }

    private void Start()
    {
        draggingUIRef = GameObject.Find("Canvas/InventoryUI/DraggingParent");
        descriptionText = GameObject.Find("Canvas/InventoryUI/ItemDescription").transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }
}