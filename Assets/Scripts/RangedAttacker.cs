using UnityEngine;

public class RangedAttacker : Attacker {
	
	public GameObject Projectile;

	protected override void Attack(ScrapBehaviour target) {
		Projectile proj = GameObject.Instantiate(Projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
		
		proj.Initialize(target.transform, () => {
			if (target == null || !target.Attackable) { return; }
			target.TakeDamage(Damage);
		});
	}
}