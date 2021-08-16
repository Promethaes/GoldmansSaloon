using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInfo : MonoBehaviour
{
    public int damage = 1;
    public float bulletSpeed = 2.0f;

    //note, unity is doing the damage twice because of how colliders work...fix this later
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.CompareTag("PBullet") && other.gameObject.CompareTag("Enemy"))
        {
            //TODO: insert enemy damage here
            gameObject.SetActive(false);
        }
        else if (gameObject.CompareTag("EBullet"))
        {
            if (other.gameObject.CompareTag("Table"))
            {
                other.gameObject.GetComponent<Table>().hp -= damage;
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
