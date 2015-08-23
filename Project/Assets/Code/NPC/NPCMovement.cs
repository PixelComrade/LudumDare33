using UnityEngine;
using System.Collections;

public class NPCMovement : NPC {

	public float speed    = 5.0f;
	public float maxSpeed = 5.0f;
	public float sprint   = 2.0f;
	public bool sprinting = false;

	protected void move(Vector3 direction) {
		if (this.body.velocity.magnitude <= maxSpeed) {
			Vector3 target = direction * speed * this.multiplier * Time.deltaTime;
			if (sprinting) {
				target = target * sprint;
			}
			this.body.AddForce(target);
		}
	}

	protected void resetMovement() {
		this.body.AddForce(-this.body.velocity * this.body.velocity.magnitude * Time.deltaTime);
	}

	protected void turnBody(Vector3 target) {
		this.body.transform.forward = Vector3.Lerp(this.body.transform.forward, target, Time.deltaTime);
	}

	protected void turnHead(Vector3 target) {
		this.head.transform.forward = Vector3.Lerp(this.head.transform.forward, target, Time.deltaTime);
	}
}
