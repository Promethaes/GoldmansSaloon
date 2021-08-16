using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [HideInInspector] public int hp = 3;

    private void OnEnable()
    {
        hp = 3;
        //add potion code here
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private void Update()
    {
        if (hp <= 0)
            gameObject.SetActive(false);
    }
}
