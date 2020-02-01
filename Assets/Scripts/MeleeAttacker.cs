public class MeleeAttacker : Attacker {
	
	protected override void Attack(ScrapBehaviour target) {
		
		target.TakeDamage(Damage);
	}
}