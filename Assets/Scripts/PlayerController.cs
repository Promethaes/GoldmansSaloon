﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        [HideInInspector] public int currentActiveGun = 0;
        [HideInInspector] public int currentAmmoUI = 0;
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

    [Header("Control Values")]
    public float movementSpeed = 1.0f;
    [Range(0.1f, 1.0f)]
    public float gamepadDeadzone = 0.15f;
    [Range(0.1f, 1.0f)]
    public float movementFalloffRate = 1.0f;
    public float timeScaleChange = 0.5f;
    public float tableKnockbackScalar = 3.0f;
    public int hp = 3;

    [Header("References")]
    public Rigidbody2D rigidbody2D;
    public PlayerUiInfo p1UiInfo;
    public PlayerUiInfo p2UiInfo;
    public SpriteRenderer spriteRenderer;
    [Tooltip("0. Forward1\n1. Forward2\n2. Left1\n3. Left2\n4. Right1\n5. Right2")]
    public List<Sprite> p1Sprites;
    [Tooltip("0. Forward1\n1. Forward2\n2. Left1\n3. Left2\n4. Right1\n5. Right2")]
    public List<Sprite> p2Sprites;

    //because the images are not on a sprite sheet, we have to do it like this
    List<Sprite> _currentPlayerSprites = new List<Sprite>();
    Sprite _activeFrame1 = null;
    Sprite _activeFrame2 = null;

    Vector2 _movementVec = new Vector2();

    bool _kicking = false;
    SpriteOrientation spriteOrientation = SpriteOrientation.Forward;

    static int numPlayers = 0;
    int playerNumber = 0;
    private void OnEnable()
    {
        numPlayers++;
        playerNumber = numPlayers;

        if (playerNumber == 1)
        {
            _currentPlayerSprites = p1Sprites;
            p1UiInfo.uiObject.SetActive(true);
        }
        else if (playerNumber == 2)
        {
            _currentPlayerSprites = p2Sprites;
            p2UiInfo.uiObject.SetActive(true);
        }

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
    }

    private void OnDisable()
    {
        numPlayers--;
    }

    private void Update()
    {
        CheckSpriteOrientation();
    }

    //potentially use animation curve? prolly not
    private void FixedUpdate()
    {
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
        }
        else if (spriteOrientation == SpriteOrientation.Left)
        {
            _activeFrame1 = _currentPlayerSprites[(int)Animations.Left1];
            _activeFrame2 = _currentPlayerSprites[(int)Animations.Left2];
        }
        else if (spriteOrientation == SpriteOrientation.Right)
        {
            _activeFrame1 = _currentPlayerSprites[(int)Animations.Right1];
            _activeFrame2 = _currentPlayerSprites[(int)Animations.Right2];
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
        if (context.performed)
            Time.timeScale = timeScaleChange;
        else if (context.canceled)
            Time.timeScale = 1.0f;
    }
    public void OnKick(CallbackContext context)
    {
        if (context.performed)
            _kicking = true;
        else if (context.canceled)
            _kicking = false;
    }
    public void OnLook(CallbackContext context){
        float x = context.ReadValue<Vector2>().x;

        if(x < -gamepadDeadzone)
            spriteOrientation = SpriteOrientation.Left;
        else if (x > gamepadDeadzone)
            spriteOrientation = SpriteOrientation.Right;
        else
            spriteOrientation = SpriteOrientation.Forward;
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

            Debug.Log("kicking");
            if (_kicking)
                otherRigid.AddForce(force * tableKnockbackScalar * otherRigid.mass, ForceMode2D.Impulse);
            else
                otherRigid.velocity = Vector2.zero;
            return;
        }
    }
}
