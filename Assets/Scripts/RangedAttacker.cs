using UnityEngine;

public class RangedAttacker : Attacker {
	
	public GameObject Projectile;
	public GameObject barrel;
	public bool TeamColorProjectile = false;
	
	protected override void Attack(ScrapBehaviour target) {
		IProjectile proj = GameObject.Instantiate(Projectile, barrel.transform.position, Quaternion.identity).GetComponent<IProjectile>();

		proj.Initialize(target, Construct, Damage);
	}
}