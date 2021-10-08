using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Controller : MonoBehaviour
{
    public float movementSpeed = 1.0f;
    [Range(0.1f, 1.0f)]
    public float gamepadDeadzone = 0.15f;
    [Range(0.1f, 1.0f)]
    public float movementFalloffRate = 1.0f;

    [Header("References")]
    [SerializeField] Rigidbody2D rigidbody2D;
    [SerializeField] EntityHealth entityHealth;
    [SerializeField] EntityKick entityKick;

    bool _doMovementFalloff = true;

    Vector2 _movementVec = Vector2.zero;

    private void FixedUpdate()
    {
        if (entityHealth.IsDead())
        {
            rigidbody2D.velocity = Vector2.zero;
            return;
        }

        Vector2 force = _movementVec * movementSpeed;
        rigidbody2D.AddForce(force, ForceMode2D.Impulse);

        if (_doMovementFalloff && rigidbody2D.velocity.magnitude > 0.0f)
            rigidbody2D.AddForce(-rigidbody2D.velocity * movementFalloffRate, ForceMode2D.Impulse);
    }

    public void TemporarilyDisableMovementFalloff(float howLong)
    {
        _doMovementFalloff = false;
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(howLong);
            _doMovementFalloff = true;
        }
        StartCoroutine(Wait());
    }

    public void OnMove(CallbackContext context)
    {
         if(entityHealth.IsDead())
            return;
        _movementVec = context.ReadValue<Vector2>();
        if (_movementVec.magnitude < gamepadDeadzone)
            _movementVec = Vector2.zero;
    }
    public void OnKick(CallbackContext context)
    {
         if(entityHealth.IsDead())
            return;
        if (context.performed)
            entityKick.canKick = true;
        else if (context.canceled)
            entityKick.canKick = false;
    }
    public void OnBulletTime(CallbackContext ctx)
    {
        if(entityHealth.IsDead())
            return;
        BulletTime.OnBulletTime(ctx);
    }
}
