using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    [System.Serializable]
    public enum AnimationType
    {
        FlipFlop,
        OrientationDependant
    }
    public AnimationType animationType = AnimationType.FlipFlop;
    [Header("References")]
    public SpriteRenderer spriteRenderer;
    public Sprite activeFrame1 = null;
    public Sprite activeFrame2 = null;
    public Enemy thisEnemy;
    void OnEnable()
    {
        //flip a flag every 0.25 seconds, which will then switch the sprite in the sprite renderer
        IEnumerator FlipFlopAnimation()
        {
            spriteRenderer.sprite = activeFrame1;
            float timer = 0.0f;
            bool frame1Or2 = false;
            while (true)
            {
                yield return new WaitForEndOfFrame();
                timer += Time.deltaTime;
                if (timer >= 0.25f)
                {
                    timer = 0.0f;
                    if (!frame1Or2)
                        spriteRenderer.sprite = activeFrame1;
                    else
                        spriteRenderer.sprite = activeFrame2;

                    frame1Or2 = !frame1Or2;
                }
            }
        }
        //orientation specific animation, although this isn't really animation
        IEnumerator OrientationDependantAnimation()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                var direction = thisEnemy.FindClosestPlayer().position - transform.position;
                direction = direction.normalized;

                if (direction.x > 0.0f)
                    spriteRenderer.sprite = activeFrame1;
                else
                    spriteRenderer.sprite = activeFrame2;
            }
        }

        switch (animationType)
        {
            case AnimationType.FlipFlop:
                StartCoroutine(FlipFlopAnimation());
                break;
            case AnimationType.OrientationDependant:
                StartCoroutine(OrientationDependantAnimation());
                break;
        }
    }

}
