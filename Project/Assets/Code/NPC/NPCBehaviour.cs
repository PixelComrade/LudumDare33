using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCBehaviour : NPCMovement {

	public GameObject[] waypoints 			= new GameObject[0];
	public int maxRecentWaypoints			= 2;
	public float threshold 					= 2.0f;
	public float scaredLength				= 10.0f;
	public float maxTimeOnSpot				= 1.0f;
	
	private List<Transform> recentWaypoints = new List<Transform>();
	private Transform waypoint 				= null;
	private Vector3 target     				= Vector3.zero;
	private bool active 					= false;
	private bool scared						= false;
	private float scaredTimer				= 0.0f;

	private bool isStuck					= false;
	private float lastTimeOnSpot			= 0.0f;
	private Vector3 lastSpot				= Vector3.zero;

	private bool ready						= false;
	
	void Awake () {
		lastSpot = this.body.transform.position;
		lastTimeOnSpot = Time.time;
	}

	void FixedUpdate () {
		if (ready) {
			if (!this.isDead) {
				determineScaredState();
				determineStuckState();
				if (!isStuck) {
					determineActiveState();
					turnHead();
				}
				triggerMovement();
				refreshRecentWaypoints();
			}
		} else {
			if (Time.time >= 1.0f) {
				ready = true;
				if (waypoints.Length == 0) {
					waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
				}
			}
		}
	}

	private void determineScaredState() {
		if (scared) {
			if ((Time.time - scaredTimer) >= scaredLength) {
				scared = false;
				this.sprinting = false;
				this.recentlyHit = false;
				scaredTimer = 0.0f;
			}
		} else {
			if (this.recentlyHit == true) {
				scared = true;
				this.sprinting = true;
				scaredTimer = Time.time;
			}
		}
	}

	private void determineStuckState() {
		if (Vector3.Distance(lastSpot, this.body.transform.position) >= threshold / 2) {
			lastSpot = this.body.transform.position;
			lastTimeOnSpot = Time.time;
			isStuck = false;
		} else {
			if (!isStuck) {
				if (Time.time - lastTimeOnSpot >= maxTimeOnSpot) {
					this.resetMovement();
					findTemporaryTarget();
					active = false;
					isStuck = true;
				}
			}
		}
	}

	private void findTemporaryTarget() {
		target = new Vector3(
			-target.x + Random.Range(-threshold, threshold),
			target.y,
			-target.z + Random.Range(-threshold, threshold)
		);
	}

	private void determineActiveState() {
		if (!active) {
			float closest = 9999.0f;
			Transform output = null;
			foreach (GameObject point in waypoints) {
				float distance = Vector3.Distance(point.transform.position, this.body.transform.position);
				if (distance <= closest) {
					if (recentWaypoints.Contains(point.transform)) {
						continue;
					}
					closest = distance;
					output = point.transform;
				}
			}
			waypoint = output;
			target = new Vector3(
				waypoint.position.x + Random.Range(-threshold, threshold),
				body.transform.position.y,
				waypoint.position.z + Random.Range(-threshold, threshold)
			);
			recentWaypoints.Add(waypoint);
			active = true;
		} else {
			float distance = Vector3.Distance(target, this.body.transform.position);
			if (distance <= threshold) {
				active = false;
			}
		}
	}

	private void turnHead() {
		Vector3 heading = target - this.head.transform.position;
		this.turnHead(heading);
	}

	private void triggerMovement() {
		Vector3 heading = Vector3.Normalize(target - this.body.transform.position);
	
//		Debug.DrawRay(this.body.transform.position, this.body.transform.forward * threshold, Color.red);
//		Debug.DrawRay(this.body.transform.position, heading * Vector3.Distance(target, this.body.transform.position), Color.green);
//		Debug.DrawRay(this.body.transform.position, Vector3.Normalize(waypoint.position - this.body.transform.position) * distance, Color.blue);

		if (isStuck) {
			this.move (-this.body.transform.forward);
		} else {
			this.move(this.body.transform.forward);
		}
		this.turnBody(heading);
	}

	private void refreshRecentWaypoints() {
		while (recentWaypoints.Count > maxRecentWaypoints) {
			recentWaypoints.RemoveAt(0);
		}
	}
}
