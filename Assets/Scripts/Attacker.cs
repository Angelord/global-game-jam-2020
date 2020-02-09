using System;
using UnityEngine;

[RequireComponent(typeof(Construct))]
public abstract class Attacker : MonoBehaviour {

	private static readonly int AnimBoolAttacking = Animator.StringToHash("Attacking");

	public float AttackRate;
	public float Damage;
	public float Range;
	public bool HasAttackAnimation;
	public bool AttackOnAnimationTrigger = false;
	public AudioData AudioDataObject;

    private Construct _construct;
	private Senses _senses;
	private ScrapBehaviour _currentTarget;
	private float _lastAttack;
	private Animator _animator;
    private AudioManager _audioManager;

    public ScrapBehaviour CurrentTarget => _currentTarget;

	public bool PlayingAttackAnim => _animator && _animator.GetBool(AnimBoolAttacking);

	public Construct Construct { get => _construct; }

	private void OnValidate() {
		HasAttackAnimation = HasAttackAnimation || AttackOnAnimationTrigger;
	}

	public bool TargetIsInRange() {
		if (_currentTarget == null) return false;
		return ObjectIsInRange(_currentTarget);
	}
	
	private bool TargetIsValid() {
		return _currentTarget != null && _currentTarget.Attackable && _construct.Faction.IsEnemy(_currentTarget.Faction);
	}

	public bool ObjectIsInRange(ScrapBehaviour obj) {
		return Vector2.Distance(obj.transform.position, transform.position) <= Range;
	}

	public void CancelAttack() {
		_animator.SetBool(AnimBoolAttacking, false);
	}

	private void Start() {
        _audioManager = FindObjectOfType<AudioManager>();
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
			CancelAttack();
			_currentTarget = _senses.GetAttackTarget(_construct.Faction);
		}
	}

	private void OnDisable() {
		if (_animator != null) {
			CancelAttack();
		}

		_currentTarget = null;
	}

    private void ResetTarget() {
		ScrapBehaviour newTarget = _senses.GetAttackTarget(_construct.Faction);

		if (newTarget != null && newTarget != _currentTarget) {
			if(PlayingAttackAnim || TargetIsInRange()) return;
			
			_currentTarget = newTarget;
		}
    }

	private void Update() {

		if (_currentTarget == null) { return; }

		if (!TargetIsValid()) {
			
			_currentTarget = _senses.GetAttackTarget(_construct.Faction);
			
			return;
		}

		if (!PlayingAttackAnim && Time.time - _lastAttack >= AttackRate && TargetIsInRange()) {
			_lastAttack = Time.time;
			
			if (!AttackOnAnimationTrigger) {
				Attack(_currentTarget);
			}

			if (HasAttackAnimation) {
				_animator.SetBool(AnimBoolAttacking, true);
				PlayAttackSound();
			}
		}
	}

	public void OnAttackAnimationClimax() {
		if (!TargetIsValid()) { return; }

		Attack(CurrentTarget);
	}

	public void OnAttackAnimationOver() {
		_animator.SetBool(AnimBoolAttacking, false);
	}
	
	private void PlayAttackSound() {
        
		if (AudioDataObject == null) { return; }
		
		switch(AudioDataObject.Race) {
			case CreatureType.Gunner:
				_audioManager.Play("gunner_attack");
				break;
			case CreatureType.Hentairoi:
				_audioManager.Play("hentairoi_attack");
				break;
			case CreatureType.Pistario:
				_audioManager.Play("pistario_attack");
				break;
			case CreatureType.Rakabat:
				_audioManager.Play("rakabat_attack");
				break;
			case CreatureType.Shlurker:
				_audioManager.Play("shlurker_attack");
				break;
			case CreatureType.Zaratiusha:
				_audioManager.Play("zaratiusha_attack");
				break;
			default:
				return;
		}
	}

	protected abstract void Attack(ScrapBehaviour target);
	
	private void OnDrawGizmosSelected() {

		Gizmos.color = Color.red;

		Gizmos.DrawWireSphere(transform.position, Range);
	}
}