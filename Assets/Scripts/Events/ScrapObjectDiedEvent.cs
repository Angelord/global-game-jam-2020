using Claw;

public class ScrapObjectDiedEvent : GameEvent {

	public readonly ScrapBehaviour ScrapObject;

	public ScrapObjectDiedEvent(ScrapBehaviour scrapObject) {
		ScrapObject = scrapObject;
	}
}