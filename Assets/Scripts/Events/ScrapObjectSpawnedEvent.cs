using Claw;

public class ScrapObjectSpawnedEvent : GameEvent {
	
	public readonly ScrapBehaviour ScrapObject;

	public ScrapObjectSpawnedEvent(ScrapBehaviour scrapObject) {
		ScrapObject = scrapObject;
	}
}