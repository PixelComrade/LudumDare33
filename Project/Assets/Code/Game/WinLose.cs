using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinLose : MonoBehaviour {

	public Text playerObjective = null;
	public Image healthMeter    = null;
	public Image crosshair		= null;
	public Text playerScore		= null;
	public Image endGameOverlay	= null;
	public Text endGame			= null;

	public Rigidbody playerBody = null;
	public GameObject spawn		= null;
	public GameObject airship	= null;

	public Sprite healthFull    = null;
	public Sprite healthHalf    = null;

	public bool inMenu			= true;

	private float score = 0.0f;

	private bool foundBriefcase = false;
	private bool farEnough 		= false;
	private bool recentlyHit	= false;

	private float timeLastHit   = 0.0f;
	private float spawnDistance	= 0.0f;

	private string needBriefcase = "My blue briefcase, I have to find it!";
	private string needToLeave	 = "Alright, time to leave!";

	private int health = 2;
	
	void Awake () {
		playerScore.text = "Score: " + score;
		playerObjective.text = needBriefcase;
	}

	void FixedUpdate () {
		if (inMenu) {
			if (Input.GetButtonDown("Fire")) {
				inMenu = false;
				beginGame();
			}
		} else {
			checkForWin();
			checkForLoss();
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.relativeVelocity.magnitude >= 100.0f) {
			healthMeter.sprite = healthHalf;
			health--;
		}
	}

	public void setBriefcaseFound() {
		foundBriefcase = true;
		foundBriefcase = true;
		playerObjective.text = needToLeave;
	}

	public void setRecentHit() {
		recentlyHit = true;
		timeLastHit = Time.time;
	}

	public void updateScore(float change) {
		score += change;
		playerScore.text = "Score: " + score;
	}

	public void beginGame() {
		playerObjective.gameObject.SetActive(true);
		healthMeter.gameObject.SetActive(true);
		crosshair.gameObject.SetActive(true);
		
		endGameOverlay.gameObject.SetActive(false);
		endGame.gameObject.SetActive(false);

		airship.SetActive(true);
	}

	public void finishGame(string message) {
		playerObjective.gameObject.SetActive(false);
		healthMeter.gameObject.SetActive(false);
		crosshair.gameObject.SetActive(false);

		endGame.text = message;

		endGameOverlay.gameObject.SetActive(true);
		endGame.gameObject.SetActive(true);

		airship.SetActive(false);
		// TODO - Assign this to a more steady variable
		this.gameObject.GetComponent<ThrowWave>().enabled = false;
	}

	private void checkForWin() {
		spawnDistance = Vector3.Distance(spawn.transform.position, this.transform.position);
		if (spawnDistance >= 200.0f) {
			farEnough = true;
		}

		if (foundBriefcase == true && farEnough == true) {
			finishGame("Made it out alive and in one piece. Solid work!");
		}
	}

	private void checkForLoss() {
		if (recentlyHit) {
			if (playerBody.velocity.magnitude >= 150.0f) {
				recentlyHit = false;
				healthMeter.sprite = healthHalf;
				health--;
			} else {
				if (Time.time - timeLastHit >= 2.0f) {
					recentlyHit = false;
				}
			}
		}

		if (health <= 0) {
			finishGame("You just took a cannonball to the face. You're out.");
		}

		if (foundBriefcase == false && farEnough == true) {
			finishGame("Not without the briefcase fool! That was important!");
		}
	}
}
