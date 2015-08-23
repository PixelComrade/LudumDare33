using UnityEngine;
using System.Collections;

public class FoliageToggle : MonoBehaviour {

	public GameObject player = null;
	public ParticleSystem foliage = null;
	public float chance = 1.0f;
	public float distanceToSee = 20.0f;

	private bool emitting = false;
	
	void Awake () {
		if (Random.Range(0, 10) <= chance) {
			emitting = true;
			player = GameObject.Find("Player");
		} else {
			foliage.enableEmission = false;
		}
	}

	void FixedUpdate() {
		if (Time.time > 5 && emitting) {
			if (Vector3.Distance(player.transform.position, this.transform.position) <= distanceToSee) {
				foliage.enableEmission = true;
			} else {
				foliage.enableEmission = false;
			}
		}
	}
}
