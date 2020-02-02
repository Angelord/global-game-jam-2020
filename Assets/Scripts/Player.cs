using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : ScrapBehaviour {

	[SerializeField] private Faction _faction;
	public PlayerStats Stats;
	public string HorizontalAxis = "Horizontal_1";
	public string VerticalAxis = "Vertical_1";
	public string SalvageButton = "Salvage_1";
	public string RepairButton = "Repair_1";
	public string UseButton = "Use_1";
	public string RecallButton = "Recall_1";
    public Camera PlayerCamera;
    public SpriteRenderer Sparks;
    public SpriteRenderer Body;
    
    public event Action<PlayerCommand> OnCommand;

	private float _scrap;
	private Rigidbody2D _rigidbody;
	private CircleCollider2D _footCollider;
	private PlayerSenses _senses;
	private float _lastRecall;
	private Animator _animator;
	
	public PlayerSenses Senses => _senses;

	public float Scrap => _scrap;

	public override Faction Faction => _faction;

	public override bool Attackable => true;

	public CircleCollider2D FootCollider => _footCollider;

	public bool Recalling { get { return Time.time - _lastRecall <= Stats.RecallDuration; } }

	private void Start() {
		_rigidbody = GetComponent<Rigidbody2D>();
		_footCollider = GetComponent<CircleCollider2D>();
		_senses = GetComponentInChildren<PlayerSenses>();
		_animator = GetComponent<Animator>();
		
		Body.material = Faction.UnitMat;
	}

	private void Update() {

		if (Input.GetButton(RepairButton)) {
			_animator.SetBool("Working", true);
			Sparks.color = Stats.SparkColorRepair;
		}
		else if (Input.GetButton(SalvageButton)) {
			_animator.SetBool("Working", true);
			Sparks.color = Stats.SparkColorSalvage;
		}
		else if (Input.GetButton(UseButton)) {
			_animator.SetBool("Working", true);
			Sparks.color = Stats.SparkColorUse;
		}
		else {
			_animator.SetBool("Working", false);
			Sparks.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
		}

		if (Input.GetButtonDown(RepairButton)) {
			ScrapBehaviour repairable = _senses.GetRepairTarget();
			if (repairable != null) { Repair(repairable as Construct); }
		}
		else if (Input.GetButtonDown(SalvageButton)) {
			_animator.SetBool("Working", true);
			Sparks.color = Stats.SparkColorRepair;
			Construct salvage = _senses.GetSalvageTarget();
			if (salvage != null) {
				Salvage(salvage);
			}
		}
		else if (Input.GetButtonDown(UseButton)) {
			ScrapBehaviour usable = _senses.GetUseTarget();
			if (usable != null) { Use(usable); }
		}
		else if (Input.GetButtonDown(RecallButton)) {
			Recall();
		}
	}

	private void Repair(Construct target) {
		Debug.Log("REPAIR");
        if(_scrap < target.RepairCost)
        {
            // TODO add beep sound
            return;
        }
        // TODO add repair sound
        _scrap -= target.RepairCost;
		    target.Repair(this);
	}

    protected override void OnTakeDamage() {
        Debug.Log("SHAKE CAMERA");
        if(PlayerCamera == null)
        {
            Debug.Log("NO CAMERA DETECTED");
        }
        PlayerCamera.transform.parent.GetComponent<PerlinShake>().PlayShake();
    }


    private void Salvage(Construct target) {
		float salvageAmount = target.Salvage();
		_scrap += salvageAmount;
		Debug.Log("SALVAGED " + salvageAmount);
	}

	private void Use(ScrapBehaviour target) {
		Debug.Log("USING");
	}

	private void Recall() {
		Debug.Log("RECALL");

		if (Time.time - _lastRecall < Stats.RecallFrequency) { return; }

		_lastRecall = Time.time;

		OnCommand?.Invoke(new RecallCommand());
	}

	private void FixedUpdate() {

		Vector2 moveDir = Vector2.zero;
		moveDir.x = Input.GetAxis(HorizontalAxis);
		moveDir.y = -Input.GetAxis(VerticalAxis);

		if (moveDir.magnitude > 0.1f) {
			moveDir.Normalize();
			_animator.SetBool("Moving", true);
			Body.transform.localScale = moveDir.x > 0.0f ? new Vector3(1.0f, 1.0f, 1.0f) : new Vector3(-1.0f, 1.0f, 1.0f);
		}
		else {
			_animator.SetBool("Moving", false);
		}

		_rigidbody.AddForce(moveDir * Stats.MovementSpeed - _rigidbody.velocity * 0.9f, ForceMode2D.Impulse);
	}
}





















