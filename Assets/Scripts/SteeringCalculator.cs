using System;
using UnityEngine;

public class SteeringCalculator {

	public ScrapBehaviour AttackTarget;
	public float AttackRange;
	public Vector2 Position;
	public Player Owner;
	public float MovementSpeed;

	public bool ObstacleAvoidanceOn = true;
	public bool SeparationOn = true;
	public bool CohesionOn = true;
	public bool AttackOn = false;
	public bool FollowOn = true;
	
	private readonly SteeringStats _stats;
	private readonly Senses _senses;
	private readonly float _footColliderRadius;
	private readonly Predicate<ScrapBehaviour> _objectAffectsSeparation;
	private readonly Predicate<ScrapBehaviour> _objectAffectsCohesion;
	private Vector2 _accumForce;

	public SteeringCalculator(SteeringStats stats, Senses senses, float footColliderRadius, Predicate<ScrapBehaviour> objectAffectsSeparation, Predicate<ScrapBehaviour> objectAffectsCohesion) {
		_stats = stats;
		_senses = senses;
		_footColliderRadius = footColliderRadius;
		_objectAffectsSeparation = objectAffectsSeparation;
		_objectAffectsCohesion = objectAffectsCohesion;
	}

	public Vector2 CalculateSteeringForce() {
		_accumForce = Vector2.zero;
//		if (ObstacleAvoidanceOn && !AccumulateForce(ObstacleAvoidance(CalculateSteeringForce()))) {
//			return _accumForce;
//		}

		if (SeparationOn && !AccumulateForce(Separation())) {
			return _accumForce;
		}

		if (AttackOn && !AccumulateForce(Attack())) {
			return _accumForce;
		}
		
		if(FollowOn && !AccumulateForce(Follow())) {
			return _accumForce;
		}

		if (CohesionOn && !AccumulateForce(Cohesion())) {
			return _accumForce;
		}

		return _accumForce;
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
	
//	private Vector2 ObstacleAvoidance(Vector2 desiredMovement) {
//		
//		int layerMask = _steering.ObstacleLayers;
//
//		RaycastHit2D hit = Physics2D.Raycast(transform.position, desiredMovement, _steering.ObstacleAvoidanceRange, layerMask);
//            
//		if (!hit) { return Vector2.zero; }
//		
//		Debug.Log("HIT " + hit.collider.gameObject.name);
//            
//		float penetrationDepth = Mathf.Clamp(1.0f - hit.distance / _steering.ObstacleAvoidanceRange, 0.0f, 1.0f) + 0.2f;
//		
//		return _steering.ObstacleAvoidance * penetrationDepth * hit.normal;
//	}

	private Vector2 Follow() {
		float minDistance = Owner.FootCollider.radius + _footColliderRadius + _stats.MinFillowDistance;
		float multiplierBonus = _stats.Follow + (Owner.Recalling ? _stats.RecallBonus : 0.0f);
		return Arrive(Owner.transform.position, minDistance, _stats.FollowDecceleration) * multiplierBonus;
	}

	private Vector2 Attack() {

		float minDistance = AttackRange;
		return Arrive(AttackTarget.transform.position, minDistance, _stats.FollowDecceleration) * _stats.Follow;
	}

	private Vector2 Seek(Vector2 target) {
		
		Vector2 desiredVel = (target - Position).normalized * MovementSpeed;

		return desiredVel;
	}

	private Vector2 Arrive(Vector2 target, float minDistance, float decceleration) {
		
		Vector2 toTarget = target - Position;

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
			if(!_objectAffectsCohesion(neigh)) continue;
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

			if (!_objectAffectsSeparation(neighbour)) { continue; }

			Vector2 fromNeighbour = (Vector3)Position - neighbour.transform.position;
			steeringForce += fromNeighbour.normalized / fromNeighbour.magnitude;
		}
		
		return steeringForce * _stats.Separation;
	}
}