using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerMovement : MonoBehaviour
{
    public float _speed = 10;
    public int id;
    public float inputSensitivity = 150.0f;

    private Player player;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public float gravMulti = 1.0f;

    bool isTouchingGround = false;

    float height; //Half the height of the mesh 

    // Start is called before the first frame update
    virtual public void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        height = (this.gameObject.GetComponent<MeshRenderer>().bounds.size.y / 2.0f) + 0.05f;
        player = ReInput.players.GetPlayer(id);
    }

    // Update is called once per frame
    virtual public void Update()
    {
        checkMovement();
    }

    void checkMovement()
    {
        
        float x = player.GetAxis("Move Horizontal") * _speed;
        float z = player.GetAxis("Move Vertical") * _speed;

        float mouseX = Input.GetAxis("Mouse X");
        //add sensitivity for compatibility with camera
        float finalMouseX = mouseX*inputSensitivity*Time.deltaTime;

        this.transform.RotateAround(transform.position, transform.up, finalMouseX);

        isTouchingGround = Physics.Raycast(this.gameObject.transform.position, Vector3.down, out RaycastHit hit, height);

        if (isTouchingGround)
        {
            rb.velocity = (transform.forward * z + transform.right * x + transform.up * rb.velocity.y);
            if (player.GetButtonDown("Jump"))
                rb.AddForce(Vector3.up * 10, ForceMode.VelocityChange);
        }
        else
        {
            rb.AddForce(transform.forward * z + transform.right * x);
            rb.velocity += Vector3.up * Physics.gravity.y * 1.5f * Time.deltaTime * gravMulti;
        }

    }
}
