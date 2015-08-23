using UnityEngine;
using System.Collections;

public class ThrowWave : MonoBehaviour {

	public GameObject wave = null;
	public Transform spawn = null;

	public int maxWaves = 3;

	private int waveCount = 0;

	private bool thrown = false;
	private float timeSinceLastThrow = 0.0f;

	void Awake() {
		timeSinceLastThrow = Time.time;
	}

	void FixedUpdate () {
		if (Input.GetButtonDown("Fire") && !thrown) {
			if (waveCount < maxWaves) {
				throwWave();
				timeSinceLastThrow = Time.time;
				thrown = true;
			}
		}

		if (Time.time - timeSinceLastThrow > 1.5f) {
			thrown = false;
		}
	}

	public void notifyOwnerOfSpawn() {
		waveCount++;
	}

	public void notifyOwnerOfDeath() {
		waveCount--;
	}

	private void throwWave() {
		GameObject newWave = GameObject.Instantiate(wave, spawn.position, Quaternion.identity) as GameObject;

		newWave.transform.forward = spawn.forward;
		newWave.GetComponent<Wave>().owner = this;

		notifyOwnerOfSpawn();
	}
}
