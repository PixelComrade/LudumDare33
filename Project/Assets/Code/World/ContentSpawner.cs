using UnityEngine;
using System.Collections;

public class ContentSpawner : MonoBehaviour {

	public Transform startingPoint = null;

	public GameObject briefcase = null;
	public GameObject[] houseTemplates = new GameObject[0];
	public GameObject houseParent = null;
	public GameObject tree = null;
	public GameObject treeParent = null;

	public float houseSpawnY = 0.0f;
	public float treeSpawnY = 0.09999996f;

	public float chanceForHouse = 50.0f;
	public float chanceForTree = 50.0f;

	public float spawnIncrement = 12.0f;

	public int densityIncrement = 8;
	public int density = 4;

	public int maxIterations = 15;

	private float radiusX = 10.0f;
	private float radiusZ = 10.0f;

	private int counter = 0;

	private bool active = false;
	private bool completed = false;

	private bool spawnedBriefcase = false;

	void Awake() {
		chanceForTree += chanceForHouse;
	}

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
			float progress = (i * 1.0f) / density;
			// get the angle for this step (in radians, not degrees)
			var angle = progress * Mathf.PI * 2;
			// the X & Y position for this angle are calculated using Sin & Cos
			float x = Mathf.Sin(angle) * radiusX;
			float z = Mathf.Cos(angle) * radiusZ;
			Vector3 pos = new Vector3(x, 0, z) + startingPoint.transform.position;
			spawnElement(pos);
		}
	}

	private void spawnElement(Vector3 position) {
		float result = Random.Range(0, 100);

		if (result < chanceForHouse) {
			GameObject chosenTemplate = null;
			for (int i = 0; i < houseTemplates.Length; i++) {
				if (Random.Range(0, 10) >= 5) {
					chosenTemplate = houseTemplates[i];
					break;
				}
				if (i == houseTemplates.Length - 1) {
					chosenTemplate = houseTemplates[i];
				}
			}
			position.y = houseSpawnY;
			GameObject house = GameObject.Instantiate(chosenTemplate, position, Quaternion.identity) as GameObject;
			house.transform.forward = Vector3.Normalize(startingPoint.position - position);
			house.transform.parent = houseParent.transform;
			if (!spawnedBriefcase) {
				if (counter == maxIterations) {
					spawnBriefcase(house);
				} else {
					attemptToSpawnBriefcase(house);
				}
			}
		} else if (result < chanceForTree) {
			position.y = treeSpawnY;
			GameObject newTree = GameObject.Instantiate(tree, position, Quaternion.identity) as GameObject;
			newTree.transform.parent = treeParent.transform;
		}
	}

	private void attemptToSpawnBriefcase(GameObject inside) {
		if (counter >= 5 && Random.Range (0, 10) > 7) {
			spawnBriefcase(inside);
		} else if (counter >= 7 && Random.Range (0, 10) > 4) {
			spawnBriefcase(inside);
		}
	}

	private void spawnBriefcase(GameObject inside) {
		Transform reference = inside.transform.FindChild("Reference");
		if (reference != null) {
			GameObject.Instantiate(briefcase, reference.transform.position, Quaternion.identity);
			spawnedBriefcase = true;
		}
	}
}
