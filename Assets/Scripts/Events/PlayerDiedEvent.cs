using Claw;

public class GameOverEvent : GameEvent { }

public class GameStartedEvent : GameEvent { }

public class PlayerDiedEvent : GameEvent {

	public readonly Player Player;

	public PlayerDiedEvent(Player player) {
		Player = player;
	}
}

public class AttackerAttackedEvent : GameEvent {

	public readonly Attacker Attacker;

	public AttackerAttackedEvent(Attacker attacker) {
		Attacker = attacker;
	}
}