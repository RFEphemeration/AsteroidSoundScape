using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float timeOut = 1f;

	public float speed = 10f;

	public float size = 0.1f;
	
	private float startTime;

	// to prevent killing multiple asteroids with one hit.
	// Spawning them on the next frame would also solve this
	private bool isColliding = false;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}

	public void Fire () {
		startTime = Time.time;
		rigidbody.velocity += transform.up * speed;
		rigidbody.velocity = rigidbody.velocity.normalized * speed;
	}
	
	// Update is called once per frame
	void Update () {
		isColliding = false;
		if (Time.time - startTime > timeOut) Destroy(gameObject);
		transform.position = new Vector3(-World.width/2 - size + Mathf.Repeat(transform.position.x + World.width/2 + size, World.width + size * 2),
		                                 -World.height/2 - size + Mathf.Repeat(transform.position.y + World.height/2 + size, World.height + size * 2),
		                                 transform.position.z);
	}

	void OnTriggerEnter(Collider other) {
		if(isColliding) return;
		Transform current = other.transform;
		while (current != null) {
			if (current.tag == "Asteroid") {
				isColliding = true;
				current.SendMessage("Kill");
				Destroy(gameObject);
			}
			current = current.parent;
		}
	}
}
