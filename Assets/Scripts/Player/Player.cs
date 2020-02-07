using System;
using System.Linq;
using Claw;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : ScrapBehaviour {

	[SerializeField] private Faction _faction;
	public PlayerStats Stats;
	public Camera PlayerCamera;
	[FormerlySerializedAs("Sparks")] public SpriteRenderer SparksSprite;
	public SpriteRenderer Body;

	public event Action<PlayerCommand> OnCommand;

	[SerializeField] private float _scrap;
	private Rigidbody2D _rigidbody;
	private CircleCollider2D _footCollider;
	private PlayerSenses _senses;
	private float _lastRecallTime;
	private Animator _animator;
    private AudioManager _audioManager;
    private PlayerAction[] _actions;
    private PlayerAction _currentAction;

    public int WinCount { get => _faction.Wins; }

	public PlayerSenses Senses => _senses;

	public float Scrap => _scrap;

	public PlayerAction CurrentAction => _currentAction;
	
	public override Faction Faction => _faction;

	public override bool Attackable => true;

	public CircleCollider2D FootCollider => _footCollider;

	public float LastRecallTime => _lastRecallTime;
	
	public bool Recalling => Time.time - _lastRecallTime <= Stats.RecallDuration;

	private InputSet Inputs => _faction.InputSet;

	protected override void OnDie() {
		EventManager.TriggerEvent(new PlayerDiedEvent(this));
		gameObject.SetActive(false);
	}

	private void Start() {
        _audioManager = FindObjectOfType<AudioManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
		_footCollider = GetComponent<CircleCollider2D>();
		_senses = GetComponentInChildren<PlayerSenses>();
		_animator = GetComponent<Animator>();

		Body.material = Faction.UnitMat;

		_actions = new PlayerAction[] {
			new RepairAction(this),
			new SalvageAction(this),
			new RecallAction(this),
			new NoAction(this), 
		};

		_currentAction = _actions.Last();

		EventManager.AddListener<PlayerDiedEvent>(HandlePlayerDiedEvent);
	}

	private void OnDestroy() {
		EventManager.RemoveListener<PlayerDiedEvent>(HandlePlayerDiedEvent);
	}

	private void Update() {

		if (!_currentAction.IsDone) {
			
			_animator.SetBool("Working", true);
			SparksSprite.color = Faction.Color;
			
			_currentAction.Update();
			
			return;
		}

		_animator.SetBool("Working", false);
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
        
		_audioManager.Play("repair_sound");
        
		target.Repair(this);
	}

	public void Salvage(Construct target) {
		
		float salvageAmount = target.Salvage();
		
		_scrap += salvageAmount;
		
		_audioManager.Play("collect_scrap");
		
		Debug.Log("SALVAGED " + salvageAmount);
	}

	public void Recall() {

		_lastRecallTime = Time.time;

		OnCommand?.Invoke(new RecallCommand());
	}

	protected override void OnTakeDamage() {
		Debug.Log("SHAKE CAMERA");
        _audioManager.Play("player_gets_hit");
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
			_animator.SetBool("Moving", true);
			Body.transform.localScale =
				moveDir.x > 0.0f ? new Vector3(1.0f, 1.0f, 1.0f) : new Vector3(-1.0f, 1.0f, 1.0f);
		}
		else {
			_animator.SetBool("Moving", false);
		}

		_rigidbody.AddForce(moveDir * Stats.MovementSpeed - _rigidbody.velocity * 0.9f, ForceMode2D.Impulse);
	}

	private void HandlePlayerDiedEvent(PlayerDiedEvent diedEvent) {
		if (diedEvent.Player.Faction != Faction) {
			_faction.Wins++;
		}
	}
}