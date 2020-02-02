﻿// Creatures, Buildings etc...

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

	private void Start() {
		OnStart();
		if (Broken) {
			CurHealth = MaxHealth * BREAK_PERCENTAGE;
			OnBreak();
		}
		else if(Owner != null) {
			Owner.OnCommand += OnOwnerCommand;
		}
	}

	public virtual void Use() { }

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
			CurHealth = MaxHealth * BREAK_PERCENTAGE;
			Broken = true;
			Owner.OnCommand -= OnOwnerCommand;
			Owner = null;
			OnBreak();
		}
	}

	protected virtual void OnSalvage() {

		// TODO : Spawn some particles

		// TODO : Throw event
	}

	protected virtual void OnRepair() {
		//TODO : Change sprites, play some animation, etc...
	}

	protected virtual void OnBreak() {
		Debug.Log(breakFx);

		if (breakFx != null)
    {
			Instantiate(breakFx, transform);
		}

		// TODO : Throw event
	}

	protected virtual void OnOwnerCommand(PlayerCommand command) { }
}