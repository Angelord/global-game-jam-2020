public class Creature : ScrapBehaviour {

	public CreatureStats Stats;
	public bool Broken;
	
	private Player _owner;
	private float _integrity;

	public override bool Salvageable => Broken;

	public override bool Repairable => Broken;

	public void TakeDamage(float amount) {
		_integrity -= amount;
		if (_integrity <= Stats.MaxIntegrity * 0.3f) {
			Break();
		}
	}

	public override void Repair(Player repairer) {
		SetOwner(repairer);
		_integrity = Stats.MaxIntegrity;
	}

	public override float Salvage(float amount) {

		float salvage = amount;

		_integrity -= amount;
		if (_integrity <= 0.0f) {
			salvage += _integrity;
			Destroy(this.gameObject);
			// TODO : Play destroy animation / Spawn some particles
		}

		return salvage;
	}

	private void OnCommand(PlayerCommand command) {
		command.Execute(this);
	}

	private void Break() {

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