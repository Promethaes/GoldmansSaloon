using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerOrientation : EntityOrientation
{
    float _xDir = 0.0f;
    public void OnLook(CallbackContext ctx)
    {
        _xDir = ctx.ReadValue<Vector2>().x;
    }

    private void Update()
    {
        DetermineOrientation();
    }

    public override void DetermineOrientation()
    {
        if (_xDir < -0.3f)
            orientation = Orientation.Left;
        else if (_xDir > 0.3f)
            orientation = Orientation.Right;
        else
            orientation = Orientation.Forward;
        
        base.DetermineOrientation();
    }
}
