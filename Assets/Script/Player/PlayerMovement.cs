using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0f )
        {
            velocity.y = -2f;
        }

        // Getting the inputs
        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical");

        // Creating the moving vector
        Vector3 move = transform.right * x + transform.forward * z; //(right - red axis, forward - blue axis)

        // actually moving the player
        controller.Move(move * speed * Time.deltaTime);

        // check if player can jump
        if(Input.GetButtonDown("Jump") && isGrounded )
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // falling down
        velocity.y += gravity * Time.deltaTime;

        // exectuting the jump
        controller.Move(velocity * Time.deltaTime);

        if(lastPosition != gameObject.transform.position && isGrounded == true) 
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        lastPosition = gameObject.transform.position;
    }
}
