using UnityEngine;
using System.Collections;

public class ReactionTrigger : MonoBehaviour {

	public WinLose player = null;
	public NPCBehaviour myBehaviour = null;

	public float hitThreshold = 12.0f;

	private string lastCollider = "";
	private bool pointsAwardedForCollision = false;
	private bool pointsAwardedForBreak = false;

	void Awake() {
		player = GameObject.Find("Player").GetComponent<WinLose>() as WinLose;
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.relativeVelocity.magnitude >= hitThreshold) {
			lastCollider = collision.collider.gameObject.tag;
			if (lastCollider == "PC" && !pointsAwardedForCollision) {
				player.updateScore(1);
				pointsAwardedForCollision = true;
			}
			myBehaviour.triggerHit(this.transform.position);
		}
	}
	
	void OnJointBreak(float breakForce) {
		if (lastCollider == "PC" && !pointsAwardedForBreak) {
			player.updateScore(2);
			pointsAwardedForBreak = true;
		}
		myBehaviour.setDead();
		myBehaviour.triggerDeath(this.transform.position);
	}
}
