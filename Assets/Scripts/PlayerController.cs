using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class PlayerUiInfo
    {
        public GameObject uiObject;
        public List<Image> hearts;
        public List<Image> gunSprites;
        public TMPro.TextMeshProUGUI ammoText;
        //insert gun enum stuff
        [HideInInspector] public GunEnum currentActiveGun = 0;
        [HideInInspector] public int currentAmmoUI = 0;

        public void OnHPChange(int currentHP, Sprite fullHeart, Sprite brokenHeart)
        {
            switch (currentHP)
            {
                case 3:
                    hearts[0].sprite = fullHeart;
                    hearts[1].sprite = fullHeart;
                    hearts[2].sprite = fullHeart;
                    break;
                case 2:
                    hearts[0].sprite = brokenHeart;
                    hearts[1].sprite = fullHeart;
                    hearts[2].sprite = fullHeart;
                    break;
                case 1:
                    hearts[0].sprite = brokenHeart;
                    hearts[1].sprite = brokenHeart;
                    hearts[2].sprite = fullHeart;
                    break;
                case 0:
                    hearts[0].sprite = brokenHeart;
                    hearts[1].sprite = brokenHeart;
                    hearts[2].sprite = brokenHeart;
                    break;
            }
        }
    }

    enum Animations
    {
        Forward1,
        Forward2,
        Left1,
        Left2,
        Right1,
        Right2,
    }

    //for animations
    enum SpriteOrientation
    {
        Forward,
        Left,
        Right
    }

    public enum GunEnum
    {
        Base,
        Shotgun,
        Gattling,
    }


    [Header("Control Values")]
    public float movementSpeed = 1.0f;
    [Range(0.1f, 1.0f)]
    public float gamepadDeadzone = 0.15f;
    [Range(0.1f, 1.0f)]
    public float movementFalloffRate = 1.0f;
    public float timeScaleChange = 0.5f;
    public float physicsTimeScaleChange = 0.5f;
    public float tableKnockbackScalar = 3.0f;
    public int hp = 3;
    [Tooltip("The speed at which the bullet time slider goes down")]
    public float bulletTimeSliderSpeed = 2.0f;
    [Tooltip("The value that the bullet time slider has to surpass to reset the cooldown.")]
    public float bulletTimeCooldownThreshold = 0.25f;
    [SerializeField] AnimationCurve _bulletTimePitchCurve = null;
    [SerializeField] float _bulletTimePitchCurveSpeed = 5.0f;
    [SerializeField] float _bulletTimeSlowPitch = 80.0f;
    [SerializeField] float _bulletTimeNormalPitch = 100.0f;
    public GunEnum _currentGun = GunEnum.Base;


    [Header("References")]
    public Rigidbody2D rigidbody2D;
    public Slider bulletTimeSlider;
    public PlayerUiInfo p1UiInfo;
    public PlayerUiInfo p2UiInfo;
    public Sprite fullHeart;
    public Sprite brokenHeart;
    public SpriteRenderer spriteRenderer;
    [Tooltip("0. Forward1\n1. Forward2\n2. Left1\n3. Left2\n4. Right1\n5. Right2")]
    public List<Sprite> p1Sprites;
    [Tooltip("0. Forward1\n1. Forward2\n2. Left1\n3. Left2\n4. Right1\n5. Right2")]
    public List<Sprite> p2Sprites;
    [Tooltip("0. Base Gun\n1. Shotgun\n2. Gattling Gun")]
    public List<Gun> guns;
    [SerializeField] AudioMixer _masterMix;

    bool _shooting = false;

    //because the images are not on a sprite sheet, we have to do it like this
    List<Sprite> _currentPlayerSprites = new List<Sprite>();
    Sprite _activeFrame1 = null;
    Sprite _activeFrame2 = null;
    bool _invinceFlickering = false;

    Vector2 _movementVec = new Vector2();

    bool _kicking = false;

    SpriteOrientation spriteOrientation = SpriteOrientation.Forward;

    static bool _bulletTimeCooldown = false;
    static bool _bulletTime = false;
    static bool _bulletTimeLerpingPitch = false;

    static int numPlayers = 0;
    int _playerNumber = 0;

    [HideInInspector] public int currentScore = 0;
    private void OnEnable()
    {
        numPlayers++;
        _playerNumber = numPlayers;

        switch (_playerNumber)
        {
            case 1:
                _currentPlayerSprites = p1Sprites;
                p1UiInfo.uiObject.SetActive(true);
                break;
            case 2:
                _currentPlayerSprites = p2Sprites;
                p2UiInfo.uiObject.SetActive(true);
                break;
        }

        bulletTimeSlider = FindObjectOfType<Slider>();

        CheckSpriteOrientation();
        spriteRenderer.sprite = _activeFrame1;
        //flip a flag every 0.25 seconds, which will then switch the sprite in the sprite renderer
        IEnumerator FlipFlopAnimation()
        {
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
                        spriteRenderer.sprite = _activeFrame1;
                    else
                        spriteRenderer.sprite = _activeFrame2;

                    frame1Or2 = !frame1Or2;
                }
            }
        }
        StartCoroutine(FlipFlopAnimation());

        IEnumerator HandleBulletTimeBar()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                if (hp <= 0)
                    continue;

                _bulletTimeCooldown = bulletTimeSlider.value <= bulletTimeCooldownThreshold;
                if (bulletTimeSlider.value <= 0.0f && _bulletTime)
                {
                    Debug.Log(bulletTimeSlider.value);
                    _bulletTime = false;
                    Time.timeScale = 1.0f;
                    Time.fixedDeltaTime = Time.timeScale * 0.02f;
                }

                if (_playerNumber == 1)
                {
                    if (_bulletTime)
                        bulletTimeSlider.value = bulletTimeSlider.value - Time.deltaTime * bulletTimeSliderSpeed;
                    else
                        bulletTimeSlider.value = bulletTimeSlider.value + Time.deltaTime;
                }
            }
        }
        StartCoroutine(HandleBulletTimeBar());

        IEnumerator BTLerpPitch()
        {
            float x = 0.0f;
            while (true)
            {
                yield return new WaitForEndOfFrame();
                float time = Time.deltaTime * _bulletTimePitchCurveSpeed;
                if (_bulletTime)
                    x += time * bulletTimeSliderSpeed;
                else
                    x -= time / bulletTimeSliderSpeed;
                if (x < 0.0f)
                    x = 0.0f;
                else if (x > 1.0f)
                    x = 1.0f;

                float temp = Mathf.Lerp(_bulletTimeNormalPitch, _bulletTimeSlowPitch, _bulletTimePitchCurve.Evaluate(x));
                _masterMix.SetFloat("Master Pitch", temp / 100.0f);
            }
        }
        StartCoroutine(BTLerpPitch());
    }

    private void OnDisable()
    {
        numPlayers--;
    }

    private void Update()
    {
        if (hp <= 0)
            return;
        if (_shooting)
            guns[(int)_currentGun].Shoot();
    }

    //potentially use animation curve? prolly not
    private void FixedUpdate()
    {
        if (hp <= 0)
        {
            rigidbody2D.velocity = Vector2.zero;
            return;
        }
        var force = _movementVec * movementSpeed;
        rigidbody2D.AddForce(force, ForceMode2D.Impulse);

        var v = rigidbody2D.velocity;
        if (v.magnitude > 0.0f)
            rigidbody2D.velocity = rigidbody2D.velocity - rigidbody2D.velocity * movementFalloffRate;
    }


    void CheckSpriteOrientation()
    {
        if (spriteOrientation == SpriteOrientation.Forward)
        {
            _activeFrame1 = _currentPlayerSprites[(int)Animations.Forward1];
            _activeFrame2 = _currentPlayerSprites[(int)Animations.Forward2];
            guns[(int)_currentGun].SetGunOrientation(Gun.GunOrientation.Forward);
        }
        else if (spriteOrientation == SpriteOrientation.Left)
        {
            _activeFrame1 = _currentPlayerSprites[(int)Animations.Left1];
            _activeFrame2 = _currentPlayerSprites[(int)Animations.Left2];
            guns[(int)_currentGun].SetGunOrientation(Gun.GunOrientation.Left);
        }
        else if (spriteOrientation == SpriteOrientation.Right)
        {
            _activeFrame1 = _currentPlayerSprites[(int)Animations.Right1];
            _activeFrame2 = _currentPlayerSprites[(int)Animations.Right2];
            guns[(int)_currentGun].SetGunOrientation(Gun.GunOrientation.Right);
        }
    }

    public void Revive()
    {
        hp = 3;
        gameObject.transform.Rotate(new Vector3(0.0f, 0.0f, -90.0f));
    }
    public void TakeDamage(int damage)
    {
        if (hp <= 0 || _invinceFlickering)
            return;
        hp -= damage;
        IEnumerator InvincibilityFlicker()
        {
            _invinceFlickering = true;
            var col = spriteRenderer.color;
            for (int i = 0; i < 4; i++)
            {
                col.a = 0.0f;
                spriteRenderer.color = col;
                yield return new WaitForSeconds(0.125f);
                col.a = 1.0f;
                spriteRenderer.color = col;
                yield return new WaitForSeconds(0.125f);
            }
            _invinceFlickering = false;
        }
        StartCoroutine(InvincibilityFlicker());
        switch (_playerNumber)
        {
            case 1:
                p1UiInfo.OnHPChange(hp, fullHeart, brokenHeart);
                break;
            case 2:
                p2UiInfo.OnHPChange(hp, fullHeart, brokenHeart);
                break;
        }
        //TODO: insert UI meddling here
        if (hp <= 0)
        {
            gameObject.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
            //find a way to check if all players are dead
            //then,
            //_bulletTime = false;
            //Time.timeScale = 1.0f;
            //Time.fixedDeltaTime = Time.timeScale * 0.02f;   
        }
    }

    public void AddScore(int score)
    {
        currentScore += score;
    }

    public void SetCurrentGun(GunEnum gun)
    {
        _currentGun = gun;
        switch (_playerNumber)
        {
            case 1:
                if (guns[(int)_currentGun].maxAmmo == -1)
                    p1UiInfo.ammoText.gameObject.SetActive(false);
                else
                    p1UiInfo.ammoText.gameObject.SetActive(true);

                break;
            case 2:
                p2UiInfo.ammoText.gameObject.SetActive(true);
                break;

        }
    }

    //Input Action Events
    public void OnMove(CallbackContext context)
    {
        _movementVec = context.ReadValue<Vector2>();
        if (_movementVec.magnitude < gamepadDeadzone)
            _movementVec = Vector2.zero;
    }
    public void OnBulletTime(CallbackContext context)
    {
        if (hp <= 0)
            return;
        if (context.performed && !_bulletTimeCooldown)
        {
            _bulletTime = true;
            Time.timeScale = timeScaleChange;
            Time.fixedDeltaTime = timeScaleChange * 0.02f;
        }
        else
        {

            _bulletTime = false;
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
    }
    public void OnKick(CallbackContext context)
    {
        if (context.performed)
            _kicking = true;
        else if (context.canceled)
            _kicking = false;
    }
    public void OnLook(CallbackContext context)
    {
        float x = context.ReadValue<Vector2>().x;

        if (x < -gamepadDeadzone)
            spriteOrientation = SpriteOrientation.Left;
        else if (x > gamepadDeadzone)
            spriteOrientation = SpriteOrientation.Right;
        else
            spriteOrientation = SpriteOrientation.Forward;
        CheckSpriteOrientation();
    }

    public void OnFire(CallbackContext context)
    {
        _shooting = context.performed;
    }


    //Collision
    private void OnTriggerEnter2D(Collider2D other)
    {
        //insert bullet stuff here
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Table"))
        {
            var force = other.gameObject.transform.position - gameObject.transform.position;
            force = force.normalized;

            var otherRigid = other.gameObject.GetComponent<Rigidbody2D>();

            if (_kicking)
                otherRigid.AddForce(force * tableKnockbackScalar * otherRigid.mass, ForceMode2D.Impulse);
            else
                otherRigid.velocity = Vector2.zero;
            return;
        }
    }
}
