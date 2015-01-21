using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroid : MonoBehaviour {

	public int level = 3;
	public int hits = 13;

	public float size = 2f;

	public float speed = 5f;

	public List<GameObject> views = new List<GameObject>();

	public bool spawned = false;

	public GameObject aster;

	void Start() {
		spawned = false;
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(-World.width/2 - size + Mathf.Repeat(transform.position.x + World.width/2 + size, World.width + size * 2),
		                                 -World.height/2 - size + Mathf.Repeat(transform.position.y + World.height/2 + size, World.height + size * 2),
		                                 transform.position.z);
	}

	public void Generate() {
		if (spawned) return;
		spawned = true;
		size = level / 2f;
		List<GameObject> legalViews = new List<GameObject>();
		foreach (GameObject v in views) {
			if (v.tag == level.ToString()) {
				legalViews.Add (v);
			}
		}
		GameObject view = (GameObject) Instantiate(legalViews[Random.Range(0, legalViews.Count)], transform.position, transform.rotation);
		// HACK to display correct asteroid type. may be problematic later if other children are necessary
		view.transform.parent = transform;
		view.transform.localEulerAngles.Set (Random.Range (0f, 360f), Random.Range (0f, 360f), Random.Range (0f, 360f));
		rigidbody.velocity = transform.up * speed;

	}

	void Kill () {
		World.instance.hitCount += 1;
		/*
		GameObject spawn;
		Asteroid child;
		int remainingMass = mass;
		if (remainingMass > 1) {
			while (remainingMass > 0) {
				spawn = (GameObject) Instantiate(aster,
				                                 transform.position + new Vector3(Random.Range(-spawnDistance, spawnDistance), Random.Range (-spawnDistance, spawnDistance), 0),
				                                 Quaternion.Euler(0,0, Random.Range(0f, 360f)));
				Debug.Log ("instantiated");
				child = spawn.GetComponent<Asteroid>();
				child.mass = Random.Range(1, remainingMass);
				remainingMass -= child.mass;
				child.speed = Random.Range(0.1f, speed * child.mass / mass);
				child.SendMessage("Generate");
			}
		}
		*/
		if (level > 1) {
			World.instance.SpawnChildren(level, hits, transform.position, speed);
		}
		Destroy (gameObject);
	}
}
