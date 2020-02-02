using System;
using System.Collections.Generic;
using Claw;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Senses : MonoBehaviour {

	private readonly List<ScrapBehaviour> _objectsInRange = new List<ScrapBehaviour>();

	public event Action<ScrapBehaviour> OnObjectEnter;
	public event Action<ScrapBehaviour> OnObjectExit;

	public List<ScrapBehaviour> ObjectsInRange => _objectsInRange;

	public ScrapBehaviour GetAttackTarget(Faction faction) {
		return GetClosestMatch((scrap) => faction.IsEnemy(scrap.Faction) && scrap.Attackable);
	}

	private void OnEnable() {
		EventManager.AddListener<ScrapObjectDiedEvent>(HandleScrapObjectDiedEvent);
	}

	private void OnDisable() {
		EventManager.RemoveListener<ScrapObjectDiedEvent>(HandleScrapObjectDiedEvent);
	}

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
			if (OnObjectEnter != null) {
				OnObjectEnter(scrap);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other) {

		ScrapBehaviour scrap = other.GetComponent<ScrapBehaviour>();
		if (scrap != null) {
			_objectsInRange.Remove(scrap);
			if (OnObjectExit != null) {
				OnObjectExit(scrap);
			}
		}
	}
	
	private void HandleScrapObjectDiedEvent(ScrapObjectDiedEvent gameEvent) {
		if (_objectsInRange.Contains(gameEvent.ScrapObject)) {
			_objectsInRange.Remove(gameEvent.ScrapObject);
		}
	}
}