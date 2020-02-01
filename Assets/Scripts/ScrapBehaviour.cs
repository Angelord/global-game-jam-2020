using UnityEngine;

public abstract class ScrapBehaviour : MonoBehaviour {
	
	public float MaxHealth;

	private float _curHealth;

	public abstract Fraction Fraction { get; }

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
		OnDie();
	}

	protected virtual void OnTakeDamage() { }

	protected virtual void OnDie() { }
}