using UnityEngine;
using System.Collections;

public class SpawnTrigger : MonoBehaviour {

	public ContentSpawner content = null;
	public WaypointAndNPCSpawner waypointAndNPC = null;

	void Awake() {
		content.activateSpawner();
	}

	void FixedUpdate() {
		if (content.getCompleted() == true) {
			waypointAndNPC.activateSpawner();
		}
	}
}
