using UnityEngine;

public class RangedAttacker : Attacker {
	
	public GameObject Projectile;
	public GameObject barrel;
	public bool TeamColorProjectile = false;
	
	protected override void Attack(ScrapBehaviour target) {
		Projectile proj = GameObject.Instantiate(Projectile, barrel.transform.position, Quaternion.identity).GetComponent<Projectile>();

		if (TeamColorProjectile) {
			proj.GetComponentInChildren<SpriteRenderer>().material = Construct.Owner.Faction.UnitMat;
		}

		proj.Initialize(target.transform, () => {
			if (target == null || !target.Attackable) { return; }
			target.TakeDamage(Damage);
		});
	}
}