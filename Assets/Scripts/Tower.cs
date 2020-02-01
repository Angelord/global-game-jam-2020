using UnityEngine;

[RequireComponent(typeof(RangedAttacker))]
public class Tower : Building {
	
	private RangedAttacker _attacker;

	private void Start() {
		_attacker = GetComponent<RangedAttacker>();
	}

	protected override void OnRepair() {
		_attacker.enabled = true;
	}

	protected override void OnBreak() {
		_attacker.enabled = false;
	}
}