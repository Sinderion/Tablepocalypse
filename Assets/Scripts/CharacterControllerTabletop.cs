using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class CharacterControllerTabletop : MonoBehaviour
{
    public float moveSpeed = 1f;
    Vector2 moveInput;
    Rigidbody rb;
    bool jumped = false;
   

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    Vector2 GetFlatVersionOfVector3(Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }

    private void FixedUpdate()
    {
        Vector2 rbFlat = GetFlatVersionOfVector3(rb.position);


        Vector2 movement2d = rbFlat + moveInput * moveSpeed * Time.fixedDeltaTime;
        Vector3 movement3d = movement2d;

        movement3d.z = movement3d.y;
        movement3d.y = 0;

        rb.MovePosition(movement3d);
        if (jumped)
        {
            Debug.Log("jumping now!");
            rb.AddForce(transform.up * 500);
        }
    }
    private void OnJump()
    {
        jumped = true;
    }
    private void OnMove(InputValue value)
    {
       moveInput = value.Get<Vector2>();

    }

}
