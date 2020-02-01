using UnityEngine;

public abstract class UsableBuilding : Building {

	public float UseFrequency;
	
	private float _lastUse;

	public override bool Usable => !Repairable && Time.time - _lastUse >= UseFrequency;
	
	public override void Use() {
		_lastUse = Time.time;
		OnUse();
	}

	protected abstract void OnUse();
}