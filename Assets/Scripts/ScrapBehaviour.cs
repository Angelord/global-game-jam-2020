using System;
using Claw;
using Claw.Chrono;
using Scrapper;
using UnityEngine;

public abstract class ScrapBehaviour : MonoBehaviour {
	
	public float MaxHealth;

	public float BarYOffset = 1.0f;
	
    [Range(0, 100)]
	public float armorPercent = 0;
	
	private ColorBlink _damageBlink;

	private float _curHealth;

	public bool isDead = false;

	public bool DestroyOnDeath = true;
	
	public abstract Faction Faction { get; }

	public virtual bool Attackable => false;
	
	public virtual float RepairCost => MaxHealth - _curHealth;
	
	public float CurHealth { get => _curHealth; set => _curHealth = value; }
	
	private void Awake() {
		_curHealth = MaxHealth;
		_damageBlink = GetComponent<ColorBlink>();
		
		CustomCoroutine.WaitThenExecute(0.01f, () => {
			EventManager.TriggerEvent(new ScrapObjectSpawnedEvent(this));
		});
	}
	
	public void TakeDamage(float amount) {
		if(isDead) return;
		
		_curHealth -= amount * (1 - armorPercent/100);
		OnTakeDamage();
		if (_curHealth <= 0.0f) {
			Die();
		}
		else {
			_damageBlink?.Blink();
		}
	}
	
	protected void Die() {
		isDead = true;

		// TODO die better death
		if (DestroyOnDeath) {
			Destroy(gameObject, 0.1f);
		}

		EventManager.TriggerEvent(new ScrapObjectDiedEvent(this));
		
		OnDie();
	}

	protected virtual void OnTakeDamage() { }

	protected virtual void OnDie() { }
}