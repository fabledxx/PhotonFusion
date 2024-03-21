using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;
using Unity.Mathematics;

public class CharacterMovementHandler : NetworkBehaviour
{
    Vector2 viewInput;
    NetworkCharacterController characterController;

    float cameraRotationX = 0;

    void Awake()
    {
        characterController = GetComponent<NetworkCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        cameraRotationX += viewInput.y * Time.deltaTime * characterController.viewUpDownRotationSpeed;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -90,90);

    }

    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputData networkInputData))
        {
            characterController.Rotate(networkInputData.rotationInput);

            Vector3 movementDirection = transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x;
            movementDirection.Normalize();

            characterController.Move(movementDirection);

            if (networkInputData.isJumpPressed)
            {
                characterController.Jump();
            }
            if (networkInputData.isTeleportButtonPressed)
            {
                int randomNumber1 = UnityEngine.Random.Range(-3, 3);
                int randomNumber2 = UnityEngine.Random.Range(-3, 3);

                if (randomNumber1 <= 1 && randomNumber1 >= -1)
                    randomNumber1 = 2;


                Vector3 newPosition = new Vector3(transform.position.x + randomNumber1, transform.position.y, transform.position.z + randomNumber2); 
                characterController.Teleport(newPosition);
            }




            CheckFallRespawn();
        }
    }

    void CheckFallRespawn()
    {
        if (transform.position.y < -12)
            transform.position = Utils.GetRandomSpawnPoint();
    }

    internal void SetViewInputVector(Vector2 viewInputVector)
    {
        this.viewInput = viewInputVector;
    }

    public void SetCharacterControllerEnabled(bool isEnabled)
    {
        characterController.enabled = isEnabled;
    }
}
