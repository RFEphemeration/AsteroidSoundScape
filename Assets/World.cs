using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {
	
	public static float width = 10f;
	public static float height = 10f;

	public int startingMass = 15;
	public int maxMass = 5;

	public float maxSpeed = 4f;
	public float minSpeed = 2f;

	public GameObject aster;

	public float spawnDistance = 0.1f;

	void Start () {
		height = Camera.main.orthographicSize * 2;

		float aspectRatio = (float)Screen.width / (float)Screen.height;
		width = height * aspectRatio;
		SpawnAsteroids();
	}

	void SpawnAsteroids () {
		GameObject spawn;
		Asteroid child;
		int remainingMass = startingMass;
		while (remainingMass > 0) {
			spawn = (GameObject) Instantiate(aster,
			                                 transform.position,
			                                 Quaternion.Euler(0,0, Random.Range(0f, 360f)));
			child = spawn.GetComponent<Asteroid>();
			child.mass = Random.Range(1, Mathf.Min(remainingMass, maxMass));
			remainingMass -= child.mass;
			child.speed = Random.Range(minSpeed, maxSpeed * child.mass / maxMass);
			child.SendMessage("Generate");
		}
	}

	public void SpawnChildren (int mass, Vector3 pos, float speed) {
		GameObject spawn;
		Asteroid child;
		int remainingMass = mass;
		while (remainingMass > 0) {
			spawn = (GameObject) Instantiate(aster,
			                                 pos,
			                                 Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
			spawn.transform.position += spawn.transform.up * (float)mass * spawnDistance * Random.value;
			child = spawn.GetComponent<Asteroid>();
			child.mass = Random.Range(1, remainingMass);
			remainingMass -= child.mass;
			child.speed = Random.Range(0.1f, speed * child.mass / mass);
			child.SendMessage("Generate");
		}
	}

	
	private static World _instance;
	
	public static World instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<World>();
				
				//Tell unity not to destroy this object when loading a new scene!
				DontDestroyOnLoad(_instance.gameObject);
			}
			
			return _instance;
		}
	}
	
	void Awake() 
	{
		if(_instance == null)
		{
			//If I am the first instance, make me the Singleton
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != _instance)
				Destroy(this.gameObject);
		}
	}

}
