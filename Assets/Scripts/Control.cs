using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Control : MonoBehaviour {

	public GameObject projectile;

	public GameObject engine;

	public float maxSpeed = 6f;
	public float acceleration = 10f;

	public float turnRate = 200f;

	public float fireDelay = 0.1f;
	public float reloadInterval = 4.4f;
	private float reloadTime;
	private float fireTime;

	public float size = 0.8f;

	public float gunpoint = 0.45f;

	public List<Reloader> ammoSlots;

	// to prevent killing multiple asteroids with one hit.
	// Spawning them on the next frame would also solve this
	private bool isColliding = false;

	public AudioClip shotSound;
	public AudioClip deathSound;

	// Use this for initialization
	void Start () {
		foreach (Reloader r in ammoSlots) {
			r.reloadInterval = reloadInterval;
		}
		reloadTime = -reloadInterval - 1f;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameFlow.instance.state == GameFlow.State.Pause) return;
		isColliding = false;
		if (Input.GetKey ("left")) transform.Rotate(0,0, turnRate * Time.deltaTime);
		if (Input.GetKey ("right")) transform.Rotate(0,0, -turnRate * Time.deltaTime);
		if (Input.GetKey ("up")) { 
			rigidbody.velocity += acceleration * Time.deltaTime * transform.up;
			engine.SetActive(true);
		} else {
			engine.SetActive(false);
		}
		if (Time.time > reloadTime + reloadInterval && !FullyLoaded()) {
			if (StartReload()) {
				reloadTime = Time.time;
			}
		}
		if (Input.GetKey ("space") && Time.time > fireTime + fireDelay) {
			if (Shoot()) {
				fireTime = Time.time;
			}
		}
		if (rigidbody.velocity.magnitude > maxSpeed) rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
		
		transform.position = new Vector3(-World.width/2 - size + Mathf.Repeat(transform.position.x + World.width/2 + size, World.width + size * 2),
		                                 -World.height/2 - size + Mathf.Repeat(transform.position.y + World.height/2 + size, World.height + size * 2),
		                                 transform.position.z);
	}

	bool Shoot () {
		Reloader next = ammoSlots[0];
		float oldestReload = Time.time;
		foreach (Reloader r in ammoSlots) {
			if (r.reloadTime < oldestReload && r.state == Reloader.State.loaded) {
				oldestReload = r.reloadTime;
				next = r;
			}
		}
		if (next.Fire()) {
			// AudioSource.PlayClipAtPoint(shotSound, transform.position);
			GameObject shot = (GameObject) Instantiate(projectile,transform.position + transform.up * gunpoint, transform.rotation);
			shot.rigidbody.velocity = rigidbody.velocity;
			shot.SendMessage("Fire");
			return true;
		} else return false;
	}

	bool StartReload() {
		Reloader next = ammoSlots[0];
		float oldestReload = Time.time;
		foreach (Reloader r in ammoSlots) {
			if (r.reloadTime < oldestReload && r.state == Reloader.State.empty) {
				oldestReload = r.reloadTime;
				next = r;
			}
		}
		return next.Reload();
	}

	bool FullyLoaded() {
		int loaded = 0;
		foreach (Reloader r in ammoSlots) {
			if (r.state == Reloader.State.loaded) loaded++;
		}
		return (loaded == ammoSlots.Count);
	}

	void OnTriggerEnter(Collider other) {
		if(isColliding) return;
		isColliding = true;
		Transform current = other.transform;
		while (current != null) {
			if (current.tag == "Asteroid") {
				// AudioSource.PlayClipAtPoint(deathSound, transform.position);
				current.SendMessage("Kill");
				World.instance.SendMessage("PlayerDied");
				Destroy(gameObject);
			}
			current = current.parent;
		}
	}

}
