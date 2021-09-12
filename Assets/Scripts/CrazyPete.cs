using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyPete : MonoBehaviour
{
    [SerializeField] AnimationCurve _lerpCurve;
    [SerializeField] float _lerpSpeed = 1.0f;
    [SerializeField] float numRotationsPerSecond = 1.0f;
    [Header("References")]
    [SerializeField] Gun _crazyGun;
    [SerializeField] Transform _lerpPoint;
    [SerializeField] SpriteRenderer _dynamiteSprite;


    Vector2 _originalPoint = Vector2.zero;
    bool _firing = false;

    private void OnEnable()
    {
        _crazyGun.SetGunOrientation(Gun.GunOrientation.Forward);
        IEnumerator Rotate()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                _dynamiteSprite.transform.Rotate(0.0f, 0.0f, 360.0f * numRotationsPerSecond * Time.deltaTime);
            }
        }
        StartCoroutine(Rotate());
    }
    // Update is called once per frame
    void Update()
    {
        if (_crazyGun.CanShoot() && !_firing)
        {
            IEnumerator Fire()
            {
                _firing = true;
                _dynamiteSprite.enabled = true;
                _originalPoint = _crazyGun.transform.position;
                float x = 0.0f;
                while (x < 1.0f)
                {
                    yield return new WaitForEndOfFrame();
                    x += Time.deltaTime * _lerpSpeed;
                    _crazyGun.transform.position = Vector2.Lerp(_originalPoint, _lerpPoint.position, _lerpCurve.Evaluate(x));
                }
                _dynamiteSprite.enabled = false;
                _crazyGun.Shoot();
                yield return new WaitForSeconds(_crazyGun.rateOfFire);
                _crazyGun.transform.position = gameObject.transform.position;
                _firing = false;
            }
            StartCoroutine(Fire());
        }
    }

    private void OnDisable()
    {
        _firing = false;
        _dynamiteSprite.enabled = false;
        _crazyGun.transform.position = gameObject.transform.position;
    }
}
