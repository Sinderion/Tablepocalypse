using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using TMPro;

public class NetworkPlayerController : NetworkBehaviour
{
    public float moveSpeed = 1f;
    Vector2 movementInput;
    public float networkMovePerSecond = 5;
    Vector3 cachedPosition = new();
    float movementDelay = 0;
    float movementWaiting = 0;
    float fixedDeltaTime;

    int networkUpdatesSent = 0;

    TMP_Text positionText;
    Playerinput controls;
    Rigidbody rb;
    

    public Camera playerCamera;

    Playerinput Controls
    {
        get
        {
            if(controls != null)
            {
                return controls;
            }
            return controls = new Playerinput();
        }
    }
    //CharacterController characterController;
    void DEBUGnetworkUpdate()
    {
        networkUpdatesSent++;
        Debug.Log($"Network updates: {networkUpdatesSent}");
    }
    private void Awake()
    {
        positionText =  SessionController.singleton.positionText;
        //characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        if (!IsLocalPlayer)
        {
            playerCamera.GetComponent<AudioListener>().enabled = false;
            playerCamera.enabled = false;
            Controls.Disable();
        }
        fixedDeltaTime = Time.fixedDeltaTime;
       
        movementInput = new(0, 0);
        movementDelay = 1/networkMovePerSecond;
        Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
        Controls.Player.Move.canceled += ctx => CancelMovement();

    }

    private void FixedUpdate()
    {
        if (movementInput == Vector2.zero) return;
        Vector3 newPosition = moveSpeed * fixedDeltaTime * new Vector3(movementInput.x, 0, movementInput.y) + transform.position;

        movementWaiting += fixedDeltaTime;
        if(movementWaiting < movementDelay)
        {
            cachedPosition += newPosition;
            return;
        }else if (IsServer && IsLocalPlayer)
        {
            movementWaiting = 0;
            MoveClientRpc(newPosition);
        }else if(IsClient && IsLocalPlayer)
        {
            movementWaiting = 0;
            NetworkOnMoveServerRpc(newPosition);
        }
    }
    private void OnEnable()
    {
        Controls.Enable();
    }
    private void OnDisable()
    {
        Controls.Disable();
    }
    private void Update()
    {
        if (IsClient && IsOwner)
        {
            //For debug information.
            positionText.text = $"Position: x:{transform.position.x},z: {transform.position.z} Moveinput: {movementInput.x}, {movementInput.y}";
        }
    }
    void SetMovement(Vector2 inputVector)
    {
        movementInput = inputVector;
    }
    void CancelMovement()
    {
        movementInput = Vector2.zero;
    }
    [ClientRpc]
    public void MoveClientRpc(Vector3 updatedPosition)
    {
        //characterController.Move(updatedPosition);
        DEBUGnetworkUpdate();
        rb.MovePosition(updatedPosition);
        //transform.position = updatedPosition;

    }
    [ServerRpc]
    public void NetworkOnMoveServerRpc(Vector3 newPosition)
    {
        DEBUGnetworkUpdate();
            MoveClientRpc(newPosition);   
    }
    private void OnEscape() { 
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
