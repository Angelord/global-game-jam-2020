using UnityEngine;

public class Maggot : MonoBehaviour {

	public float LifetimeAfterSplash;
	private bool _dead;

	private void OnTriggerEnter2D(Collider2D other) {
		if(_dead) return;
		
		if (other.gameObject.CompareTag("Player")) {
			_dead = true;
			GetComponent<Animator>().SetTrigger("Die");
			Destroy(this.gameObject, LifetimeAfterSplash);
		}
	}
}