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

	public AudioClip explosionSound; 

	public float rotation = 0f;

	void Start() {
		spawned = false;
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(-World.width/2 - size + Mathf.Repeat(transform.position.x + World.width/2 + size, World.width + size * 2),
		                                 -World.height/2 - size + Mathf.Repeat(transform.position.y + World.height/2 + size, World.height + size * 2),
		                                 transform.position.z);
		transform.Rotate(0f, 0f, rotation * Time.deltaTime);
	}

	public void Generate() {
		if (spawned) return;
		spawned = true;
		size = level / 4f;
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
		rotation = Random.Range(-20f, 20f);

	}

	void Kill () {
		// AudioSource.PlayClipAtPoint(explosionSound, transform.position);
		World.instance.hitCount += 1;
		AudioManager.Progress();
		World.instance.Asteroids.Remove(gameObject);
		if (level > 1) {
			World.instance.SpawnChildren(level, hits, transform.position, speed);
		}
		Destroy (gameObject);
	}
}
