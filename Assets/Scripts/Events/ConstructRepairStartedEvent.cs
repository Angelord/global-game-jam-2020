using Claw;

public class ConstructRepairStartedEvent : GameEvent {

	public readonly Construct Construct;

	public ConstructRepairStartedEvent(Construct construct) {
		Construct = construct;
	}
}