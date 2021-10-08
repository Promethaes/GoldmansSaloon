using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityAnimator : MonoBehaviour
{
    [SerializeField] protected float timeBetweenFrames = 0.25f;
    [Header("References")]
    [SerializeField] SpriteRenderer spriteRenderer = null;
    [SerializeField] EntityOrientation orientation = null;
    [SerializeField] Sprite leftOne = null;
    [SerializeField] Sprite leftTwo = null;
    [SerializeField] Sprite forwardOne = null;
    [SerializeField] Sprite forwardTwo = null;
    [SerializeField] Sprite rightOne = null;
    [SerializeField] Sprite rightTwo = null;

    protected Sprite activeFrameOne = null;
    protected Sprite activeFrameTwo = null;

    private void Start()
    {
        orientation.OnDetermineOrientation.AddListener(SwitchActiveFrameSet);
    }

    private void OnEnable()
    {
        SpriteSetup();
        StartCoroutine(Animate());
    }

    protected virtual void SpriteSetup()
    {
        activeFrameOne = forwardOne;
        activeFrameTwo = forwardTwo;
    }

    protected virtual IEnumerator Animate()
    {
        bool frameOneOrTwo = false;
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenFrames);
            spriteRenderer.sprite = (frameOneOrTwo ? activeFrameTwo : activeFrameOne);
            frameOneOrTwo = !frameOneOrTwo;
        }
    }

    public void SwitchActiveFrameSet()
    {
        switch (orientation.orientation)
        {
            case EntityOrientation.Orientation.Left:
                activeFrameOne = leftOne;
                activeFrameTwo = leftTwo;
                break;
            case EntityOrientation.Orientation.Forward:
                activeFrameOne = forwardOne;
                activeFrameTwo = forwardTwo;
                break;
            case EntityOrientation.Orientation.Right:
                activeFrameOne = rightOne;
                activeFrameTwo = rightTwo;
                break;
        }
    }
}
