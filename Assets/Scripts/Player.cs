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

	public float scrap;

	public event Action<PlayerCommand> OnCommand;

	private Rigidbody2D _rigidbody;
	private SpriteRenderer _renderer;
	private PlayerSenses _senses;

	public override Faction Faction => _faction;

	public override bool Attackable => true;

	private void Start() {
		_rigidbody = GetComponent<Rigidbody2D>();
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
			if (salvage != null)
			{
				Salvage(salvage);
      }
		}
		else if (Input.GetButtonDown(UseButton)) {
			ScrapBehaviour usable = _senses.GetUseTarget();
			if (usable != null) { Use(usable); }
		}
		else if (Input.GetButtonDown(RecallButton)) {
			Debug.Log("RECALL");
			OnCommand?.Invoke(new RecallCommand());
		}
	}

	private void Repair(ScrapBehaviour target) {
		Debug.Log("REPAIR");
	}

	private void Salvage(Construct target) {
		float salvageAmount = target.Salvage();
		scrap += salvageAmount;
		Debug.Log("SALVAGED " + salvageAmount);

	}

	private void Use(ScrapBehaviour target) {
		Debug.Log("USING");
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





















