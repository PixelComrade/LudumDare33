using UnityEngine;
using System.Collections;

public class WaypointAndNPCSpawner : MonoBehaviour {

	public Transform startingPoint = null;
	
	public GameObject waypoint = null;
	public GameObject waypointParent = null;

	public GameObject npc = null;
	public GameObject npcParent = null;

	public float npcSpawnY = -0.45f;
	
	public float chanceForSpawn = 80.0f;
	public float npcsPerWaypoint = 4.0f;
	
	public float spawnIncrement = 12.0f;
	
	public int densityIncrement = 10;
	public int density = 6;
	
	public int maxIterations = 15;
	
	private float radiusX = 5.0f;
	private float radiusZ = 5.0f;
	
	private int counter = 0;

	private bool active = false;
	private bool completed = false;
	
	void FixedUpdate () {
		if (active && completed == false) {
			if (counter <= maxIterations) {
				spawnAroundArea();
				incrementSpawnArea();
				counter++;
			} else {
				completed = true;
			}
		}
	}

	public void activateSpawner() {
		active = true;
	}

	public bool getCompleted() {
		return completed;
	}
	
	private void incrementSpawnArea() {
		radiusX += spawnIncrement;
		radiusZ += spawnIncrement;
		density += densityIncrement;
	}
	
	private void spawnAroundArea() {
		for (int i = 0; i < density; i++) {
			// "i" now represents the progress around the circle from 0-1
			// we multiply by 1.0 to ensure we get a fraction as a result.
			float counter = (i * 1.0f) / density;
			// get the angle for this step (in radians, not degrees)
			var angle = counter * Mathf.PI * 2;
			// the X & Y position for this angle are calculated using Sin & Cos
			float x = Mathf.Sin(angle) * radiusX;
			float z = Mathf.Cos(angle) * radiusZ;
			Vector3 pos = new Vector3(x, 0, z) + startingPoint.transform.position;
			spawnElement(pos);
		}
	}
	
	private void spawnElement(Vector3 position) {
		float result = Random.Range(0, 100);
		
		if (result < chanceForSpawn) {
			GameObject newWaypoint = GameObject.Instantiate(waypoint, position, Quaternion.identity) as GameObject;
			newWaypoint.transform.parent = waypointParent.transform;

			for (int i = 0; i < npcsPerWaypoint; i++) {
				result = Random.Range(0, 100);
				if (result < chanceForSpawn) {
					spawnNpc(position);
				}
			}
		}
	}

	private void spawnNpc(Vector3 position) {
		float variance = density / 2;

		Vector3 spawnPosition = new Vector3(
			position.x + Random.Range(-variance, variance),
			npcSpawnY,
			position.z + Random.Range(-variance, variance)
		);

		GameObject newNpc = GameObject.Instantiate(npc, spawnPosition, Quaternion.identity) as GameObject;
		newNpc.transform.parent = npcParent.transform;
	}
}
