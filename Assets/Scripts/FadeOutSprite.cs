using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FadeOutSprite : MonoBehaviour {

    public float Duration = 0.5f;
    private float _timeRemaining;
    private SpriteRenderer _renderer;
    private float _startAlpha;
    
    private void Start() {
        _timeRemaining = Duration;
        _renderer = GetComponent<SpriteRenderer>();
        _startAlpha = _renderer.color.a;
    }

    private void Update() {

        _timeRemaining -= Time.deltaTime;

        Color col = _renderer.color;
        
        if (_timeRemaining <= 0.0f) {
            col.a = 0.0f;
            this.enabled = false;
        }
        else {
            col.a = _timeRemaining / Duration * _startAlpha;
        }

        _renderer.color = col;
    }
}