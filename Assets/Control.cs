using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {

	public GameObject projectile;

	public GameObject engine;

	public float maxSpeed = 6f;
	public float acceleration = 10f;

	public float turnRate = 200f;

	public float fireDelay = 0.1f;
	private float fireTime;

	public float size = 0.8f;

	public float gunpoint = 0.45f;

	private bool isColliding = false;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("left")) transform.Rotate(0,0, turnRate * Time.deltaTime);
		if (Input.GetKey ("right")) transform.Rotate(0,0, -turnRate * Time.deltaTime);
		if (Input.GetKey ("up")) { 
			rigidbody.velocity += acceleration * Time.deltaTime * transform.up;
			engine.SetActive(true);
		} else {
			engine.SetActive(false);
		}

		if (Input.GetKey ("space")) {
			if (Time.time > fireTime) {
				fireTime = Time.time + fireDelay;
				Shoot ();
			}
		}

		if (rigidbody.velocity.magnitude > maxSpeed) rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
		
		transform.position = new Vector3(-World.width/2 - size + Mathf.Repeat(transform.position.x + World.width/2 + size, World.width + size * 2),
		                                 -World.height/2 - size + Mathf.Repeat(transform.position.y + World.height/2 + size, World.height + size * 2),
		                                 transform.position.z);
	}

	void Shoot () {
		GameObject shot = (GameObject) Instantiate(projectile,transform.position + transform.up * gunpoint, transform.rotation);
		shot.rigidbody.velocity = rigidbody.velocity;
		shot.SendMessage("Fire");
	}

	void OnTriggerEnter(Collider other) {
		if(isColliding) return;
		isColliding = true;
		Transform current = other.transform;
		while (current != null) {
			if (current.tag == "Asteroid") {
				current.SendMessage("Kill");
				// TODO: write respawn
				Destroy(gameObject);
			}
			current = current.parent;
		}
	}

}
