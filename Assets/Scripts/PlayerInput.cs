using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{

    Vector3 moveVector = new Vector3();
    Vector2 lookVector = new Vector2();

    public PlayerMovement movement;
    public UnityEvent shoot;
    public UnityEvent endShoot;
    public EventTypes.IntEvent scrollWheel;

    // events are a special type of delegate (a function) thats used to call other functions,
    // these functions are listening to the event and when its invoked these functions will run


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    // GetInput gets player input
    void GetInput()
    {
        moveVector.x = Input.GetAxisRaw("Horizontal");
        moveVector.z = Input.GetAxisRaw("Vertical");

        lookVector.x = Input.GetAxis("Mouse X");
        lookVector.y = Input.GetAxis("Mouse Y");

        bool jump = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0)) // left mouse button
        {
            shoot.Invoke(); // call the unity event
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            endShoot.Invoke();
        }

        if (Input.mouseScrollDelta.y != 0f)
        {
            scrollWheel.Invoke((int)Mathf.Sign(Input.mouseScrollDelta.y));
            //print("Scrollimg");
        }

        movement.UpdateMovment(moveVector, lookVector, jump);
    }

}
