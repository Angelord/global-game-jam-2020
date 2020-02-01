using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Senses : MonoBehaviour {

	private readonly List<ScrapBehaviour> _objectsInRange = new List<ScrapBehaviour>();

	public ScrapBehaviour GetSalvageTarget() {
		foreach (var obj in _objectsInRange) {
			if (obj.Salvageable) return obj;
		}

		return null;
	}
	
	public ScrapBehaviour GetRepairTarget() {
		foreach (var obj in _objectsInRange) {
			if (obj.Repairable) return obj;
		}

		return null;
	}

	public ScrapBehaviour GetUseTarget() {
		foreach (var obj in _objectsInRange) {
			if (obj.Usable) return obj;
		}

		return null;
	}

	private void OnTriggerEnter2D(Collider2D other) {

		ScrapBehaviour scrap = other.GetComponent<ScrapBehaviour>();
		if (scrap != null) {
			_objectsInRange.Add(scrap);
		}
	}

	private void OnTriggerExit2D(Collider2D other) {

		ScrapBehaviour scrap = other.GetComponent<ScrapBehaviour>();
		if (scrap != null) {
			_objectsInRange.Add(scrap);	
		}
	}
}