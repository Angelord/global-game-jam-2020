using System;
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

    public event Action<PlayerCommand> OnCommand;

	private float _scrap;
	private Rigidbody2D _rigidbody;
	private CircleCollider2D _footCollider;
	private SpriteRenderer _renderer;
	private PlayerSenses _senses;
	private float _lastRecall;

	public PlayerSenses Senses => _senses;

	public float Scrap => _scrap;

	public override Faction Faction => _faction;

	public override bool Attackable => true;

	public CircleCollider2D FootCollider => _footCollider;

	public bool Recalling { get { return Time.time - _lastRecall <= Stats.RecallDuration; } }

	private void Start() {
		_rigidbody = GetComponent<Rigidbody2D>();
		_footCollider = GetComponent<CircleCollider2D>();
		_renderer = GetComponentInChildren<SpriteRenderer>();
		_senses = GetComponentInChildren<PlayerSenses>();
	}

	private void Update() {

		if (Input.GetButtonDown(RepairButton)) {
			ScrapBehaviour repairable = _senses.GetRepairTarget();
			if (repairable != null) { Repair(repairable); }
		}
		else if (Input.GetButtonDown(SalvageButton)) {
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

	private void Repair(ScrapBehaviour target) {
		Debug.Log("REPAIR");
        if(_scrap < target.RepairCost)
        {
            // TODO add beep sound
            return;
        }
        // TODO add repair sound
        _scrap -= target.RepairCost;
	}

    protected override void OnTakeDamage() {
        Debug.Log("SHAKE CAMERA");
        if(PlayerCamera == null)
        {
            Debug.Log("NO CAMERA DETECTED");
        }
        PlayerCamera.GetComponent<PerlinShake>().PlayShake();
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
			_renderer.flipX = moveDir.x > 0.0f;
		}

		_rigidbody.AddForce(moveDir * Stats.MovementSpeed - _rigidbody.velocity * 0.9f, ForceMode2D.Impulse);
	}

}





















