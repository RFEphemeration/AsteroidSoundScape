using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject player;
	
	public static float width = 10f;
	public static float height = 10f;

	public static float maxDist;

	public int maxLevel = 3;

	public int[,] levelHits = new int[4,2] {{1,1},{3,4},{7,13},{15,40}};

	public int startingHits = 52;

	public float maxSpeed = 4f;
	public float minSpeed = 2f;

	public GameObject aster;

	public float spawnDistance = 0.1f;

	public bool respawning = false;
	public float respawnTime;
	public float respawnDelay = 2f;
	public float softSafeDistance = 2.5f;
	public float hardSafeDistance = 1f;

	public List<GameObject> Asteroids = new List<GameObject>();

	public float shortTermChange = 1f;
	public int hitCount = 0;
	public int deathCount = 0;
	
	void Start () {
	}

	void Update() {
		if (GameFlow.instance.state != GameFlow.State.Play) return;
		if (respawning && Time.time > respawnTime) {
			TryRespawnPlayer();
		}
		if (respawning) {
			shortTermChange = 0f;
		} else {
			float minDistance = maxDist;
			foreach(GameObject a in Asteroids) {
				float dist = (a.transform.position - player.transform.position).magnitude;
				if (dist < minDistance) minDistance = dist;
			}
			shortTermChange = (maxDist - minDistance)/maxDist;
		}
		if (hitCount >= startingHits) {
			GameFlow.instance.EndGame();
		}
	}

	void SpawnAsteroids () {
		GameObject spawn;
		Asteroid child;
		int remainingHits = startingHits;
		Control ship = playerPrefab.GetComponent<Control>();
		/* Trying to divide in interesting ways, instead of naiive 
		int[] hits = new int[startingHits/levelHits[maxLevel][0]];
		int[] newhits;
		bool flag = false;
		do {
			newhits = new int[hits.Length-1];
			Array.Copy(hits, 0, newhits, 0, newhits.Length);
			int remainder = hits[hits.Length];
			while (remainder > 0 && !flag) {
				foreach (int a in newhits) {
					if (a < levelHits[maxLevel][0]) {
						a++;
						remainder--;
					} else {
						flag = true;
					}
				}
			}
			if (!flag && remainder == 0) {
				hits = newhits;

			} else if (!flag && remainder > 0) {
				
			}
		} while (!flag);
		*/
		while (remainingHits > 0) {
			Vector3 pos = Vector3.zero;
			while (pos.magnitude < ship.size + maxLevel / 2f) {
				pos = new Vector3(Random.Range(0, World.width) - World.width/2 , Random.Range(0, World.height) - World.height/2, 0);
			}
			spawn = (GameObject) Instantiate(aster,
			                                 pos,
			                                 Quaternion.Euler(0,0, Random.Range(0f, 360f)));
			child = spawn.GetComponent<Asteroid>();
			child.hits = 0;
			while (child.hits == 0) {
				if (remainingHits > levelHits[maxLevel-1,1]) {
					child.hits = levelHits[maxLevel-1,1];
				} else if (remainingHits >= levelHits[maxLevel-1,0]) {
					child.hits = remainingHits;
				} else {
					maxLevel -= 1;
				}
			}
			remainingHits -= child.hits;
			child.level = 1;
			while (levelHits[child.level-1,1] < child.hits) {
				child.level++;
			}
			child.speed = Random.Range(minSpeed, maxSpeed * child.level / maxLevel);
			child.SendMessage("Generate");
			Asteroids.Add(spawn);
		}
	}

	public void SpawnChildren (int level, int hits, Vector3 pos, float speed) {
		level -= 1;
		GameObject spawn;
		Asteroid child;
		int remainingHits = hits - 1;
		while (remainingHits > 0) {
			spawn = (GameObject) Instantiate(aster,
			                                 pos,
			                                 Quaternion.Euler(0,0, Random.Range(0f, 360f)));
			child = spawn.GetComponent<Asteroid>();
			child.hits = 0;
			while (child.hits == 0) {
				if (remainingHits > levelHits[level-1,1]) {
					child.hits = levelHits[level-1,1];
				} else if (remainingHits >= levelHits[level-1,0]) {
					child.hits = remainingHits;
				} else {
					level -= 1;
				}
			}
			remainingHits -= child.hits;
			child.level = 1;
			while (levelHits[child.level-1,1] < child.hits) {
				child.level++;
			}
			child.speed = Random.Range(minSpeed, maxSpeed * child.level / maxLevel);
			child.SendMessage("Generate");
			Asteroids.Add(spawn);
		}
	}

	public bool TryRespawnPlayer() {
		if (!respawning) return false;
		foreach(GameObject a in Asteroids) {
			// check if it is safe
			float dist = a.transform.position.magnitude;
			if (dist < softSafeDistance) {
				if (dist < hardSafeDistance) return false;
				bool approachingCenter = dist > (a.transform.position + a.rigidbody.velocity * 0.1f).magnitude;
				if (approachingCenter) return false;
			}
		}
		player = (GameObject) Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		respawning = false;
		return true;

	}

	public void PlayerDied() {
		respawning = true;
		deathCount++;
		// TODO: make respawn time coordinate with loop timing
		respawnTime = Time.time + respawnDelay;
	}

	public void Reset() {
		respawning = true;
		respawnTime = Time.time + respawnDelay;
		height = Camera.main.orthographicSize * 2;
		
		float aspectRatio = (float)Screen.width / (float)Screen.height;
		width = height * aspectRatio;
		maxDist = Mathf.Sqrt(width * width + height * height);
		SpawnAsteroids();
	}

	public void Clear() {
		Destroy(player);
		player = null;
		foreach (GameObject g in Asteroids) {
			Destroy(g);
		}
		Asteroids = new List<GameObject>();
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
