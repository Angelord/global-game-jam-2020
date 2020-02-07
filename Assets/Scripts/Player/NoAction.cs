public class NoAction : PlayerAction {
	
    public NoAction(Player player) : base(player) { End(); }

    public override bool ReadyToUse() { return true; }

    protected override void OnBegin() { End(); }

    protected override void OnUpdate() { End(); }
}