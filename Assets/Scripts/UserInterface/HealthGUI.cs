using System;
using System.Linq;
using Boo.Lang;
using Claw;
using UnityEngine;

public class HealthGUI : GUIBehaviour {
	
	public GameObject BarPrefab;
	private readonly List<HealthBar> _bars = new List<HealthBar>();

	private void Awake() {
		EventManager.AddListener<ScrapObjectSpawnedEvent>(HandleObjSpawnedEvent);
		EventManager.AddListener<ScrapObjectDiedEvent>(HandleObjDiedEvent);
	}

	private void OnDestroy() {
		EventManager.RemoveListener<ScrapObjectSpawnedEvent>(HandleObjSpawnedEvent);
		EventManager.RemoveListener<ScrapObjectDiedEvent>(HandleObjDiedEvent);
	}
	
	private void HandleObjSpawnedEvent(ScrapObjectSpawnedEvent objectSpawnedEvent) {

		HealthBar newBar = Instantiate(BarPrefab, transform).GetComponent<HealthBar>();
		
		newBar.Initialize(Camera, Player, objectSpawnedEvent.ScrapObject);

		_bars.Add(newBar);
	}

	private void HandleObjDiedEvent(ScrapObjectDiedEvent objectDiedEvent) {
		
		_bars.Find((bar) => bar.Target == objectDiedEvent.ScrapObject, out HealthBar targetBar);

		if (targetBar == null) return;
		
		Destroy(targetBar.gameObject);
	}
}