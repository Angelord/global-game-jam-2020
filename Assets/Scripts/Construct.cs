using Claw;
using UnityEngine;

public abstract class Construct : ScrapBehaviour {

	private const float BREAK_PERCENTAGE = 0.3f;

	public Player Owner;

	public bool Broken = true;

	public virtual bool Usable => false;

	public virtual bool Salvageable => false;

	public virtual bool Repairable => false;

	public override Faction Faction { get { return Owner == null ? Faction.Neutral : Owner.Faction; } }

	public abstract AudioData AudioData { get; }

	public GameObject breakFx;
	
	private void Start() {
		PreStart();

		if (Broken) {
			Break(false);
		}
		else if(Owner != null) {
			Owner.OnCommand += OnOwnerCommand;
		}
		
		EventManager.AddListener<PlayerDiedEvent>(HandlePlayerDiedEvent);
	}

	private void OnDestroy() {
		EventManager.RemoveListener<PlayerDiedEvent>(HandlePlayerDiedEvent);
	}
	
	private void HandlePlayerDiedEvent(PlayerDiedEvent playerDiedEvent) {
		if (playerDiedEvent.Player == Owner) {
			Break();
		}
	}
	
	public void Repair(Player repairer) {
		if (Owner != repairer) {
			Owner = repairer;
			if (Owner != null) { Owner.OnCommand -= OnOwnerCommand; }
			repairer.OnCommand += OnOwnerCommand;
		}

		CurHealth = MaxHealth;
		
		Broken = false;
		
		Instantiate(Owner.Faction.RepairEffect, transform.position, Quaternion.identity);
		
		OnRepair();
	}

	public float Salvage() {
		
		EventManager.TriggerEvent(new ConstructSalvagedEvent(this, CurHealth));

		Die();

		OnSalvage();
		
		return CurHealth;
	}
	
	protected override void OnTakeDamage() {
		if (CurHealth <= MaxHealth * BREAK_PERCENTAGE) {
			Break();
		}
	}

	private void Break(bool spawnParticles = true) {
		CurHealth = MaxHealth * BREAK_PERCENTAGE;
		Broken = true;

		if (Owner != null) {
			Owner.OnCommand -= OnOwnerCommand;
		}
		Owner = null;
		
		if (spawnParticles && breakFx != null) {
			Instantiate(breakFx, transform);
		}
		
		OnBreak();
	}
	
	public virtual void Use() { }

	protected virtual void PreStart() { }

	protected virtual void OnSalvage() { }

	protected virtual void OnRepair() { }

	protected virtual void OnBreak() { }

	protected virtual void OnOwnerCommand(PlayerCommand command) { }
}