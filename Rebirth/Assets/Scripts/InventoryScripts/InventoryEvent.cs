using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryEventArgs : EventArgs
{
   public InventoryEventArgs(ItemObjectScript item)
    {
        Item = item;
    }

    public ItemObjectScript Item;
}