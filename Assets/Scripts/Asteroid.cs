using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroid : MonoBehaviour {

	public int level = 3;
	public int hits = 13;

	public float speed = 5f;

	public List<GameObject> views = new List<GameObject>();

	public bool spawned = false;

	public GameObject aster;

	public AudioClip explosionSound; 

	public float rotation = 0f;

	public bool isColliding = false;

	void Start() {
		spawned = false;
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate(0f, 0f, rotation * Time.deltaTime);
		isColliding = false;
	}

	public void Generate() {
		if (spawned) return;
		spawned = true;
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
		if(gameObject.GetComponentInChildren<Wrapper>().index >= 0) {
			gameObject.GetComponentInChildren<Wrapper>().original.SendMessage("Kill");
			return;
		}
		if(isColliding) return;
		isColliding = true;
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
