public abstract class Construct : ScrapBehaviour {
	
	private const float BREAK_PERCENTAGE = 0.3f;

	private bool _broken;

	private Player _owner;
	
	public virtual bool Usable => false;

	public virtual bool Salvageable => false;

	public virtual bool Repairable => false;

	protected bool Broken => _broken;

	protected Player Owner => _owner;

	public override Fraction Fraction { get { return _owner == null ? Fraction.Neutral : _owner.Fraction; } }
	
	public virtual void Use() { }

	public void Repair(Player repairer) {
		if (_owner != repairer) {
			if (_owner != null) { _owner.OnCommand -= OnOwnerCommand; }
			repairer.OnCommand += OnOwnerCommand;
		}

		CurHealth = MaxHealth;
		OnRepair();
	}

	public float Salvage() {
		
		OnSalvage();
		
		Die();
		
		return CurHealth;
	}

	protected override void OnTakeDamage() {
		if (CurHealth <= MaxHealth * BREAK_PERCENTAGE) {
			CurHealth = MaxHealth * BREAK_PERCENTAGE;
			_broken = true;
			_owner.OnCommand -= OnOwnerCommand;
			_owner = null;
			OnBreak();
		}
	}

	protected virtual void OnSalvage() {
		
		// TODO : Spawn some particles
		
		// TODO : Throw event
	}

	protected virtual void OnRepair() {
		//TODO : Change sprites, play some animation, etc...
	}

	protected virtual void OnBreak() {
		// TODO : Throw event
	}
	
	protected virtual void OnOwnerCommand(PlayerCommand command) { }
}