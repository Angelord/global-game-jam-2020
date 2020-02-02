using Claw;

public class ConstructSalvagedEvent : GameEvent {
	
	public readonly Construct Construct;
	public readonly float Amount;

	public ConstructSalvagedEvent(Construct construct, float amount) {
		Construct = construct;
		Amount = amount;
	}
}