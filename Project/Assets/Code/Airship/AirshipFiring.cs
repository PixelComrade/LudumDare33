using UnityEngine;
using System.Collections;

public class AirshipFiring : Airship {

	public GameObject[] ports = new GameObject[0];
	public GameObject projectilePrefab = null;
	public GameObject shootEffect = null;
	public float fireRate = 5.0f;
	public float variance = 5.0f;

	private float timePassed = 0.0f;
	private bool firedBefore = false;

	void Awake () {
		timePassed = Time.time;
	}

	void FixedUpdate () {
//		foreach (GameObject port in ports) {
//			Debug.DrawRay(port.transform.position, port.transform.forward * 100.0f, Color.red);
//		}

		if (Time.time - timePassed >= fireRate) {
			refreshNpcList();

			foreach (GameObject port in ports) {
				if (Random.Range(0, 10) >= variance) {
					aim(port);
					fire(port);
				}
			}
			if (!firedBefore) {
				int current = Random.Range(0, ports.Length - 1);
				aim(ports[current]);
				fire(ports[current]);
			} else {
				firedBefore = false;
			}
			timePassed = Time.time;
		}
	}

	private void refreshNpcList() {
		GameObject[] everyNpc = GameObject.FindGameObjectsWithTag("NPC");
		foreach (GameObject npc in everyNpc) {
			if (npc.name == "Person(Clone)") {
				if (!this.npcs.Contains(npc)) {
					this.npcs.Add(npc);
				}
			}
		}
	}

	private Vector3 getAverageTarget() {
		float x = this.player.transform.position.x;
		float y = this.player.transform.position.y;
		float z = this.player.transform.position.z;
		foreach (GameObject npc in this.npcs) {
			x += npc.transform.position.x;
			y += npc.transform.position.x;
			z += npc.transform.position.x;
		}
		int count = this.npcs.Count + 1;
		return new Vector3(x / count, y / count, z / count);
	}

	private void aim(GameObject port) {
		Vector3 target = Vector3.zero;

		if (Random.Range(0, 10) > 7) {
			target = getAverageTarget();
			target = target - port.transform.position;
		} else {
			if (Random.Range(0, 10) > 2) {
				int selected = Random.Range(0, this.npcs.Count - 1);
				target = this.npcs[selected].transform.position - port.transform.position;
			} else {
				target = this.player.transform.position - port.transform.position;
			}
		}

		target = new Vector3(
			target.x + Random.Range(-variance * 2, variance * 2),
			target.y,
			target.z + Random.Range(-variance * 2, variance * 2)
		);

		target = Vector3.Normalize(target);

		target.y += 0.3f * target.magnitude; // 0.3f * target.magnitude * 1.33f;

		port.transform.forward = target;
	}

	private void fire(GameObject port) {
		GameObject shot = GameObject.Instantiate(projectilePrefab, port.transform.position, Quaternion.identity) as GameObject;
		shot.transform.forward = port.transform.forward;

		GameObject shotEffect = GameObject.Instantiate(shootEffect, port.transform.position, Quaternion.identity) as GameObject;
		shotEffect.transform.forward = port.transform.forward;
		
		firedBefore = true;
	}
}
