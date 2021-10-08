using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityKick : MonoBehaviour
{
    [SerializeField] List<string> kickableTags = new List<string>();
    [SerializeField] float kickScalar = 5.0f;
    [HideInInspector] public bool canKick = false;
    //may not need this
    public UnityEvent OnKick;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canKick || !kickableTags.Contains(other.tag))
            return;

        var rigid = other.GetComponent<Rigidbody2D>();
        var direction = transform.position - other.transform.position;
        direction = direction.normalized;

        rigid.AddForce(direction * kickScalar, ForceMode2D.Impulse);

        EntityKick entityKick = null;
        if (other.TryGetComponent<EntityKick>(out entityKick))
            entityKick.WasKicked(other.gameObject);
        OnKick.Invoke();
    }

    public virtual void WasKicked(GameObject other)
    {
    }
}
