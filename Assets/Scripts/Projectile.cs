using System;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float Speed;
	private Transform _target;
	private Action _onReach;
	
	public void Initialize(Transform target, Action onReach) {
		_target = target;
		_onReach = onReach;
	}

	private void Update() {
		var targetPos = _target.position;
		
		Vector2 moveDir = targetPos - transform.position;
		moveDir.Normalize();
		transform.Translate(Time.deltaTime * Speed * moveDir);
		if (Vector2.Distance(targetPos, transform.position) <= 0.08f) {
			_onReach();
			Destroy(this.gameObject);
		}
	}
}