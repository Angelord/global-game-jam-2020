using System;
using UnityEngine;

[RequireComponent(typeof(Construct))]
public abstract class Attacker : MonoBehaviour {

	private static readonly int AttackTrigger = Animator.StringToHash("Attack");

	public float AttackRate;
	public float Damage;
	public float Range;
	public bool HasAttackAnimation;

	private Construct _construct;
	private Senses _senses;
	private ScrapBehaviour _currentTarget;
	private float _lastAttack;
	private Animator _animator;
	private bool _attacking;

	public ScrapBehaviour CurrentTarget => _currentTarget;

	public bool Attacking => _attacking;

	public bool TargetIsInRange() {
		if (_currentTarget == null) return false;
		return Vector2.Distance(_currentTarget.transform.position, transform.position) <= Range;
	}

	private void Start() {
		_construct = GetComponent<Construct>();
		_senses = GetComponentInChildren<Senses>();
		_senses.OnObjectEnter += OnObjectEnter;
		_senses.OnObjectExit += OnObjectExit;
		_animator = GetComponent<Animator>();

		InvokeRepeating("ResetTarget", UnityEngine.Random.Range(1, 4), 2f);
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

	private void OnDisable() {
		_currentTarget = null;
	}

    private void ResetTarget()
    {
		_currentTarget = _senses.GetAttackTarget(_construct.Faction);
	}

	private void Update() {

		if (_currentTarget == null) { return; }

		if (_currentTarget.Faction == _construct.Faction) {
			_currentTarget = null;
			return;
		}

		if (!_currentTarget.Attackable || !TargetIsInRange()) {

			_currentTarget = _senses.GetAttackTarget(_construct.Faction);

			return;
		}

		if (Time.time - _lastAttack >= AttackRate) {

			Attack(_currentTarget);

			if (HasAttackAnimation) {
				_animator.SetTrigger(AttackTrigger);
				_attacking = true;
			}

			_lastAttack = Time.time;
		}
	}

	public void OnAttackAnimationOver() {
		_attacking = false;
	}

	protected abstract void Attack(ScrapBehaviour target);

	private void OnDrawGizmosSelected() {

		Gizmos.color = Color.red;

		Gizmos.DrawWireSphere(transform.position, Range);
	}
}