using Claw;

public class ConstructRepairedEvent : GameEvent {

	public readonly Construct Construct;

	public ConstructRepairedEvent(Construct construct) {
		Construct = construct;
	}
}