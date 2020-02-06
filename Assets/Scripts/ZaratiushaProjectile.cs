using System.Collections;
using UnityEngine;

public class ZaratiushaProjectile : MonoBehaviour, IProjectile {

	public GameObject MiniProjectilePrefab;
	public int SpawnCount = 10;
	public float SpawnDuration = 1.0f;
	public float XOffset = 0.2f;
	public float RandomSpawnOffset = 0.05f;
	private ScrapBehaviour _target;
	private ScrapBehaviour _attacker;
	private float _damage;
	
	public void Initialize(ScrapBehaviour target, ScrapBehaviour attacker, float damage) {
		_target = target;
		_attacker = attacker;
		_damage = damage;
		
		transform.SetParent(attacker.transform);

		StartCoroutine(SpawnProjectiles());
	}

	private IEnumerator SpawnProjectiles() {

		float spawnFrequency = SpawnDuration / SpawnCount;
		
		for (int i = 0; i < SpawnCount; i++) {

			Vector2 left = (Vector2)transform.position + new Vector2(-XOffset, 0.0f);

			Vector2 right = (Vector2)transform.position + new Vector2(XOffset, 0.0f);
			
			SpawnProjectile(left);

			SpawnProjectile(right);

			yield return new WaitForSeconds(spawnFrequency);
		}
		
		Destroy(this.gameObject);
	}

	private void SpawnProjectile(Vector2 spawnPos) {
		
		spawnPos += new Vector2(Random.Range(-RandomSpawnOffset, RandomSpawnOffset), Random.Range(-RandomSpawnOffset, RandomSpawnOffset));
		
		GameObject miniProjectile = Instantiate(MiniProjectilePrefab, spawnPos, Quaternion.identity);
		
		miniProjectile.GetComponent<Projectile>().Initialize(_target, _attacker,_damage / SpawnCount / 2.0f);
	}
}