using UnityEngine;

public class Creature : MonoBehaviour {

	public CreatureStats Stats;
	public bool Broken;
	
	private Player _owner;
	private float _integrity;

	public void Resurrect(Player newOwner) {
		SetOwner(newOwner);
		_integrity = Stats.MaxIntegrity;
	}

	public void TakeDamage(float amount) {
		_integrity -= amount;
		if (_integrity <= 0.0f) {
			Die();		
		}
	}

	public float Salvage(float amount) {

		float salvage = amount;

		_integrity -= amount;
		if (_integrity <= 0.0f) {
			salvage += _integrity;
			// TODO : Play destroy animation / Spawn some particles
		}

		return salvage;
	}

	private void OnCommand(PlayerCommand command) {
		command.Execute(this);
	}

	private void Die() {

		UnsetOwner();

		Broken = true;
		
		// TODO : Play some particles
	}

	private void SetOwner(Player owner) {
		_owner = owner;
		_owner.OnCommand += OnCommand;
	}

	private void UnsetOwner() {
		if (_owner != null) {
			_owner.OnCommand -= OnCommand;
		}

		_owner = null;
	}
}