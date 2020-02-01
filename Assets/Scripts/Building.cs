public abstract class Building : Construct {

	public override bool Repairable => Broken;

	public override bool Attackable => !Broken;

	protected override void OnRepair() {
	}
}