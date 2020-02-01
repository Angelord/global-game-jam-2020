public abstract class Building : ScrapBehaviour {

	public float MaxHealth;
	
	private float _health;

	private Player _owner;

	public override bool Repairable => _health <= 0.0f;
	
	public override float RepairCost => MaxHealth - _health;

	protected Player Owner => _owner;

	private void Start() {
		_health = MaxHealth / 2.0f;
	}
	
	public void TakeDamage(float amount) {
		_health -= amount;
		if (_health <= 0.0f) {
			_health = 0.0f;
			Break();
		}
	}

	public override void Repair(Player repairer) {

		_health = MaxHealth;

		_owner = repairer;
		
		// TODO : Spawn some particles
	}
	
	private void Break() {

		_owner = null;
		
		// TODO : Spawn some particles, switch sprites
	}
}