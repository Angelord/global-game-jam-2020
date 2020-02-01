using System;
using System.Collections.Generic;
using Claw;
using UnityEditorInternal;
using UnityEngine;

public enum UnitState {
	Following,
	Attacking
}

[RequireComponent(typeof(Attacker))]
public class Creature : Construct {

	public SteeringStats _steering;
	
	public GameObject _scrapEffect;

	public float MovementSpeed = 1.0f;
	
	[SerializeField] private UnitState _state = UnitState.Following;
	
	private SpriteRenderer _sprite;
	
	private Rigidbody2D _rigidbody;

	private CircleCollider2D _footCollider;
	
	private Attacker _attacker;

	private Senses _senses;

	private Vector2 _accumForce;

	private Animator _animator;

	public override bool Salvageable => Broken;

	public override bool Repairable => Broken;

	public override bool Attackable => !Broken;

	public Attacker Attacker {
		get {
			if (_attacker == null) {
				_attacker = GetComponent<Attacker>();
			}

			return _attacker;
		}
	}

	public void Recall() {
		_state = UnitState.Following;
	}

	protected override void OnStart() {
		_attacker = GetComponentInChildren<Attacker>();
		_rigidbody = GetComponent<Rigidbody2D>();
		_footCollider = GetComponent<CircleCollider2D>();
		_sprite = GetComponentInChildren<SpriteRenderer>();
		_senses = GetComponentInChildren<Senses>();
		_animator = GetComponent<Animator>();
	}

	private void Update() {
		if(Broken) return;
		
		if (_state == UnitState.Following) {
			if (_attacker.CurrentTarget == null) { return; }

			if (Owner.Recalling) { return; }

			if (!_attacker.TargetIsInRange()) {

				_state = UnitState.Attacking;
			}
		}
	}

	private void FixedUpdate() {
		if(Owner == null) return;
		if(Broken) return;

		if (Attacker.Attacking) {
			_rigidbody.velocity = Vector2.zero;
			return;
		}

		_accumForce = Vector2.zero;

		CalculateSteeringForce();

		Move(_accumForce);
	}

	private void CalculateSteeringForce() {
		if (!AccumulateForce(Separation())) {
			return;
		}
		
		if ((_state == UnitState.Attacking && !AccumulateForce(Attack())) ||
			(_state == UnitState.Following && !AccumulateForce(Follow()))) {
			return;
		}

		AccumulateForce(Cohesion());
	}

	private bool AccumulateForce(Vector2 toAdd) {
        
        float magCurrent = _accumForce.magnitude;

        float magRemaining = MovementSpeed - magCurrent;
        if (magRemaining <= 0.0f) { return false; }

        float magToAdd = toAdd.magnitude;
        if (magToAdd > magRemaining) {
	        _accumForce += toAdd.normalized * magRemaining;
            return false;
        }
     
        _accumForce += toAdd;
        return true;
    }

	private Vector2 Follow() {
		float minDistance = Owner.FootCollider.radius + _footCollider.radius + _steering.MinFillowDistance;
		return Arrive(Owner.transform.position, minDistance, _steering.FollowDecceleration) * _steering.Follow;
	}

	private Vector2 Attack() {
		if (_attacker.CurrentTarget == null) {
			_state = UnitState.Following;
			return Follow();
		}

		if (Attacker.TargetIsInRange()) {
			return Vector2.zero;
		}

		float minDistance = Attacker.Range;
		return Arrive(Attacker.CurrentTarget.transform.position, minDistance, _steering.FollowDecceleration) * _steering.Follow;
	}

	private void Move(Vector2 direction) {
		
		if (direction.sqrMagnitude > MovementSpeed * 0.1f) {
			_sprite.flipX = _rigidbody.velocity.x < 0.0f;
			_animator.SetBool("Moving", true);
		}
		else {
			_animator.SetBool("Moving", false);
		}
		
		_rigidbody.AddForce(direction - _rigidbody.velocity, ForceMode2D.Force);
	}

	protected override void OnOwnerCommand(PlayerCommand command) {
		command.Execute(this);
	}

	protected override void OnBreak() {
		_scrapEffect.SetActive(true);
		Attacker.enabled = false;
		_rigidbody.velocity = Vector2.zero;
	}

	protected override void OnRepair() {
		_scrapEffect.SetActive(false);
		Attacker.enabled = true;
	}

	private Vector2 Seek(Vector2 target) {
		
		Vector2 desiredVel = (target - (Vector2)transform.position).normalized * MovementSpeed;

		return desiredVel;
	}

	private Vector2 Arrive(Vector2 target, float minDistance, float decceleration) {
		
		Vector2 toTarget = target - (Vector2)transform.position;

		float distance = toTarget.magnitude;

		if (distance > 0.0f) {

			float speed = (distance - minDistance) / decceleration;

			if (speed <= 0.0f) {
				return Vector2.zero;
			}

			speed = Mathf.Min(speed, MovementSpeed);

			Vector2 desiredVel = (toTarget / distance) * speed;

			return desiredVel;
		}

		return Vector2.zero;
	}

	private Vector2 Cohesion() {
		Vector2 centerOfMass = Vector2.zero;

		int count = 0;
		foreach (var neigh in _senses.ObjectsInRange) {
			if(!ObjectAffectsCohesion(neigh)) continue;
			count++;
			centerOfMass += (Vector2)neigh.transform.position;
		}

		if (count == 0) {
			return Vector2.zero;
		}

		centerOfMass /= count;

		return Seek(centerOfMass);
	}

	private Vector2 Separation() {
		
		Vector2 steeringForce = Vector2.zero;

		foreach (var neighbour in _senses.ObjectsInRange) {

			if (!ObjectAffectsSeparation(neighbour)) { continue; }

			Vector2 fromNeighbour = transform.position - neighbour.transform.position;
			steeringForce += fromNeighbour.normalized / fromNeighbour.magnitude;
		}
		
		return steeringForce * _steering.Separation;
	}

	private bool ObjectAffectsSeparation(ScrapBehaviour behaviour) {
		return !Owner.Faction.IsEnemy(behaviour.Faction);
	}

	private bool ObjectAffectsCohesion(ScrapBehaviour behaviour) {
		return behaviour != Owner && !Owner.Faction.IsEnemy(behaviour.Faction);
	}
}