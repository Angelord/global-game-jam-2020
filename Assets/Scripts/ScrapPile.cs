using UnityEngine;

public class ScrapPile : ScrapBehaviour {

	public float Value;
	
	public override bool Salvageable => true;

	public override float Salvage(float amount) {

		float salvage = amount;
		Value -= amount;
		if (Value <= 0.0f) {
			salvage += Value;
			// TODO : Play particles and stuff.
			
			Destroy(this.gameObject);
		}

		return salvage;
	}
}