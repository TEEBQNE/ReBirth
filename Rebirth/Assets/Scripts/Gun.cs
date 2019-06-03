﻿w using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Gun : MonoBehaviour
{	
	public float damage = 100f;
	public float baseDamage = 100f;
	public float fireRate;
	public float recoil;
	public float range;
	public float effectiveRange;
	public float CritMultiplier = 2.0f;

	public Camera cam;

	//Vars for Rewired stuffs
    public int id;
    private Player player = ReInput.players.GetPlayer(id);


    // Update is called once per frame
    void Update()
    {
			//cam = Camera.gameObject.GetComponentInChildren<Camera>();

        	if (player.GetButtonDown("Fire1"))
        	{
        		Fire1();
        	}
    }


	void Fire1()
	{
		RaycastHit hit;

		if (Physics.Raycast(cam.transform.position, cam.transform.position, out hit))
		{
			Debug.Log(hit.transform.name);

			Enemy target = hit.transform.GetComponent<Enemy>();
			if(target != null)
			{
				target.TakeDamage(baseDamage);
			}
		}

	}

}