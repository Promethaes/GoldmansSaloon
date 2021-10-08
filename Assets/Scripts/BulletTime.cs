using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class BulletTime : MonoBehaviour
{
    static float timeScaleChange = 0.75f;

    public static void OnBulletTime(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Time.timeScale = timeScaleChange;
            Time.fixedDeltaTime = timeScaleChange * 0.02f;
        }
        else if (ctx.canceled)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
    }
}
