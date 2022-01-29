using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponDrop : MonoBehaviour
{
    [SerializeField] PlayerController.GunEnum type = PlayerController.GunEnum.Base;
    [SerializeField] UnityEvent OnPickup = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        var pc = other.GetComponent<PlayerController>();
        OnPickup.Invoke();
        pc.SetCurrentGun(type);
        Destroy(gameObject);
    }
}
