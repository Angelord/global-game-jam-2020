public class PlayerSenses : Senses {

	public Construct GetRepairTarget() {
		return GetClosestMatch((scrap) => {
			
			Construct repTarget = scrap as Construct;

			if (repTarget == null) { return false; }

			return repTarget.Repairable;
		}) as Construct;
	}
	
	public Construct GetSalvageTarget() {
		return GetClosestMatch((scrap) => {
			
			Construct repTarget = scrap as Construct;

			if (repTarget == null) { return false; }

			return repTarget.Salvageable;
		}) as Construct;
	}
	
	
	public Construct GetUseTarget() {
		return GetClosestMatch((scrap) => {
			
			Construct repTarget = scrap as Construct;

			if (repTarget == null) { return false; }

			return repTarget.Usable;
		}) as Construct;
	}
}