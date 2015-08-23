using UnityEngine;
using System.Collections;

public class autoDestroyParticles : MonoBehaviour {

	public ParticleSystem particles = null;
	
	void FixedUpdate () {
		if (!particles.IsAlive()) {
			Destroy(this.gameObject);
		}
	}
}
