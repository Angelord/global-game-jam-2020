using System;
using UnityEngine;

[RequireComponent(typeof(Construct))]
public abstract class Attacker : MonoBehaviour {
	
	public float AttackRate;
	public float Damage;
	public float Range;
	
	private Construct _construct;
	private Senses _senses;
	private ScrapBehaviour _currentTarget;
	private float _lastAttack;

	public ScrapBehaviour CurrentTarget => _currentTarget;

	public bool TargetIsInRange() {
		if (_currentTarget == null) return false;
		return Vector2.Distance(_currentTarget.transform.position, transform.position) <= Range;
	}

	private void Start() {
		_construct = GetComponent<Construct>();
		_senses = GetComponentInChildren<Senses>();
		_senses.OnObjectEnter += OnObjectEnter;
		_senses.OnObjectExit += OnObjectExit;
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

		if (!TargetIsInRange()) {

			_currentTarget = _senses.GetAttackTarget(_construct.Faction);
				
			return;
		}

		if (Time.time - _lastAttack >= AttackRate) {
			Attack(_currentTarget);
			
			// TODO : Play attack animation.
			
			_lastAttack = Time.time;
		}
	}

	protected abstract void Attack(ScrapBehaviour target);

	private void OnDrawGizmosSelected() {
		
		Gizmos.color = Color.red;
		
		Gizmos.DrawWireSphere(transform.position, Range);
	}
}