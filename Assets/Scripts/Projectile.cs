using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile {

	public bool UsesTeamMaterial = false;
	public bool UsesTeamColor = false;
	public float Speed;
	private ScrapBehaviour _target;
	private float _damage;

	public ScrapBehaviour Target => _target;

	public void Initialize(ScrapBehaviour target, ScrapBehaviour attacker, float damage) {
		_target = target;
		_damage = damage;
		
		if (UsesTeamMaterial) {
			GetComponentInChildren<SpriteRenderer>().material = attacker.Faction.UnitMat;
		}
		else if (UsesTeamColor) {
			GetComponentInChildren<SpriteRenderer>().color = attacker.Faction.Color;
		}
		
		OnInitialize();
	}

	private void Update() {
		if (_target == null) {
			Destroy(this.gameObject);
			return;
		}
		
		MoveTowardsTarget();
		
		if (Vector2.Distance(_target.transform.position, transform.position) <= 0.08f) {

			if (_target.Attackable) {
				_target.TakeDamage(_damage);
			}
			
			Destroy(this.gameObject);
		}
	}

	protected virtual void OnInitialize() { }

	protected virtual void MoveTowardsTarget() {
		
		Vector2 moveDir = _target.transform.position - transform.position;
		moveDir.Normalize();
		transform.Translate(Time.deltaTime * Speed * moveDir);
	}
}