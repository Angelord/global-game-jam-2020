using Claw;

public class GameOverEvent : GameEvent {
}

public class PlayerDiedEvent : GameEvent {

	public readonly Player Player;

	public PlayerDiedEvent(Player player) {
		Player = player;
	}
}