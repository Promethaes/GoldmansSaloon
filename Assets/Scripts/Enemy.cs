using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public enum ShootType
    {
        Basic,
        FollowPlayerLoose,
        FollowPlayerExactly,
        [Tooltip("This class will NOT call gun.shoot")]
        HandledExternally
    }
    public int maxHP = 3;
    public int scoreValue = 100;

    [Header("Gun Related")]
    [Tooltip("Basic: Sets the gun's orientation to straight down.\nFollowPlayerLoose: Gun orientation depends on whether the player is to the left or right of the enemy.\nFollowPlayerExactly: Bullet start position will change depending on player position relative to this enemy. NOTE: if using this mode, make sure to properly hook up the BulletStartPoint reference.")]
    public ShootType shootType = ShootType.Basic;
    [Tooltip("Used with ShootType FollowPlayerExactly. Changes how far away from the enemy the bullet will spawn.")]
    public float bulletStartPointDistanceScalar = 2.0f;
    [Tooltip("Used with ShootType FollowPlayerExactly. Changes how high the bullet can be relative to this enemy.")]
    public float bulletYPosLimit = 0.4f;

    [Header("References")]
    public Gun gun;
    [Tooltip("Only used when ShootType is set to FollowPlayerExactly.")]
    public Transform bulletStartPoint;
    public GameObject deathParticles;
    public AudioSource deathSound;

    PlayerController[] _players = null;

    EntityHealth health = null;

    private void OnEnable()
    {
        health = GetComponent<EntityHealth>();
        health.FullHeal();
        IEnumerator BasicShoot()
        {
            gun.SetGunOrientation(Gun.GunOrientation.Forward);
            while (health.GetCurrentHP() > 0)
            {
                yield return new WaitForEndOfFrame();
                gun.Shoot();
            }
        }

        IEnumerator FollowPlayerLooseShoot()
        {
            while (health.GetCurrentHP() > 0)
            {
                yield return new WaitForEndOfFrame();

                var direction = FindClosestPlayer().position - transform.position;
                direction = direction.normalized;

                if (direction.x > 0.0f)
                    gun.SetGunOrientation(Gun.GunOrientation.Right);
                else //default to shooting left if < 0 or on 0
                    gun.SetGunOrientation(Gun.GunOrientation.Left);
                gun.Shoot();
            }
        }
        IEnumerator FollowPlayerExactlyShoot()
        {
            gun.SetGunOrientation(Gun.GunOrientation.Forward);
            while (health.GetCurrentHP() > 0)
            {
                yield return new WaitForEndOfFrame();

                var direction = FindClosestPlayer().position - transform.position;
                direction = direction.normalized;
                if (direction.y > bulletYPosLimit)
                    continue;
                bulletStartPoint.position = transform.position + direction * bulletStartPointDistanceScalar;
                gun.Shoot();
            }
        }

        //select method of shooting
        switch (shootType)
        {
            case ShootType.Basic:
                StartCoroutine(BasicShoot());
                break;
            case ShootType.FollowPlayerLoose:
                StartCoroutine(FollowPlayerLooseShoot());
                break;
            case ShootType.FollowPlayerExactly:
                StartCoroutine(FollowPlayerExactlyShoot());
                break;
            case ShootType.HandledExternally:
                break;
        }
    }

    public Transform FindClosestPlayer()
    {
        if (_players == null)
            _players = FindObjectsOfType<PlayerController>();
        Transform pTransform = null;
        foreach (var p in _players)
        {
            if (pTransform == null)
            {
                pTransform = p.transform;
                continue;
            }

            //find the closer player
            var currentDist = (transform.position - pTransform.position).magnitude;
            var tempDist = transform.position - p.transform.position;
            if (tempDist.magnitude < currentDist)
                pTransform = p.transform;
        }
        return pTransform;
    }

    public int GetScoreValue()
    {
        return scoreValue;
    }

    public void OnDie()
    {
        if (!GetComponent<SpriteRenderer>().enabled)
            return;
        GetComponent<SpriteRenderer>().enabled = false;
        deathSound.Play();
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.25f);
            Destroy(gameObject);
        }
        StartCoroutine(Wait());
    }
}
