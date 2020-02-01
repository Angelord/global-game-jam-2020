using UnityEngine;

public abstract class Building : MonoBehaviour, IRepairable, IUsable {

	public float MaxHealth;

	public float UseFrequency;
	
	private float _health;

	private float _lastUse;

	public bool CanUse => Time.time - _lastUse >= UseFrequency;

	public bool NeedsRepair => _health <= 0.0f;
	
	public float RepairCost => MaxHealth - _health;

	private void Start() {
		_health = MaxHealth / 2.0f;
	}

	public void Use() {
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

	public void Repair() {

		_health = MaxHealth;
		
		// TODO : Spawn some particles
	}

	protected abstract void OnUse();
	
	private void Break() {
		
		// TODO : Spawn some particles, switch sprites
	}
}