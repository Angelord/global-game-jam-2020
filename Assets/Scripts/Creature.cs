using Claw;
using UnityEngine;

public class Creature : Construct {

	public GameObject _scrapEffect;
	
	public override bool Salvageable => Broken;

	public override bool Repairable => Broken;

	public override bool Attackable => !Broken;

	private void OnCommand(PlayerCommand command) {
		command.Execute(this);
	}

	protected override void OnBreak() {
		_scrapEffect.SetActive(true);
	}

	protected override void OnRepair() {
		_scrapEffect.SetActive(false);
	}
}