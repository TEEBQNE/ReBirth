using System.Collections;
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
	public float impactForce;

	public Camera cam;

	//Vars for Rewired stuffs
    public int id;
    public Player player;

    //public particleSystem muzzleFlash;

    // Update is called once per frame
    void Update()
    {
			//cam = Camera.main;

    		player = ReInput.players.GetPlayer(id);
        	if (player.GetButtonDown("Fire1"))
        	{
        		Fire1();
        	}
    }


	void Fire1()
	{
		RaycastHit hit;
		//muzzleFlash.Play();


		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
		{
			Debug.Log(hit.transform.name);

			Enemy target = hit.transform.GetComponent<Enemy>();
			if(target != null)
			{
				target.TakeDamage(baseDamage);
			}
			if (hit.rigidbody != null)
			{
				hit.rigidbody.AddForce(-hit.normal * impactForce);
			}
		}
		//Debug.DrawRay(ray, Color.red);
		//GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
		//Destroy(impact, 2f);
	}

}