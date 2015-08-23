using UnityEngine;
using System.Collections;

public class Briefcase : MonoBehaviour {

	public WinLose player = null;
	
	void Awake () {
		player = GameObject.Find("Player").GetComponent<WinLose>() as WinLose;
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.gameObject.tag == "PC" && collision.collider.transform.name == "Player") {
			player.setBriefcaseFound();
			Destroy(this.gameObject);
		}
	}
}
