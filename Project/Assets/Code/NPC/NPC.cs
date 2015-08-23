using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

	public Rigidbody body 			= null;
	public Rigidbody head 			= null;

	public GameObject splatterHit	= null;
	public GameObject splatterDead 	= null;

	protected bool recentlyHit		= false;
	protected bool isDead 			= false;
	protected float multiplier 		= 1000.0f;

	public bool setDead() {
		isDead = true;
		return true;
	}

	public void triggerHit(Vector3 position) {
		GameObject.Instantiate(splatterHit, position, Quaternion.identity);
		recentlyHit = true;
	}

	public void triggerDeath(Vector3 position) {
		GameObject.Instantiate(splatterDead, position, Quaternion.identity);
	}
}
