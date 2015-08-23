using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour {

	public Rigidbody body = null;
	public float strength = 2000.0f;
	public float maxTimeAlive = 1.0f;
	public GameObject particles = null;
	
	public ThrowWave owner = null;

	private float modifier = 1000.0f;
	private float timeAlive = 0;

	void Start () {
		body.AddForce(this.transform.forward * strength * modifier * Time.deltaTime);
		timeAlive = Time.time;
	}

	void FixedUpdate () {
		if ((Time.time - timeAlive) >= maxTimeAlive) {
			destroySelf();
		}
	}

	void OnCollisionEnter(Collision collision) {
//		if (collision.collider.gameObject.layer != 13 && collision.collider.gameObject.layer != 8) {
//			destroySelf();
//		}
	}

	private void destroySelf() {
		particles.transform.parent = null;

		owner.notifyOwnerOfDeath();
		Destroy(this.gameObject);
	}
}
