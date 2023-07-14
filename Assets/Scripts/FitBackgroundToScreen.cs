using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitBackgroundToScreen : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        FitImageToScreen();
    }

    private void FitImageToScreen()
    {
        transform.localScale = new(1, 1, 1);

        var width = spriteRenderer.sprite.bounds.size.x;
        var height = spriteRenderer.sprite.bounds.size.y;

        var worldScreenHeight = Camera.main.orthographicSize * 2.0F;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        transform.position = Vector2.zero;
        transform.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height, 1);

    }
  }
