using System;
using UnityEngine;

[RequireComponent(typeof(Construct))]
public class RangedAttacker : MonoBehaviour {

	public float AttackRate;
	public float Damage;
	public GameObject Projectile;

	private Construct _construct;
	private Senses _senses;
	private ScrapBehaviour _currentTarget;
	private float _lastAttack;
	
	private void Start() {
		_construct = GetComponent<Construct>();
		_senses = GetComponentInChildren<Senses>();
		_senses.OnObjectEnter += OnObjectEnter;
		_senses.OnObjectExit += OnObjectExit;
	}

	private void OnDestroy() {
		_senses.OnObjectEnter -= OnObjectEnter;
		_senses.OnObjectExit -= OnObjectExit;
	}

	private void OnObjectEnter(ScrapBehaviour obj) {
		if (_currentTarget == null) {
			_currentTarget = _senses.GetAttackTarget(_construct.Faction);
		}
	}

	private void OnObjectExit(ScrapBehaviour obj) {
		if (_currentTarget == obj) {
			_currentTarget = _senses.GetAttackTarget(_construct.Faction);
		}
	}

	private void Update() {
		if (_currentTarget == null) { return; }
		
		if (Time.time - _lastAttack >= AttackRate) {
			FireProjectile();
			_lastAttack = Time.time;
		}
	}

	private void FireProjectile() {
		Projectile proj = GameObject.Instantiate(Projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
		
		proj.Initialize(_currentTarget.transform, () => {
			if (_currentTarget == null || _currentTarget.Attackable) { return; }
			_currentTarget.TakeDamage(Damage);
		});
	}
}