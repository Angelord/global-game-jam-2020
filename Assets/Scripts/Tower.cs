using UnityEngine;

[RequireComponent(typeof(RangedAttacker))]
public class Tower : Building {
	
	private RangedAttacker _attacker;

	public RangedAttacker Attacker {
		get {
			if (_attacker == null) _attacker = GetComponent<RangedAttacker>();
			return _attacker;
		}
	}

	protected override void OnRepair() {
		Attacker.enabled = true;
	}

	protected override void OnBreak() {
		Attacker.enabled = false;
	}
}