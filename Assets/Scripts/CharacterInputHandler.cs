using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isJumpButtonPressed = false;
    bool isTeleporButtonPressed = false;
    bool isFireButtonPressed = false;
    public Vector3 aimFowardVector;
    public Transform aimPoint;
    CharacterMovementHandler CharacterMovementHandler;

    private void Awake()
    {
        CharacterMovementHandler = GetComponent<CharacterMovementHandler>();
    }

    private void Update()
    {
        //viewInputVector.x = Input.GetAxis("Mouse x");
        //viewInputVector.y = Input.GetAxis("Mouse y") * -1;

        if (!CharacterMovementHandler.Object.HasInputAuthority)
            return;

        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            isJumpButtonPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            isTeleporButtonPressed = true;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            isFireButtonPressed = true;

            aimFowardVector = aimPoint.transform.position;
        }
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        networkInputData.rotationInput = viewInputVector.x;

        networkInputData.movementInput = moveInputVector;

        networkInputData.isJumpPressed = isJumpButtonPressed;
        networkInputData.isTeleportButtonPressed = isTeleporButtonPressed;
        networkInputData.isFireButtomPressed = isFireButtonPressed;
        networkInputData.aimFowardVector = aimFowardVector;

        isTeleporButtonPressed = false;
        isJumpButtonPressed = false;
        isFireButtonPressed = false;

        return networkInputData;
    }

}
