public abstract class PlayerAction {

    private Player _player;
	
    private bool _isDone;

    public bool IsDone => _isDone;

    protected Player Player => _player;

    protected PlayerSenses Senses => _player.Senses;

    protected InputSet InputSet => _player.Faction.InputSet;

    protected PlayerStats Stats => _player.Stats;
    
    protected PlayerAction(Player player) {
        _player = player;
    }
	
    public void Begin() { _isDone = false; OnBegin(); }

    public void Update() { OnUpdate(); }
	
    protected void End() { _isDone = true; }
	
    public abstract bool ReadyToUse();

    protected abstract void OnBegin();

    protected abstract void OnUpdate();
}