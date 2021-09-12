using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum GunOrientation
    {
        Left,
        Forward,
        Right,
    }
    public GameObject bulletPrefab;
    [Header("Spawn locations for different orientations")]
    public List<GameObject> spawnLocationsLeft;
    public List<GameObject> spawnLocationsForward;
    public List<GameObject> spawnLocationsRight;
    [Tooltip("Set to -1 for infinite ammo.")]
    public int maxAmmo = -1;
    public float rateOfFire = 0.5f;
    public int bulletPoolSize = 8;
    public AudioSource shootSound;
    int _currentAmmo = -1;
    List<BulletInfo> _bullets = new List<BulletInfo>();
    int _bulletKey = 0;
    float _rateOfFireTimer = 0.0f;
    List<GameObject> _currentSpawnLocations = null;

    // Start is called before the first frame update
    void Start()
    {
        _currentAmmo = maxAmmo;
        bulletPrefab.SetActive(false);
        var parent = GameObject.Find("Bullets").transform;
        for (int i = 0; i < bulletPoolSize; i++)
            _bullets.Add(GameObject.Instantiate(bulletPrefab, parent).GetComponent<BulletInfo>());

        SetGunOrientation(GunOrientation.Forward);
    }

    private void Update()
    {
        _rateOfFireTimer -= Time.deltaTime;
    }

    public void SetGunOrientation(GunOrientation orientation)
    {
        switch (orientation)
        {
            case GunOrientation.Left:
                _currentSpawnLocations = spawnLocationsLeft;
                break;
            case GunOrientation.Forward:
                _currentSpawnLocations = spawnLocationsForward;
                break;
            case GunOrientation.Right:
                _currentSpawnLocations = spawnLocationsRight;
                break;
        }
    }

    public void Shoot()
    {
        if (_currentAmmo == 0 || _rateOfFireTimer > 0.0f)
            return;
        foreach (var s in _currentSpawnLocations)
        {
            _bullets[_bulletKey].gameObject.transform.position = s.transform.position;

            var bRigid = _bullets[_bulletKey].gameObject.GetComponent<Rigidbody2D>();
            bRigid.velocity = Vector2.zero;
            _bullets[_bulletKey].gameObject.SetActive(true);

            var dir = _bullets[_bulletKey].transform.position - gameObject.transform.position;
            dir = dir.normalized;
            bRigid.AddForce(dir * _bullets[_bulletKey].bulletSpeed, ForceMode2D.Impulse);

            _bulletKey = (_bulletKey + 1) % _bullets.Count;
            _currentAmmo--;
        }
        _rateOfFireTimer = rateOfFire;
        shootSound.PlayOneShot(shootSound.clip);
    }

    public bool CanShoot()
    {
        return _rateOfFireTimer < rateOfFire;
    }

}
