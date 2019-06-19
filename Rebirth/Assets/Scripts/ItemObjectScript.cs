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



    //gun things
    public float damage;
    public float critMult;
    public float fireRate;
    public float recoil;
    public float range;
    public float effectiveRange;
    public float impactForce;

	public int ammoType;



    //mod1 things
    public float damageMod;
    public float critMultMod;

    //mod2 things
    public float recoilMod;
    public float fireRateMod;

    //mod3 things
    //element? 0 = no element, 1 = fire
    public int element;

}
