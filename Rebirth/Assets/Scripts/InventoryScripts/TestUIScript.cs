using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUIScript : MonoBehaviour
{
    public Inventory Inventory;

    public ItemObjectScript testItem;

    public void UITest()
    {
        Inventory.AddItem(testItem);
    }

    public void GunUITest()
    {
        Inventory.AddGun(testItem);
    }
}