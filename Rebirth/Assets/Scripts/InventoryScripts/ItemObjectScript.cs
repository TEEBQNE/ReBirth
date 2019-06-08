using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemObjectScript : ScriptableObject
{
    public string itemName;
    public string description;

    public Sprite itemImage;

    // 0 = gun, 1 = mod1, 2 = mod2, 3 = mod3
    public int itemType;

    // no idea about what ammo types we have
    public int ammoType;
}