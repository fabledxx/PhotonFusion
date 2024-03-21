using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{

    public const byte MOUSEBUTTON0 = 1;
    public const byte MOUSEBUTTON1 = 2;

    public NetworkButtons buttons;
    public Vector3 direction;
    public Vector3 aimFowardVector;

    public Vector2 movementInput;
    public float rotationInput;
    public NetworkBool isJumpPressed;
    public NetworkBool isTeleportButtonPressed;
    public NetworkBool isFireButtomPressed;

    private bool _mouseButton0;
    //private void update()
    //{
    //    _mousebutton0 = _mousebutton0 | input.getmousebutton(0);
    //}
}

