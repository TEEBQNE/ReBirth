using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CameraCollider : MonoBehaviour
{
	//Variables
	public float min = 1.0f;
	public float max = 5.0f;
	public Vector3 dir;
	public float dist;
	public float smooth = 10.0f;

	void Awake()
	{
		//get direction and distance of camera
		dir = transform.localPosition.normalized;
		dist = transform.localPosition.magnitude;
	}

    void Update()
    {
    	//furthest distance for camera and ideal place when no collision is detected
        Vector3 camPos = transform.parent.TransformPoint (dir*max);
        RaycastHit hit;

        //if the camera will hit something then clamp
        if (Physics.Linecast(transform.parent.position, camPos, out hit))
        {
        	dist = Mathf.Clamp ((hit.distance*0.9f), min, max);
        }
        else
        {
        	dist = max;
        }

        //move camera accordingly via linear interpolation
        transform.localPosition = Vector3.Lerp(transform.localPosition, dir*dist, Time.deltaTime*smooth);
    }
}
