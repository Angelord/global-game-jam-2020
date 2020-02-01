public class RangedAttack : Attacker {

	protected override void Attack(ScrapBehaviour target) {
		
		target.TakeDamage(Damage);
	}
}