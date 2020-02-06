using UnityEngine;

public class ZaratiushaMiniProjectile : Projectile {

	public float MaxDeviation = 0.5f;

	private bool _deviating = true;
	private Vector2 _deviationTarget;
	
	protected override void OnInitialize() {
		
		Vector2 toTarget = Target.transform.position - transform.position;

		_deviationTarget = (Vector2)transform.position + toTarget * 0.4f;

		Vector2 randomOffset = new Vector2(Random.Range(-MaxDeviation, MaxDeviation),
			Random.Range(-MaxDeviation, MaxDeviation));
		
		randomOffset.Normalize();
		
		_deviationTarget += randomOffset;
	}

	protected override void MoveTowardsTarget() {

		if (_deviating && Vector2.Distance(_deviationTarget, transform.position) <= 0.1f) {
			_deviating = false;
		}

		Vector2 curTarget = _deviating ? _deviationTarget : (Vector2)Target.transform.position;
		
		Vector2 moveDir = curTarget - (Vector2)transform.position;
		
		moveDir.Normalize();
		
		transform.Translate(Time.deltaTime * Speed * moveDir);
	}
}