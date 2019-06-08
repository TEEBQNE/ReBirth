using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GunEventArgs : EventArgs
{
    public GunEventArgs(ItemObjectScript gun)
    {
        Gun = gun;
    }

    public ItemObjectScript Gun;
}