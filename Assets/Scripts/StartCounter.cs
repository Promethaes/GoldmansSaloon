using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCounter : MonoBehaviour
{
    [SerializeField] float lerpSpeed = 1.0f;
    [SerializeField] AnimationCurve lerpCurve = null;
    [SerializeField] Vector3 lerpScale = new Vector3(0.5f, 0.5f, 0.5f);
    [Header("References")]
    [SerializeField] TMPro.TextMeshProUGUI text = null;
    Vector3 _originalScale = Vector2.one;
    // Start is called before the first frame update
    void Start()
    {
        IEnumerator Counter()
        {
            StartCoroutine(Lerp());
            yield return new WaitForSeconds(1.0f);
            
            StartCoroutine(Lerp());
            text.text = "2";
            yield return new WaitForSeconds(1.0f);
            
            StartCoroutine(Lerp());
            text.text = "1";
            yield return new WaitForSeconds(1.0f);
            
            StartCoroutine(Lerp());
            text.text = "GO!!";
            yield return new WaitForSeconds(1.0f);
        }
        IEnumerator Lerp()
        {
            float x = 0.0f;
            while (x < 1.0f)
            {
                yield return new WaitForEndOfFrame();
                x += Time.deltaTime * lerpSpeed;
                transform.localScale = Vector3.Lerp(lerpScale, _originalScale, lerpCurve.Evaluate(x));
            }
        }
        StartCoroutine(Counter());
    }
}
