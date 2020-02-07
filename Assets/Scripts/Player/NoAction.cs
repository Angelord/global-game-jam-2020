public class NoAction : PlayerAction {
	
    public NoAction(Player player) : base(player) { End(); }

    public override bool IsReadyToUse() { return true; }

    protected override void OnBegin() { End(); }

    protected override void OnUpdate() { End(); }
}