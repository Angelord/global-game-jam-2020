using System.Collections;
using System.Collections.Generic;
using Claw;
using UnityEngine;



public class DevControls : MonoBehaviour {
	
#if UNITY_EDITOR 

	public Player PlayerOne;
	private bool _gameOver = false;
	
	private void Update() {
		if (Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.L)) {
			if (_gameOver) { return; }

			EventManager.QueueEvent(new PlayerDiedEvent(PlayerOne));
			_gameOver = true;
		}
	}

#endif
}
