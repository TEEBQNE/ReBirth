using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Shoot : MonoBehaviour
{    
    public float damage = 100f;
    public float baseDamage = 100f;
    public float fireRate = 15f;
    public float recoil;
    public float range;
    public float effectiveRange;
    public float CritMultiplier = 2.0f;
    public float impactForce;

    private float nextFireTime = 0f;

    public Camera rightCam;
    public Camera leftCam;
    private Camera activeCam;
    private bool camSwitch;

    //Vars for Rewired stuffs
    public int id;
    public Player player;

    //public particleSystem muzzleFlash;

    void Start()
    {
        player = ReInput.players.GetPlayer(id);
        activeCam = rightCam;
    }
    // Update is called once per frame
    void Update()
    {
        if (player.GetButtonDown("ShoulderSwitch"))
        {
            CamSwap();
        }

        if (player.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f/fireRate;
            Fire1();
        }
    }

    void CamSwap()
    {
    	   camSwitch = !camSwitch;
        if (camSwitch)
        {
            activeCam = leftCam;
        }
        else
        {
            activeCam = rightCam;
        }
        
    }

    void Fire1()
    {
        RaycastHit hit;
        //muzzleFlash.Play();


        if (Physics.Raycast(activeCam.transform.position, activeCam.transform.forward, out hit))
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