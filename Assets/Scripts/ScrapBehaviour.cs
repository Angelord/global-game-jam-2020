using UnityEngine;

public abstract class ScrapBehaviour : MonoBehaviour {
	
	public float MaxHealth;

	private float _curHealth;

	public bool isDead = false;

	public abstract Faction Faction { get; }

	public virtual bool Attackable => false;
	
	public virtual float RepairCost => MaxHealth - _curHealth;

	public float CurHealth { get => _curHealth; set => _curHealth = value; }

	public void TakeDamage(float amount) {
		_curHealth -= amount;
		OnTakeDamage();
		if (_curHealth <= 0.0f) {
			Die();
		}
	}

	protected void Die() {
		isDead = true;

		// TODO die better death
		Destroy(gameObject, 0.1f);

		OnDie();
	}

	protected virtual void OnTakeDamage() { }

	protected virtual void OnDie() { }
}