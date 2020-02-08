using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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

		// Team color the trail renderer if there is one
		TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
		if (trailRenderer != null) {
			
			Gradient colorGradient = trailRenderer.colorGradient;
			colorGradient.SetKeys(
				new[] { new GradientColorKey(attacker.Faction.Color, 0.0f), new GradientColorKey(attacker.Faction.Color, 1.0f) },
				colorGradient.alphaKeys
			);
			trailRenderer.colorGradient = colorGradient;
		}

		OnInitialize();
	}

	private void Update() {
		if (_target == null) {
			Destroy(this.gameObject);
			return;
		}
		
		MoveTowardsTarget();
		
		if (Vector2.Distance(_target.transform.position, transform.position) <= 0.18f) {

			if (_target.Attackable) {
				_target.TakeDamage(_damage);
			}
			
			Destroy(this.gameObject);
		}
	}

	protected virtual void OnInitialize() { }

	protected virtual void MoveTowardsTarget() {
		
		Vector2 moveDir = _target.transform.position - transform.position;

		if (moveDir.magnitude > 1.0f) {
			moveDir.Normalize();
		}

		transform.Translate(Time.deltaTime * Speed * moveDir);
	}
}