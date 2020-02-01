using System;
using Claw;
using Claw.Chrono;
using UnityEngine;

public abstract class ScrapBehaviour : MonoBehaviour {
	
	public float MaxHealth;

    [Range(0, 100)]
	public float armorPercent = 0;

	private float _curHealth;

	public bool isDead = false;

	public abstract Faction Faction { get; }

	public virtual bool Attackable => false;
	
	public virtual float RepairCost => MaxHealth - _curHealth;

	public float CurHealth { get => _curHealth; set => _curHealth = value; }
	
	private void Awake() {
		_curHealth = MaxHealth;
		
		CustomCoroutine.WaitThenExecute(0.01f, () => {
			EventManager.TriggerEvent(new ScrapObjectSpawnedEvent(this));
		});
	}
	
	public void TakeDamage(float amount) {
		_curHealth -= amount * (1 - armorPercent/100);
		OnTakeDamage();
		if (_curHealth <= 0.0f) {
			Die();
		}
	}
	
	protected void Die() {
		isDead = true;

		// TODO die better death
		Destroy(gameObject, 0.1f);
		
		EventManager.TriggerEvent(new ScrapObjectDiedEvent(this));
		
		OnDie();
	}

	protected virtual void OnTakeDamage() { }

	protected virtual void OnDie() { }
}