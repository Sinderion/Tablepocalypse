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
    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        oldPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsClient && IsOwner)
        {
            if (Vector3.Distance(oldPosition, transform.position) > 0)
            {
                MoveCubeServerRPC(transform.position, transform.rotation);
            }
            
        }
        //rb.MovePosition(networkPosition.Value);
        
    }

    [ClientRpc]
    void MoveCubeClientRpc()
    {
        transform.position = networkPosition.Value;
        transform.rotation = networkRotation.Value;

    }

    [ServerRpc]
    void MoveCubeServerRPC(Vector3 newPosition, Quaternion newRotation )
    {
        oldPosition = newPosition;
        networkPosition.Value = newPosition;
        networkRotation.Value = newRotation;
        MoveCubeClientRpc();
    }
}
