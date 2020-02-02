// Creatures, Buildings etc...

using System;
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

	public GameObject breakFx;

	private bool skipBreak = false;

	private void Start() {
		OnStart();
		if (Broken) {
			Break();
			skipBreak = true;
			OnBreak();
		}
		else if(Owner != null) {
			Owner.OnCommand += OnOwnerCommand;
		}
		
		EventManager.AddListener<PlayerDiedEvent>(HandlePlayerDiedEvent);
	}

	private void OnDestroy() {
		EventManager.RemoveListener<PlayerDiedEvent>(HandlePlayerDiedEvent);
	}

	public virtual void Use() { }
	
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
		OnRepair();
	}

	public float Salvage() {

		OnSalvage();

		EventManager.TriggerEvent(new ConstructSalvagedEvent(this, CurHealth));

		Die();

		return CurHealth;
	}

	protected virtual void OnStart() { }

	protected override void OnTakeDamage() {
		if (CurHealth <= MaxHealth * BREAK_PERCENTAGE) {
			Break();
			OnBreak();
		}
	}

	private void Break() {
		CurHealth = MaxHealth * BREAK_PERCENTAGE;
		Broken = true;

		if (Owner != null) {
			Owner.OnCommand -= OnOwnerCommand;
		}
		Owner = null;
	}

	protected virtual void OnSalvage() {

		// TODO : Spawn some particles

		// TODO : Throw event
	}

	protected virtual void OnRepair() {
		//TODO : Change sprites, play some animation, etc...
	}

	protected virtual void OnBreak() {
        if(skipBreak)
        {
			skipBreak = false;
			return;
        }

		if (breakFx != null)
        {
			Instantiate(breakFx, transform);
		}

		// TODO : Throw event
	}

	protected virtual void OnOwnerCommand(PlayerCommand command) { }
}