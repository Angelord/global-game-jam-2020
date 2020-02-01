using UnityEngine;
using UnityEngine.UI;

public class RepairGUI : GUIBehaviour {

	public Text PriceText;
	public Color CanAffordColor = Color.yellow;
	public Color CantAffordColor = Color.red;
	public float YOffset = 120.0f;
	private RectTransform _rectTransform;

	private void Start() {
		_rectTransform = PriceText.GetComponent<RectTransform>();
	}

	private void Update() {

		Construct target = Player.Senses.GetRepairTarget();
		if (target == null || !target.Repairable) {
			PriceText.gameObject.SetActive(false);
			return;
		}
		
		PriceText.gameObject.SetActive(true);

		float cost = target.RepairCost;
		bool affordable = target.RepairCost <= Player.Scrap;

		PriceText.color = affordable ? CanAffordColor : CantAffordColor;
		PriceText.text = cost.ToString();

		Vector2 screenPoss = Camera.WorldToScreenPoint(target.transform.position) + new Vector3(0.0f, YOffset, 0.0f);

		_rectTransform.anchoredPosition = screenPoss;
	}
}