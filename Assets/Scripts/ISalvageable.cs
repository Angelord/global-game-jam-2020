public interface ISalvageable {
	
	bool Salvageable { get; }
	
	float Salvage(float amount);
}