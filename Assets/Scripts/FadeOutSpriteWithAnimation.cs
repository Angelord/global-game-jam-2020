using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FadeOutSpriteWithAnimation : MonoBehaviour {

    private SpriteRenderer _renderer;
    private Animator _animator;
    private float _initialAlpha;
    
    private void Start() {

        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _initialAlpha = _renderer.color.a;
        
        Invoke("Disable", this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }

    private void Update() {

        Color col = _renderer.color;

        col.a = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime * _initialAlpha;
    }
    
    private void Disable() {
        Color col = _renderer.color;
        col.a = 0.0f;
        _renderer.color = col;

        this.enabled = false;
    }
}