using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float moveSpeed = 8;
    public float moveSmoothingTime = -0.1f;
    float currentVelocityX = 0;
    float currentVelocityZ = 0;
    public float gravityForce = -20;
    public float jumpForce = 8;
    Vector3 currentMovement = new Vector3();
    
    public Transform camera;
    public float lookSensitivity;
    public float maxY = 90;
    public float minY = -90;
    float yRotation = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        //lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UpdateMovment(Vector3 movement, Vector3 lookVector, bool jump)
    {
        //look around
        transform.Rotate(transform.up, lookVector.x * lookSensitivity);

        yRotation += lookVector.y * lookSensitivity;

        yRotation = Mathf.Clamp(yRotation, minY, maxY);
        camera.eulerAngles = new Vector3(-yRotation, camera.eulerAngles.y, camera.eulerAngles.z);

        //movement
        //clamp the diagonal movement and limit the speed to the same as horiz and vert speeds
        if(movement.magnitude > 1)
        {
            movement.Normalize();
        }
        
        //take movment vector and apply it to current rotation
        movement = transform.TransformDirection(movement); // transforms position from local space to world space so that the WASD movement aligns with the position we are facing rather then the world direction
        movement *= moveSpeed;

        //smooth the movement
        currentMovement.x = Mathf.SmoothDamp(currentMovement.x, movement.x, ref currentVelocityX, moveSmoothingTime); 
        currentMovement.z = Mathf.SmoothDamp(currentMovement.z, movement.z, ref currentVelocityZ, moveSmoothingTime);

        //jumping
        if (jump && controller.collisionFlags.HasFlag(CollisionFlags.Below))
        {
            currentMovement.y = jumpForce;
        }
        else
        {
            currentMovement.y += gravityForce * Time.deltaTime;
        }

        //clamp y speed so we dont go too fast up or down
        currentMovement.y = Mathf.Clamp(currentMovement.y, gravityForce, jumpForce);

        controller.Move(currentMovement * Time.deltaTime);
        
    }
}
