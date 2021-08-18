using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum BulletSpawnpointMovementType
    {
        SyncWithPlayer,
        FollowAnimationCurve
    }
    public int maxHP = 3;

    [Header("Gun Related")]
    [Tooltip("This is for when you need the position of the bullet to change based on the position of the closest player, or if you need the bullet spawn position to change based on an animation curve.")]
    public bool canMoveBulletSpawnpoint = false;
    [Tooltip("Only use this when canMoveBulletSpawnpoint is true.")]
    public BulletSpawnpointMovementType bulletSpawnpointMovementType;
    [Tooltip("The number of times the bullet start point should lerp back and forth. One cycle is a round trip. NYE: Set to -1 to only lerp for half a cycle.")]
    public int numLerpCycles = 0;

    [Header("References")]
    [Tooltip("Only use this when canMoveBulletSpawnpoint is true.")]
    public Transform bulletSpawnPoint;
    public Gun gun;
    [Tooltip("Lerp position 1 for animation curve style bullet spawnpoint movement.")]
    public Transform animCurvePosition1;
    [Tooltip("Lerp position 2 for animation curve style bullet spawnpoint movement.")]
    public Transform animCurvePosition2;
    [Tooltip("Gets disabled on death and re-enabled on enable.")]
    public CircleCollider2D triggerVolume;
    public GameObject deathParticles;
    public SpriteRenderer spriteRenderer;
    //public List<Sprite> 

    int _currentHP = 0;
    private void OnEnable()
    {
        _currentHP = maxHP;
        gun.SetGunOrientation(Gun.GunOrientation.Forward);
    }
    // Update is called once per frame
    void Update()
    {
        gun.Shoot();
    }

    public void TakeDamage(int damage){
        _currentHP -= damage;
        if(_currentHP <= 0){
            //IEnumerator Die(){
            //    deathParticles.SetActive(true);
            //}
        }
    }
}
