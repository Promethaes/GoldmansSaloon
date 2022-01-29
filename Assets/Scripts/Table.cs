using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [System.Serializable]
    public enum PotionType
    {
        None,
        Heal,
        Invince,
        Revive,
    }
    [System.Serializable]
    public class PotionHelper
    {
        public Sprite tablePotionSprite;
        public AudioSource potionSound;
        public float chance = 0.0f;
        public int scoreValue = 100;
    }
    public float chanceToBePotionTable = 25.0f;
    public PotionHelper heal;
    public PotionHelper invince;
    public PotionHelper revive;
    public AudioSource knockedOver;
    public Sprite knockedOverSprite;

    [HideInInspector] public PotionType potionType = PotionType.None;
    PotionHelper selectedHelper = null;
    bool isKnockedOver = false;
    private void OnEnable()
    {
        float isPotTable = Random.Range(0.0f, 100.0f);
        if (isPotTable <= chanceToBePotionTable)
        {
            var renderer = GetComponent<SpriteRenderer>();
            float type = Random.Range(0.0f, 100.0f);
            if (type <= revive.chance)
            {
                potionType = PotionType.Revive;
                selectedHelper = revive;
            }
            if (type <= invince.chance)
            {
                potionType = PotionType.Invince;
                selectedHelper = invince;
            }
            else
            {
                potionType = PotionType.Heal;
                selectedHelper = heal;
            }
            renderer.sprite = selectedHelper.tablePotionSprite;
        }
        else
            potionType = PotionType.None;


        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("PTable"))
            return;

        var pc = other.gameObject.GetComponent<PlayerController>();
        if (!pc.IsKicking() || isKnockedOver)
            return;

        isKnockedOver = true;
        if (selectedHelper != null)
        {
            pc.AddScore(selectedHelper.scoreValue);
            selectedHelper.potionSound.Play();
        }
    }

    public int GetKicked()
    {
        if (isKnockedOver)
            return 0;
        isKnockedOver = true;
        knockedOver.Play();
        GetComponent<SpriteRenderer>().sprite = knockedOverSprite;
        if (selectedHelper != null)
        {
            selectedHelper.potionSound.Play();
            return selectedHelper.scoreValue;
        }
        return 0;

    }

    public void OnDie()
    {
        Destroy(gameObject);
    }
}
