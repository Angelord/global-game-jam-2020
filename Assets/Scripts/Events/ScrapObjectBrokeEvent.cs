using Claw;

public class ScrapObjectBrokeEvent : GameEvent {

	public readonly ScrapBehaviour ScrapObject;
	
	public ScrapObjectBrokeEvent(ScrapBehaviour scrapObject) {
		ScrapObject = scrapObject;
	}
}