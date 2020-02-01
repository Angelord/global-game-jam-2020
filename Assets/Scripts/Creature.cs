using Claw;

public class Creature : Construct {
	
	public override bool Salvageable => Broken;

	public override bool Repairable => Broken;

	public override bool Attackable => !Broken;

	private void OnCommand(PlayerCommand command) {
		command.Execute(this);
	}
}