using UnityEngine;

public class RangedAttacker : Attacker {
	
	public GameObject Projectile;
	public GameObject barrel;

	protected override void Attack(ScrapBehaviour target) {
		Projectile proj = GameObject.Instantiate(Projectile, barrel.transform.position, Quaternion.identity).GetComponent<Projectile>();
		
		proj.Initialize(target.transform, () => {
			if (target == null || !target.Attackable) { return; }
			target.TakeDamage(Damage);
		});
	}
}