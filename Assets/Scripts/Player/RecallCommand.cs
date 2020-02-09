public class RecallCommand : PlayerCommand {
	public override void Execute(Creature unit) {
		unit.Recall();
	}
}

public class EnrageCommand : PlayerCommand {
	public override void Execute(Creature unit) {
		unit.Enrage();
	}
}