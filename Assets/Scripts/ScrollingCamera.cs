using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingCamera : MonoBehaviour
{
    [Tooltip("The curve that will dictate how the camera transitions from moving up to moving down.")]
    [SerializeField] AnimationCurve _transitionCurve = null;
    [Tooltip("Positive number will make the camera scroll upwards, negative will make it scroll downards.")]
    public float _scrollSpeed = 5.2f;
    [Tooltip("How fast the background scrolls.")]
    public float _backgroundScrollSpeed = 0.5f;
    [SerializeField] float _tranistionSpeed = 0.5f;

    [Header("References")]
    [SerializeField] GameObject _background;
    Renderer _bgRenderer = null;

    //Debug
    [SerializeField] bool debug_invokeTransition = false;

    bool _shouldScroll = true;

    //implement game manager
    //subscribe to OnGoldmanSpawn event
    //tell the camera to transition to moving downward

    private void Start()
    {
        _bgRenderer = _background.GetComponent<Renderer>();
    }

    //subrscribe to players dead event
    //set should scroll to false

    // Update is called once per frame
    void Update()
    {
        if (!_shouldScroll)
            return;
        if (debug_invokeTransition)
        {
            debug_invokeTransition = false;
            OnGoldmanSpawn();
        }
        transform.position = transform.position + new Vector3(0.0f, _scrollSpeed * Time.deltaTime, 0.0f);
        _bgRenderer.material.mainTextureOffset = new Vector2(0.0f, Time.time * _backgroundScrollSpeed);
    }

    //flip camera direction
    public void OnGoldmanSpawn()
    {
        IEnumerator Transition()
        {
            float x = 0.0f;
            float originalSpeed = _scrollSpeed;
            float originalBGSpeed = _backgroundScrollSpeed;
            while (x < 1.0f)
            {
                yield return new WaitForEndOfFrame();
                x += Time.deltaTime * _tranistionSpeed;
                _scrollSpeed = Mathf.Lerp(originalSpeed, -originalSpeed, _transitionCurve.Evaluate(x));
                _backgroundScrollSpeed = -originalBGSpeed;
            }

        }
        StartCoroutine(Transition());
    }

}
