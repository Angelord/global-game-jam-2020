using System;
using System.Linq;
using Claw;
using Claw.Chrono;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : ScrapBehaviour {

	private static readonly int AnimBoolWorking = Animator.StringToHash("Working");
	private static readonly int AnimBoolMoving = Animator.StringToHash("Moving");

	[SerializeField] private Faction _faction;
	public PlayerStats Stats;
	public Camera PlayerCamera;
	public SpriteRenderer SparksSprite;
	public SpriteRenderer Body;
	public PlayerParticleEffect RecallEffect;
	public PlayerParticleEffect EnrageEffect;
	[SerializeField] private float _scrap;

	[Header("Wwise Events")]
	public AK.Wwise.Event SalvageScrapAudioEvent;
	public AK.Wwise.Event SalvageRobotAudioEvent;
	public AK.Wwise.Event CommandStartAudioEvent;
	public AK.Wwise.Event CommandStopAudioEvent;
	public AK.Wwise.Event StepEvent;

	public event Action<PlayerCommand> OnCommand;

	private Rigidbody2D _rigidbody;
	private CircleCollider2D _footCollider;
	private PlayerSenses _senses;
	private bool _enraging = false;
	private bool _recalling = false;
	private Animator _animator;
    private PlayerAction[] _actions;
    private PlayerAction _currentAction;

    public int WinCount { get => _faction.Wins; }

	public PlayerSenses Senses => _senses;

	public float Scrap => _scrap;

	public PlayerAction CurrentAction => _currentAction;
	
	public override Faction Faction => _faction;

	public override bool Attackable => true;

	public CircleCollider2D FootCollider => _footCollider;

	public bool Recalling => _recalling;

	public bool Enraging => _enraging;
	
	private InputSet Inputs => _faction.InputSet;

	protected override void OnDie() {
		EventManager.TriggerEvent(new PlayerDiedEvent(this));
		gameObject.SetActive(false);
	}

	private void Start() {
        _rigidbody = GetComponent<Rigidbody2D>();
		_footCollider = GetComponent<CircleCollider2D>();
		_senses = GetComponentInChildren<PlayerSenses>();
		_animator = GetComponent<Animator>();
		
		RecallEffect.Initialize(this);
		EnrageEffect.Initialize(this);
		
		Body.material = Faction.UnitMat;

		_actions = new PlayerAction[] {
			new RepairAction(this),
			new SalvageAction(this),
			new RecallAction(this),
			new EnrageAction(this), 
			new NoAction(this)
		};

		_currentAction = _actions.Last();

		EventManager.AddListener<PlayerDiedEvent>(HandlePlayerDiedEvent);
	}

	private void OnDestroy() {
		EventManager.RemoveListener<PlayerDiedEvent>(HandlePlayerDiedEvent);
	}

	private void Update() {

		if (!_currentAction.IsDone) {
			
			_animator.SetBool(AnimBoolWorking, true);
			SparksSprite.color = Faction.Color;
			
			_currentAction.Update();
			
			return;
		}

		if (Input.GetButton(Inputs.RepairButton) || Input.GetButton(Inputs.SalvageButton)) {
			_animator.SetBool(AnimBoolWorking, true);
		}
		else {
			_animator.SetBool(AnimBoolWorking, false);
		}
		
		SparksSprite.color = Color.clear;

		foreach (var playerAction in _actions) {
			if (!playerAction.IsReadyToUse()) continue;
			
			_currentAction = playerAction;
			playerAction.Begin();
			
			return;
		}
	}

	public void Repair(Construct target) {

		_scrap -= target.RepairCost;
		
		target.Repair(this);
	}

	public void Salvage(Construct target) {
		
		float salvageAmount = target.Salvage();
		
		_scrap += salvageAmount;
	}

	public void StartRecall() {
		_recalling = true;
		CommandStartAudioEvent.Post(gameObject);
		RecallEffect.Show();
		OnCommand?.Invoke(new RecallCommand());
	}

	public void StartEnrage() {
		_enraging = true;
		CommandStartAudioEvent.Post(gameObject);
		EnrageEffect.Show();
		OnCommand?.Invoke(new EnrageCommand());
	}

	public void StopRecall() {
		_recalling = false;
		CommandStopAudioEvent.Post(gameObject);
		RecallEffect.Stop();
	}
	
	public void StopEnrage() {
		_enraging = false;
		CommandStopAudioEvent.Post(gameObject);
		EnrageEffect.Stop();
	}

	// Animation event
	public void OnAnimationStep() {
		StepEvent.Post(gameObject);
	}

	protected override void OnTakeDamage() {
		if (PlayerCamera == null) {
			Debug.Log("NO CAMERA DETECTED");
		}

		PlayerCamera.transform.parent.GetComponent<PerlinShake>().PlayShake();
	}

	private void FixedUpdate() {

		Vector2 moveDir = Vector2.zero;
		moveDir.x = Input.GetAxis(Inputs.HorizontalAxis);
		moveDir.y = -Input.GetAxis(Inputs.VerticalAxis);

		if (moveDir.magnitude > 0.1f) {
			moveDir.Normalize();
			_animator.SetBool(AnimBoolMoving, true);
			Body.transform.localScale =
				moveDir.x > 0.0f ? new Vector3(1.0f, 1.0f, 1.0f) : new Vector3(-1.0f, 1.0f, 1.0f);
		}
		else {
			_animator.SetBool(AnimBoolMoving, false);
		}

		_rigidbody.AddForce(moveDir * Stats.MovementSpeed - _rigidbody.velocity * 0.9f, ForceMode2D.Impulse);
	}

	private void HandlePlayerDiedEvent(PlayerDiedEvent diedEvent) {
		if (diedEvent.Player.Faction != Faction) {
			_faction.Wins++;
		}
	}
}