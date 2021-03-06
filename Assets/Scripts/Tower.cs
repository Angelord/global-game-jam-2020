﻿using System;
using UnityEngine;

[RequireComponent(typeof(RangedAttacker))]
public class Tower : Building {

	public Transform FlipSprite;
	private RangedAttacker _attacker;
	private Animator _animator;
    private AudioManager _audioManager;
    public float HealthBarOffsetBroken;
    public float HealthBarOffsetWorking;
    private static readonly int AnimBoolBroken = Animator.StringToHash("Broken");

    public override AudioData AudioData => Attacker.AudioData;

    public RangedAttacker Attacker {
		get {
			if (_attacker == null) _attacker = GetComponent<RangedAttacker>();
			return _attacker;
		}
	}

	public Animator Animator {
		get {
			if (_animator == null) _animator = GetComponent<Animator>();
			return _animator;
		}
	}

	protected override void OnRepair() {
		base.OnRepair();
		Attacker.enabled = true;
		Animator.SetBool(AnimBoolBroken, false);
		FlipSprite.GetComponent<SpriteRenderer>().material = Faction.UnitMat;
		BarYOffset = HealthBarOffsetWorking;
	}

	protected override void OnBreak() {
		Attacker.enabled = false;
		Animator.SetBool(AnimBoolBroken, true);
		FlipSprite.GetComponent<SpriteRenderer>().material = Faction.UnitMat;
		BarYOffset = HealthBarOffsetBroken;
	}

	protected override void PreStart() {
		base.PreStart();
		_audioManager = FindObjectOfType<AudioManager>();
	}
	
    private void Update() {
		
		if(Broken) return;
	
		if(_attacker.CurrentTarget == null) return;

		float diff = transform.position.x - _attacker.CurrentTarget.transform.position.x;

		FlipSprite.localScale = diff > 0.0f ? new Vector3(1.0f, 1.0f, 1.0f) : new Vector3(-1.0f, 1.0f, 1.0f);

        if(_audioManager != null) {
            _audioManager.Play("tower_attack");
        }

	}
}