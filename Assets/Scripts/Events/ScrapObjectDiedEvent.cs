using Claw;

public class ScrapObjectSpawnedEvent : GameEvent {
	
	public readonly ScrapBehaviour ScrapObject;

	public ScrapObjectSpawnedEvent(ScrapBehaviour scrapObject) {
		ScrapObject = scrapObject;
	}
}

public class ScrapObjectDiedEvent : GameEvent {

	public readonly ScrapBehaviour ScrapObject;

	public ScrapObjectDiedEvent(ScrapBehaviour scrapObject) {
		ScrapObject = scrapObject;
	}
}