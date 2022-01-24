using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] int maxHP = 3;
    [SerializeField] int scoreValue = 0;
    public UnityEvent OnTakeDamage = new UnityEvent();
    public UnityEvent OnDie = new UnityEvent();


    int _hp = 0;

    private void OnEnable()
    {
        _hp = maxHP;
    }

    public virtual void TakeDamage(int damage)
    {
        if (_hp <= 0)
            return;
        if (gameObject.name.Contains("Outlaw"))
            Debug.Log("yes");
        _hp -= damage;
        OnTakeDamage.Invoke();
        if (_hp <= 0)
            OnDie.Invoke();
    }

    public bool IsDead()
    {
        return maxHP <= 0;
    }

    public int GetCurrentHP()
    {
        return _hp;
    }

    public void FullHeal()
    {
        _hp = maxHP;
    }

    public int GetScoreValue()
    {
        return scoreValue;
    }
}
