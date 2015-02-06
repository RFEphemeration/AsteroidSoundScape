using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wrapper : MonoBehaviour {

	public List<GameObject> copies = new List<GameObject>();

	public GameObject original;

	public Vector3 offset;

	public int index;

	public static Vector3[] offsets = new Vector3[8]{Vector3.left,
		Vector3.right,
		Vector3.up,
		Vector3.down,
		Vector3.left+Vector3.up,
		Vector3.right+Vector3.up,
		Vector3.left+Vector3.down,
		Vector3.right+Vector3.down};

	// Use this for initialization
	void Start () {

	}

	void CreateCopies() {
		if (index >= 0) return;
		offset = Vector3.zero;
		for (int i = 0; i < 8; i++) {
			copies.Add((GameObject) Instantiate(gameObject));
			Wrapper w = copies[i].GetComponent<Wrapper>();
			w.copies = new List<GameObject>();
			w.original = gameObject;
			w.offset = offsets[i];
			w.index = i;
			w.OnResize();
		}
	}

	void Update() {
		if (index >= 0) {
			transform.position = original.transform.position + offset;
			transform.rotation = original.transform.rotation;
		} else {
			transform.position = new Vector3(-World.width/2 + Mathf.Repeat(transform.position.x + World.width/2, World.width * 2),
			                                 -World.height/2 + Mathf.Repeat(transform.position.y + World.height/2, World.height * 2),
			                                 transform.position.z);
		}
	}

	void OnResize() {
		if (index == -1) return;
		Vector3 off = Vector3.zero;
		if (offset.x != 0) {
			off += Vector3.right * offsets[index].x * World.width;
		}
		if (offset.y != 0) {
			off += Vector3.up * offsets[index].y * World.height;
		}
		offset = off;
	}

	void OnDestroy() {
		foreach (GameObject g in copies) {
			Destroy(g);
		}
	}
}
