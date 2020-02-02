using Claw;

public class PlayerDiedEvent : GameEvent {

	public readonly Player Player;

	public PlayerDiedEvent(Player player) {
		Player = player;
	}
}