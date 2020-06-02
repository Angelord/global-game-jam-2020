using UnityEngine;

public class ScrapPile : Construct {
	
	public override bool Salvageable => !isDead;

	public override AudioData AudioData => null;
}