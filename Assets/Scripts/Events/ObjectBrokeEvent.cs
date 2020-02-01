using Claw;

public class ObjectBrokeEvent : GameEvent {

	public readonly Construct Construct;

	public ObjectBrokeEvent(Construct construct) {
		Construct = construct;
	}
}