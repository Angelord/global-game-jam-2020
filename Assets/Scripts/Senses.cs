using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Senses : MonoBehaviour {

	private readonly List<ScrapBehaviour> _objectsInRange = new List<ScrapBehaviour>();

	public ScrapBehaviour GetClosestMatch(Predicate<ScrapBehaviour> criteriaCheck) {

		ScrapBehaviour closestMatch = null;
		float curClosest = float.MaxValue;

		foreach (ScrapBehaviour scrapBehaviour in _objectsInRange) {
			if (!criteriaCheck(scrapBehaviour)) { continue; }

			float distance = Vector2.Distance(transform.position, scrapBehaviour.transform.position);
			if (distance < curClosest) {
				closestMatch = scrapBehaviour;
				curClosest = distance;
			}
		}

		return closestMatch;
	}
	
	private void OnTriggerEnter2D(Collider2D other) {

		ScrapBehaviour scrap = other.GetComponent<ScrapBehaviour>();
		if (scrap != null) {
			_objectsInRange.Add(scrap);
			OnScrapObjectEnter(scrap);
		}
	}

	private void OnTriggerExit2D(Collider2D other) {

		ScrapBehaviour scrap = other.GetComponent<ScrapBehaviour>();
		if (scrap != null) {
			_objectsInRange.Remove(scrap);	
			OnScrapObjectExit(scrap);
		}
	}

	protected virtual void OnScrapObjectEnter(ScrapBehaviour scrapBehaviour) {
		
	}

	protected virtual void OnScrapObjectExit(ScrapBehaviour scrapBehaviour) {
		
	}
}