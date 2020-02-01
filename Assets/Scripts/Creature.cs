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
	
	private UnitState _state = UnitState.Following;
	
	private SpriteRenderer _sprite;
	
	private Rigidbody2D _rigidbody;

	private CircleCollider2D _footCollider;
	
	private Attacker _attacker;

	private Senses _senses;

	private Vector2 _accumForce;

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
	}
	
	private void FixedUpdate() {
		if(Owner == null) return;
		if(Broken) return;

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
		float minDistance = Owner.FootCollider.radius + _footCollider.radius;
		return Arrive(Owner.transform.position, minDistance, _steering.FollowDistance) * _steering.Follow;
	}

	private Vector2 Attack() {
		if (_attacker.CurrentTarget == null) {
			_state = UnitState.Following;
			return Follow();
		}

		return Seek(_attacker.CurrentTarget.transform.position) * _steering.Follow;
	}

	private void Move(Vector2 direction) {
		
		_rigidbody.AddForce(direction, ForceMode2D.Force);

		if (_rigidbody.velocity.sqrMagnitude > MovementSpeed * 0.2f) {
			_sprite.flipX = _rigidbody.velocity.x > 0.0f;
		}
	}

	private void OnCommand(PlayerCommand command) {
		command.Execute(this);
	}

	protected override void OnBreak() {
		_scrapEffect.SetActive(true);
		Attacker.enabled = false;
	}

	protected override void OnRepair() {
		_scrapEffect.SetActive(false);
		Attacker.enabled = true;
	}

	private Vector2 Seek(Vector2 target) {
		
		Vector2 desiredVel = (target - (Vector2)transform.position).normalized * MovementSpeed;

		return desiredVel - _rigidbody.velocity;
	}

	private Vector2 Arrive(Vector2 target, float minDistance, float decceleration) {
		
		Vector2 toTarget = target - (Vector2)transform.position;

		float distance = toTarget.magnitude;

		if (distance > 0.0f) {

			float speed = Mathf.Clamp(distance - minDistance, 0.0f, distance) / decceleration;

			speed = Mathf.Min(speed, MovementSpeed);

			Vector2 desiredVel = (toTarget / distance) * speed;

			return desiredVel - _rigidbody.velocity;
		}

		return Vector2.zero;
	}

	private Vector2 Cohesion() {
		Vector2 centerOfMass = Vector2.zero;
		Vector2 steeringForce = Vector2.zero;

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

//		Vector2 force = centerOfMass - (Vector2) transform.position;
//		if (force.sqrMagnitude > 1.0f) {
//			force.Normalize();
//		}

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