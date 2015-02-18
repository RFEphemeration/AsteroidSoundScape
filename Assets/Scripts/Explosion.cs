using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explosion : MonoBehaviour {

	public GameObject bit;

	public float startingRadius = 0.1f;

	public float maxScale = 0.4f;
	public float minScale = 0.05f;

	public float maxVelocity = 3f;
	public float minVelocity = 1f;

	public float maxRotation = 40f;

	public float time = 1f;

	float startTime = -1f;

	public int bits = 4;

	public Vector3 center = Vector3.zero;

	List<GameObject> bitObjects = new List<GameObject>();

	void Start() {
		center = transform.position;
		Explode ();
	}

	// Update is called once per frame
	void Update () {
		if (startTime < 0f) return;
		foreach (Transform t in transform) {
			t.localScale = new Vector3(1f, (1f-Fade.Ease((Time.time - startTime)/time)), 1f);
		}
		if (Time.time - startTime > time) {
			Destroy (gameObject);
		}
	
	}

	void Explode() {
		startTime = Time.time;
		Vector3 pos;
		Quaternion rot;
		for(int i = 0; i < bits; i++) {
			pos = center;
			pos += Vector3.right * Random.Range (-startingRadius, startingRadius);
			pos += Vector3.up * Random.Range (-startingRadius, startingRadius);
			rot = Quaternion.LookRotation(pos - center);
			GameObject g = (GameObject) Instantiate(bit, pos, rot);
			bitObjects.Add(g);
			Rigidbody r = g.rigidbody;
			foreach (Transform t in g.transform) {
				t.localScale = new Vector3 (t.localScale.x, t.localScale.y * Random.Range(0.2f, startingRadius * 3f), t.localScale.z);
			}
			r.angularVelocity = (Vector3.forward * Random.Range(-maxRotation, maxRotation));
			r.velocity = (Random.value - 0.5f) * Vector3.up + (Random.value - 0.5f) * Vector3.right;
			r.velocity = r.velocity.normalized * Random.Range (minVelocity, maxVelocity);
			r.transform.parent = transform;
		}
	}
}
