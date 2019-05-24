using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CameraFollow : MonoBehaviour
{
	//Variables
	public float CameraMoveSpeed = 100.0f;
	public GameObject CameraFollowObj;
	public float clampAngle = 80.0f;
	public float inputSensitivity = 150.0f;
	
    //Vars for rotations
	public float finalInputX;
	public float finalInputY;
	public float mouseX;
	public float mouseY;
	public float controllerX;
	public float controllerZ;
	private float rotX = 0.0f;
	public float rotY = 0.0f;

    //Vars for Rewired stuffs
    public int id;
    private Player player;

    //Vars for ShoulderSwitch
    public bool shoulder = false;
    public Camera LeftCamera;
    public Camera RightCamera;

    void Start()
    {
    	//Get current rotation of Camera
     	Vector3 rot = transform.localRotation.eulerAngles;
     	rotX = rot.x;
     	rotY = rot.y;

     	//Lock Cursor
     	Cursor.lockState = CursorLockMode.Locked;
     	Cursor.visible = false;

        player = ReInput.players.GetPlayer(id);

        //enable/dissable cameras initially
        LeftCamera.gameObject.SetActive(false);
        RightCamera.gameObject.SetActive(true);

    }

    void Update()
    {
        //Get mouse inputs for camera rotation
    	CameraRotation();

    	//Transform camera to follow player
    	FollowPlayer();

        //Switch cameras on input
    	ShoulderSwitch();

    }

    void CameraRotation()
    {
        //Get mouse and controller inputs
        mouseX = player.GetAxis("Mouse Horizontal");//Input.GetAxis("Mouse X");
        mouseY = player.GetAxis("Mouse Vertical");//Input.GetAxis("Mouse Y");
        //Use these comments for controller compatibility (not sure how this works with rewired EDIT:wont work with rewired)
        //float controllerX = Input.GetAxis ("RightStickHorizontal");
        //float controllerZ = Input.GetAxis ("RightStickVertical");
        finalInputX = /*controllerInputX +*/ mouseX;
        finalInputY = -(/*controllerInputZ +*/ mouseY);

        //change rotation based on inputs
        rotX += finalInputY*inputSensitivity*Time.deltaTime;
        rotY += finalInputX*inputSensitivity*Time.deltaTime;

        //Clamp X rotation
        rotX = Mathf.Clamp (rotX, -clampAngle, clampAngle);

        //transform rot using rotX, rotY
        Quaternion localRotation = Quaternion.Euler (rotX, rotY, 0.0f);
        transform.rotation = localRotation;
    }

    void FollowPlayer()
    {
        Transform target = CameraFollowObj.transform;
        float step = CameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

    void ShoulderSwitch()
    {
        //on keypress (key is 'c')
        if (player.GetButtonDown("ShoulderSwitch"))
        {
            //switch bool over and enable/dissable cameras
            shoulder = !shoulder;
            LeftCamera.gameObject.SetActive(shoulder);
            RightCamera.gameObject.SetActive(!shoulder);
        }
    }
}
