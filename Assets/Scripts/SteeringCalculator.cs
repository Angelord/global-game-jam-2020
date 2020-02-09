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

	private float ObstacleAvoidanceRange { get { return _footColliderRadius + _stats.ObstacleAvoidanceRange; } }

	public SteeringCalculator(SteeringStats stats, Senses senses, float footColliderRadius, Predicate<ScrapBehaviour> objectAffectsSeparation, Predicate<ScrapBehaviour> objectAffectsCohesion) {
		_stats = stats;
		_senses = senses;
		_footColliderRadius = footColliderRadius;
		_objectAffectsSeparation = objectAffectsSeparation;
		_objectAffectsCohesion = objectAffectsCohesion;
	}

	public Vector2 Calculate() {

		CalculateSteeringForce();
		
		if(!ObstacleAvoidanceOn) {
			return _accumForce;
		}

		Vector2 obstacleAvoidance = ObstacleAvoidance(_accumForce);

		if ((_accumForce + obstacleAvoidance).magnitude > MovementSpeed) {
			_accumForce = _accumForce.normalized * Mathf.Clamp(MovementSpeed - obstacleAvoidance.magnitude, 0.0f, MovementSpeed);
		}
		
		return _accumForce + obstacleAvoidance;
	}

	private void CalculateSteeringForce() {
		_accumForce = Vector2.zero;

		if (AttackOn && !AccumulateForce(Attack())) {
			return;
		}
		
		if (SeparationOn && !AccumulateForce(Separation())) {
			return;
		}

		if(FollowOn && !AccumulateForce(Follow())) {
			return;
		}

		if (CohesionOn && !AccumulateForce(Cohesion())) {
			return;
		}
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
	
	private Vector2 ObstacleAvoidance(Vector2 desiredMovement) {
		
		int layerMask = _stats.ObstacleLayers;

		RaycastHit2D hit = Physics2D.Raycast(Position, desiredMovement.normalized, ObstacleAvoidanceRange, layerMask);
            
		if (!hit) { return Vector2.zero; }
		
		float penetrationDepth = Mathf.Clamp(1.3f - hit.distance / ObstacleAvoidanceRange, 0.0f, 1.0f);

		Vector2 avoidanceDirLeft = Vector2.Perpendicular(desiredMovement);
		Vector2 avoidanceDirRight = -avoidanceDirLeft;

		Vector2 avoidanceDir = Vector2.zero;

		if (Vector2.Distance(Position + avoidanceDirLeft, Owner.transform.position) < Vector2.Distance(Position + avoidanceDirRight, Owner.transform.position) ) {
			avoidanceDir = avoidanceDirLeft;
		}
		else {
			avoidanceDir = avoidanceDirRight;
		}

		return _stats.ObstacleAvoidance * penetrationDepth * MovementSpeed * avoidanceDir;
	}

	private Vector2 Follow() {
		float minDistance = Owner.FootCollider.radius + _footColliderRadius + _stats.MinFillowDistance;
		float multiplierBonus = _stats.Follow + (Owner.Recalling ? _stats.RecallBonus : 0.0f);
		return Arrive(Owner.transform.position, minDistance, _stats.FollowDecceleration) * multiplierBonus;
	}

	private Vector2 Attack() {

		return Arrive(AttackTarget.transform.position, AttackRange * 0.85f, _stats.AttackDecceleration) * _stats.Attack;
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

			return toTarget.normalized * speed;
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

		return Seek(centerOfMass) * _stats.Cohesion;
	}

	private Vector2 Separation() {
		
		Vector2 steeringForce = Vector2.zero;

		foreach (var neighbour in _senses.ObjectsInRange) {

			if (!_objectAffectsSeparation(neighbour)) { continue; }

			Vector2 fromNeighbour = (Vector3)Position - neighbour.transform.position;

			steeringForce += fromNeighbour.normalized / fromNeighbour.magnitude;
		}

		if (steeringForce.magnitude > 1.0f) {
			steeringForce.Normalize();
		}

		return _stats.Separation * MovementSpeed * steeringForce;
	}
}