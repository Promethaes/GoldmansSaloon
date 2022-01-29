using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SpriteRenderer spriteRenderer = null;
    [SerializeField] Image image = null;

    List<Sprite> _sprite = new List<Sprite>();
    // Update is called once per frame
    void Update()
    {
        image.sprite = spriteRenderer.sprite;
    }
}
