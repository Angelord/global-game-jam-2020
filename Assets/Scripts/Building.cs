using UnityEngine;

public abstract class Building : ScrapBehaviour {

	public float MaxHealth;

	public float UseFrequency;
	
	private float _health;

	private float _lastUse;

	public override bool Usable => !Repairable && Time.time - _lastUse >= UseFrequency;

	public override bool Repairable => _health <= 0.0f;
	
	public override float RepairCost => MaxHealth - _health;

	private void Start() {
		_health = MaxHealth / 2.0f;
	}

	public override void Use() {
		_lastUse = Time.time;
		OnUse();
	}

	public void TakeDamage(float amount) {
		_health -= amount;
		if (_health <= 0.0f) {
			_health = 0.0f;
			Break();
		}
	}

	public override void Repair() {

		_health = MaxHealth;
		
		// TODO : Spawn some particles
	}

	protected abstract void OnUse();
	
	private void Break() {
		
		// TODO : Spawn some particles, switch sprites
	}
}