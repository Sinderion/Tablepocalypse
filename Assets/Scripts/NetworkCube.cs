using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkCube : NetworkBehaviour
{
    NetworkVariable<Vector3> networkPosition = new();
    NetworkVariable<Quaternion> networkRotation = new();
    //Rigidbody rb;
    Vector3 oldPosition;
    Quaternion oldRotation;
    public float networkMovePerSecond = 5;
    //Vector3 cachedPosition = new();
    float movementDelay = 0;
    float movementWaiting = 0;
    float fixedDeltaTime;


    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        oldPosition = transform.position;
        oldRotation = transform.rotation;

        fixedDeltaTime = Time.fixedDeltaTime;
        movementDelay = 1 / networkMovePerSecond;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movementWaiting += fixedDeltaTime;
        if (movementWaiting < movementDelay)
        {
            
            return;
        }else if (Vector3.Distance(oldPosition, transform.position) > 0 || Quaternion.Angle(oldRotation, transform.rotation) > 0)
            {

                MoveCubeServerRPC(transform.position, transform.rotation);
            movementWaiting = 0;
            }
            
        
        //rb.MovePosition(networkPosition.Value);
        
    }

    [ClientRpc]
    void MoveCubeClientRpc()
    {
        transform.position = networkPosition.Value;
        transform.rotation = networkRotation.Value;
    }

    [ServerRpc(RequireOwnership = false)]
    void MoveCubeServerRPC(Vector3 newPosition, Quaternion newRotation )
    {
        oldPosition = newPosition;
        networkPosition.Value = newPosition;
        networkRotation.Value = newRotation;
        MoveCubeClientRpc();
    }
}
