﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	private static readonly Color DefaultColor = new Color(0.24f, 0.78f, 0.42f);
	private static readonly Color EnemyColor = new Color(0.77f, 0.16f, 0.16f);
	private static readonly Color BrokenColor = new Color(0.63f, 0.5f, 0.4f);

	public float YOffset = 100.0f;
	public Slider Bar;
	public Image BarFill;

	private ScrapBehaviour _target;
	private Player _player;
	private Camera _camera;

	public ScrapBehaviour Target => _target;

	public void Initialize(Camera camera, Player player, ScrapBehaviour scrapBehaviour) {
	
		_camera = camera;

		_player = player;
		
		_target = scrapBehaviour;
	}
	
	private void Update() {
		if(_target == null) return;

		Vector2 screenPos = GUIBehaviour.WorldToScreen(_target.transform.position, _camera, YOffset);
		
		GetComponent<RectTransform>().anchoredPosition = screenPos;

		Bar.value = _target.CurHealth / _target.MaxHealth;

		if (_target is Construct) {
			Construct target = _target as Construct;
			if (target.Broken) {
				BarFill.color = BrokenColor;
			}
			else if (_player.Faction.IsEnemy(_target.Faction)) {
				BarFill.color = EnemyColor;
			}
			else {
				BarFill.color = DefaultColor;
			}
		}
		else {
			BarFill.color = BrokenColor;
		}
	}
}