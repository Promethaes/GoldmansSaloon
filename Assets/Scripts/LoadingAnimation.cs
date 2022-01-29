using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnimation : UIAnimation
{
    [SerializeField] Vector3 desiredScale = Vector3.one;
    [SerializeField] AnimationCurve lerpCurve = null;

    Vector3 _originalScale = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        _originalScale = transform.localScale;
        IEnumerator Lerp()
        {
            float x = 0.0f;
            while (x < 1.0f)
            {
                yield return new WaitForEndOfFrame();
                x += Time.deltaTime;
                transform.localScale = Vector3.Lerp(_originalScale, desiredScale, lerpCurve.Evaluate(x));
            }
        }
        StartCoroutine(Lerp());
    }
}
