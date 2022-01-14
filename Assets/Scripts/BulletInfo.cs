using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInfo : MonoBehaviour
{
    public float maxLifetime = 5.0f;
    public int damage = 1;
    public float bulletSpeed = 2.0f;

    [Tooltip("Only use for player bullets. This is used to determine who to give score to when an enemy dies.")]
    public PlayerController bulletOwner;

    float _lifetime = 0.0f;
    private void OnEnable()
    {
        _lifetime = maxLifetime;
    }
    private void Update()
    {
        if (_lifetime > 0.0f)
            _lifetime -= Time.deltaTime;
        if (_lifetime < 0.0f)
            Destroy(gameObject);
    }

    //note, unity is doing the damage twice because of how colliders work...fix this later
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.CompareTag("PBullet") && other.gameObject.CompareTag("Enemy"))
        {
            var e = other.gameObject.GetComponent<EntityHealth>();
            e?.TakeDamage(damage);
            gameObject.SetActive(false);
            if (e?.GetCurrentHP() <= 0)
                bulletOwner.AddScore(e.GetCurrentHP());
        }
        else if (gameObject.CompareTag("EBullet"))
        {
            if (other.gameObject.CompareTag("Table"))
            {
                other.gameObject.GetComponent<EntityHealth>().TakeDamage(damage);
                gameObject.SetActive(false);
                return;
            }
            else if (other.gameObject.CompareTag("Player"))
            {
                var pc = other.gameObject.GetComponent<PlayerController>();
                pc?.TakeDamage(damage);
                gameObject.SetActive(false);
            }
        }
    }
}
