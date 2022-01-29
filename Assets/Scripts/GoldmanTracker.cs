using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldmanTracker : MonoBehaviour
{
    public float lerpSpeed = 1.0f;
    [SerializeField] AnimationCurve animationCurve = null;
    [SerializeField] Transform lerpPoint = null;

    Vector3 _originalPosition = Vector3.one;
    // Start is called before the first frame update
    void Start()
    {
        _originalPosition = transform.localPosition;
        IEnumerator Lerp()
        {
            while (true)
            {
                yield return null;
                float x = 0.0f;
                while (x < 1.0f)
                {
                    yield return new WaitForEndOfFrame();
                    x += Time.deltaTime * lerpSpeed;
                    transform.localPosition = Vector3.Lerp(_originalPosition, lerpPoint.localPosition, animationCurve.Evaluate(x));
                }
                x = 1.0f;
                while (x > 0.0f)
                {
                    yield return new WaitForEndOfFrame();
                    x -= Time.deltaTime * lerpSpeed;
                    transform.localPosition = Vector3.Lerp(_originalPosition, lerpPoint.localPosition, animationCurve.Evaluate(x));
                }
                x = 0.0f;
            }
        }
        StartCoroutine(Lerp());
    }
}
