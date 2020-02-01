// Creatures, Buildings etc...
public abstract class Construct : ScrapBehaviour {
	
	private const float BREAK_PERCENTAGE = 0.3f;

	public Player Owner;

	public bool Broken = true;
	
	public virtual bool Usable => false;

	public virtual bool Salvageable => false;

	public virtual bool Repairable => false;
	
	public override Faction Faction { get { return Owner == null ? Faction.Neutral : Owner.Faction; } }

	private void Start() {
		if (Broken) {
			CurHealth = MaxHealth * BREAK_PERCENTAGE;
			OnBreak();
		}
	}

	public virtual void Use() { }

	public void Repair(Player repairer) {
		if (Owner != repairer) {
			if (Owner != null) { Owner.OnCommand -= OnOwnerCommand; }
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
			Broken = true;
			Owner.OnCommand -= OnOwnerCommand;
			Owner = null;
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