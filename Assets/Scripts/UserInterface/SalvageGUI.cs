using Claw;
using UnityEngine;
using UnityEngine.UI;

public class SalvageGUI : GUIBehaviour {

	public GameObject FloatingTextPrefab;
	
	private void OnEnable() {
		EventManager.AddListener<ConstructSalvagedEvent>(HandleConstructSalvagedEvent);
	}

	private void OnDisable() {
		EventManager.RemoveListener<ConstructSalvagedEvent>(HandleConstructSalvagedEvent);
	}

	private void HandleConstructSalvagedEvent(ConstructSalvagedEvent gameEvent) {
		Vector2 targetPos = WorldToScreen(gameEvent.Construct.transform.position, Camera, 0.0f);
		GameObject floatingText = Instantiate(FloatingTextPrefab, transform);
		floatingText.GetComponent<Text>().text = "+" + gameEvent.Amount;
		floatingText.GetComponent<FloatingText>().Init(targetPos);
	}
}