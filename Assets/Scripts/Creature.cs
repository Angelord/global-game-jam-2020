using Claw.Chrono;
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
	
	public UnitState _state = UnitState.Following;
	
	private SpriteRenderer _sprite;
	
	private Rigidbody2D _rigidbody;
	
	private Attacker _attacker;

	private Senses _senses;

	private SteeringCalculator _steeringCalculator;

	private Animator _animator;
	
	private static readonly int AnimMoving = Animator.StringToHash("Moving");

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
		Attacker.CancelAttack();
	}

	public void Enrage() {
		// TODO : implement
	}

	protected override void PreStart() {
		_attacker = GetComponentInChildren<Attacker>();
		_rigidbody = GetComponent<Rigidbody2D>();
		_sprite = GetComponentInChildren<SpriteRenderer>();
		_senses = GetComponentInChildren<Senses>();
		_animator = GetComponent<Animator>();

		_sprite.material = Faction.UnitMat;
		
		CircleCollider2D footCollider = GetComponent<CircleCollider2D>();
		
		_steeringCalculator = new SteeringCalculator(_steering, _senses, footCollider.radius, ObjectAffectsSeparation, ObjectAffectsCohesion);
	}

	private void Update() {
		if (Broken) { return; }

		Attacker.enabled = !Owner.Recalling;

		if (_state == UnitState.Attacking) {

			if (_attacker.CurrentTarget != null) { return; }
			
			_state = UnitState.Following;
		}
		else if (_state == UnitState.Following) {
			
			if (_attacker.CurrentTarget == null) { return; }

			if (Owner.Recalling) { return; }

			_state = UnitState.Attacking;
		}
	}

	private void FixedUpdate() {
		if (Broken) {
			_rigidbody.velocity *= 0.5f;
			return;
		}
		
		if (Attacker.PlayingAttackAnim) {
			
			_rigidbody.velocity = Vector2.zero;
			
			if (Attacker.CurrentTarget != null) {
				SetFlip(transform.position.x - Attacker.CurrentTarget.transform.position.x > 0.0f);
			}

			return;
		}

		_steeringCalculator.Position = transform.position;
		_steeringCalculator.AttackRange = Attacker.Range;
		_steeringCalculator.AttackTarget = Attacker.CurrentTarget;
		_steeringCalculator.Owner = Owner;
		_steeringCalculator.MovementSpeed = MovementSpeed;
		_steeringCalculator.AttackOn = _state == UnitState.Attacking;
		_steeringCalculator.FollowOn = _state == UnitState.Following;
		
		Vector2 moveForce = _steeringCalculator.Calculate();

		Move(moveForce);
	}

	private void SetFlip(bool left) {
		Vector3 scale = _sprite.transform.localScale;

		bool isFacingLeft = scale.x < 0.0f;
		if (left != isFacingLeft) {
			scale.x = -scale.x;
		}

		_sprite.transform.localScale = scale;
	}

	private void Move(Vector2 direction) {
		
		if (direction.sqrMagnitude > MovementSpeed * 0.08f) {
			SetFlip(_rigidbody.velocity.x < 0.0f);
			_animator.SetBool(AnimMoving, true);
		}
		else {
			direction = Vector2.zero;
			_animator.SetBool(AnimMoving, false);
		}
		
		_rigidbody.AddForce(direction - _rigidbody.velocity * 0.5f, ForceMode2D.Impulse);
	}

	protected override void OnOwnerCommand(PlayerCommand command) {
		command.Execute(this);
	}

	protected override void OnBreak() {
		base.OnBreak();
		_scrapEffect.SetActive(true);
		Attacker.enabled = false;
		_rigidbody.velocity = Vector2.zero;
		_sprite.material = Faction.Neutral.UnitMat;
	}

	protected override void OnRepair() {
		base.OnRepair();
		
		_scrapEffect.SetActive(false);
		
		CustomCoroutine.WaitOneFrameThenExecute(() => {
			Attacker.enabled = true;
		});
		
		_sprite.material = Faction.UnitMat;
	}
	
	private bool ObjectAffectsSeparation(ScrapBehaviour behaviour) {
		return !Owner.Faction.IsEnemy(behaviour.Faction);
	}

	private bool ObjectAffectsCohesion(ScrapBehaviour behaviour) {
		return behaviour != Owner && !Owner.Faction.IsEnemy(behaviour.Faction);
	}
	
	private void OnDrawGizmos() {
		if(Broken) return;
		
		Gizmos.color = Color.yellow;
		
		Vector2 moveForce = _steeringCalculator.Calculate();
		
		Gizmos.DrawWireSphere(transform.position, _steering.ObstacleAvoidanceRange);

		Gizmos.DrawLine(transform.position, transform.position + (Vector3)moveForce);
	}
}