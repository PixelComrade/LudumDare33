using UnityEngine;
using System.Collections;

public class TreeCreator : MonoBehaviour {
    
    public GameObject trunk   = null;
    public GameObject foliage = null;
    
    public float variance     = 1.0f;
    public int treeHeight     = 3;
    public int foliageDensity = 3;
    
    void Awake () {
        treeHeight     = Random.Range(treeHeight - (int)variance, treeHeight + (int)variance);
        foliageDensity = Random.Range(foliageDensity - (int)variance, foliageDensity + (int)variance);
        
        GameObject created = trunk;
        for (int i = 0; i < treeHeight; i++) {
            Vector3 placement = placeTrunk(created);
            created = spawnTrunk(created, placement);
        }

        int counter = 0;
        foreach (Transform child in this.transform) {
            if (child.name != "Base") {
                if (counter >= 1) {
                    int heightLevelDensity = foliageDensity + Random.Range(0, counter);
                    for (int i = 0; i < heightLevelDensity; i++) {
                        Vector3 placement = placeFoliage(child.gameObject);
                        spawnFoliage(child.gameObject, placement);
                    }
                }
                counter++;

                if (child.GetSiblingIndex() + 1 >= this.transform.childCount) {
                    Vector3 finalPosition = child.transform.position;
                    finalPosition.y = child.transform.position.y + (child.transform.localScale.y / 2);
                    spawnFoliage(child.gameObject, finalPosition);
                }
            }
        }
    }

    private Vector3 placeTrunk (GameObject origin) {
        Vector3 position = new Vector3(
            origin.transform.position.x,
            origin.transform.position.y + (trunk.transform.localScale.y * 2),
            origin.transform.position.z
        );

        return position;
    }
    
    private GameObject spawnTrunk (GameObject origin, Vector3 position) {
        GameObject created = GameObject.Instantiate(
            trunk,
            position,
            Quaternion.identity
        ) as GameObject;
        
        created.transform.parent = this.transform;
        
        FixedJoint joint = created.AddComponent<FixedJoint>() as FixedJoint;
        joint.connectedBody = origin.GetComponent<Rigidbody>() as Rigidbody;
        
        return created;
    }

    private Vector3 placeFoliage (GameObject origin) {
        // TODO - Add some variation to the foliage size too
//        spawnFoliage.transform.localScale.x = spawnFoliage.transform.localScale.x * Random.Range(-variance, variance);
//        spawnFoliage.transform.localScale.y = spawnFoliage.transform.localScale.y * Random.Range(-variance, variance);
//        spawnFoliage.transform.localScale.z = spawnFoliage.transform.localScale.z * Random.Range(-variance, variance);

        Vector3 position = new Vector3(
            origin.transform.position.x + Random.Range(-variance, variance),
            origin.transform.position.y + Random.Range(-variance, variance) * 0.5f,
            origin.transform.position.z + Random.Range(-variance, variance)
        );
        // TODO - The variance in foliage position should not exceed the localScale of said foliage

        return position;
    }

    private GameObject spawnFoliage (GameObject origin, Vector3 location) {
        GameObject created = GameObject.Instantiate(
            foliage,
            location,
            Quaternion.identity
        ) as GameObject;

        created.transform.parent = origin.transform;

        return created;
    }
}
