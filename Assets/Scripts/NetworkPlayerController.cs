using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using TMPro;

public class NetworkPlayerController : NetworkBehaviour
{
    public float moveSpeed = 1f;
    //public NetworkVariable<Vector3> networkPosition = new(NetworkVariableReadPermission.Everyone, new (0,0,0));
    Vector2 movementInput;
    TMP_Text positionText;
    Vector3 oldPosition;

    Playerinput controls;

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

    CharacterController characterController;

    private void Awake()
    {
        positionText =  SessionController.singleton.positionText;
        characterController = GetComponent<CharacterController>();
    }
    private void Start()
    {
        oldPosition = transform.position;
        movementInput = new(0, 0);

        Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
        Controls.Player.Move.canceled += ctx => CancelMovement();
        if (!IsLocalPlayer)
        {
            Controls.Disable();
        }
       

    }

    private void FixedUpdate()
    {
        if (movementInput == Vector2.zero) return;

        Vector3 newPosition = moveSpeed * Time.deltaTime * new Vector3(movementInput.x, 0, movementInput.y);


        if (IsServer && IsLocalPlayer)
        {
            //characterController.Move(newPosition);
            MoveClientRpc(newPosition);
        }else if(IsClient && IsLocalPlayer)
        {
            NetworkOnMoveServerRpc(newPosition);
        }

        

        //characterController.Move(moveSpeed * Time.deltaTime * moveVector);
        /*
        if (IsServer && IsOwner)
        {
            //Host takes care of itself and lets clients know.
            if (movementInput.x != 0 || movementInput.y != 0)
            {
                Vector2 change = moveSpeed * Time.fixedDeltaTime * movementInput;
                transform.position = new Vector3(change.x, 0, change.y) + transform.position;
                MoveClientRpc(transform.position);
                //transform.position = new Vector3(change.x, 0, change.y) + transform.position;
                //transform.position = transform.position;
            }
        }
        else if (IsOwner)
        {
            if (movementInput.x != 0 || movementInput.y != 0)
            {
                Vector2 change = moveSpeed * Time.fixedDeltaTime * movementInput;
                
                NetworkOnMoveServerRpc(change.x, change.y);
            }
        }*/

        
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
            positionText.text = $"Position: x:{transform.position.x},z: {transform.position.z} Moveinput: {movementInput.x}, {movementInput.y}";

        }
    }

    void SetMovement(Vector2 inputVector)
    {
        Debug.Log("Movement happening");
        movementInput = inputVector;
    }
    void CancelMovement()
    {
        Debug.Log("movement Stopping");
        movementInput = Vector2.zero;

    }

    [ClientRpc]
    public void MoveClientRpc(Vector3 updatedPosition)
    {
        characterController.Move(updatedPosition);
       // Debug.Log($"New x: {transform.position.x}. Moveinput.x = {moveInput.x}");
    }

    [ServerRpc]
    public void NetworkOnMoveServerRpc(Vector3 newPosition)
    {
        /*
            Vector3 change = new (x, 0, z);
            Vector3 newPosition = transform.position + change;
            //transform.position = newPosition;*/
            MoveClientRpc(newPosition);   
    }
    
    private void OnMove(InputValue value)
    {
        /*
        //Debug.Log($"Onmove fired.{IsServer} {IsClient} {IsOwnedByServer} {IsLocalPlayer}");
        if (IsLocalPlayer)
        {
            movementInput = value.Get<Vector2>();
        }*/
    }


    
    private void OnEscape() { 
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }



}
