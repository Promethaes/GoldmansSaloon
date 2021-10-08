using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] int maxHP = 3;
    public UnityEvent OnTakeDamage = new UnityEvent();
    public UnityEvent OnDie = new UnityEvent();

    int _hp = 0;

    private void Start()
    {
        _hp = maxHP;
    }

    public virtual void TakeDamage(int damage)
    {
        if (_hp <= 0)
            return;
        _hp -= damage;
        OnTakeDamage.Invoke();
        if (_hp <= 0)
            OnDie.Invoke();
    }

    public bool IsDead()
    {
        return maxHP <= 0;
    }

}
