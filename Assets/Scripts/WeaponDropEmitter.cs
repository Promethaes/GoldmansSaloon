using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponDropEmitter : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] int dropChance = 100;

    public void TryDropWeapon()
    {
        if (dropChance == 0.0f)
            return;
        int chance = Random.Range(0, 101);
        if (chance > dropChance)
            return;
        GameObject w = WeaponDropManager.GetSpawnedWeapon();
        if (w == null)
            return;
        var nw = GameObject.Instantiate(w);
        nw.transform.position = transform.position;
    }
}
