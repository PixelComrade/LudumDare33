using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public WinLose player = null;
	public Rigidbody body = null;
	public float strength = 10000.0f;
	public float explosionRadius = 5.0f;
	public float explosionForce = 250.0f;
	public float maxTimeAlive = 60.0f;
	public ParticleSystem particles = null;
	public GameObject smokeEffect = null;
	public GameObject hitEffect = null;
	public GameObject explodeEffect = null;
	
	private float modifier = 1000.0f;
	private float timeAlive = 0;

	private bool exploded = false;
	
	void Start () {
		player = GameObject.Find("Player").GetComponent<WinLose>() as WinLose;
		body.AddForce(this.transform.forward * strength * modifier * Time.deltaTime);
		timeAlive = Time.time;
	}
	
	void FixedUpdate () {
		if ((Time.time - timeAlive) >= maxTimeAlive) {
			destroySelf();
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		if (!exploded) {
			explode();
		}

		smokeEffect.transform.parent = null;

		destroySelf();
	}

	private void explode() {
		GameObject effect = GameObject.Instantiate(hitEffect, this.transform.position, Quaternion.identity) as GameObject;
		effect.transform.forward = this.transform.up;

		GameObject exploding = GameObject.Instantiate(explodeEffect, this.transform.position, Quaternion.identity) as GameObject;
		exploding.transform.up = Vector3.up;

		RaycastHit[] hits = Physics.SphereCastAll(this.transform.position, explosionRadius, this.transform.forward);
		foreach (RaycastHit hit in hits) {
			if (hit.rigidbody != null) {
				if (hit.rigidbody.gameObject.name == "Player") {
					player.setRecentHit();
				}
				hit.rigidbody.AddExplosionForce(explosionForce * modifier, this.transform.position, explosionRadius);
			}
		}

		exploded = true;
	}
	
	private void destroySelf() {
		Destroy(this.gameObject);
	}
}
