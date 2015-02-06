using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SectionFade : Fade {

	public GameObject section;

	public float spacing = 0.2f;

	new void Start() {
		if (visible) {
			foreach (Transform t in transform) {
				if (t.renderer != null) {
					t.renderer.enabled = true;
				}
			}
		} else {
			foreach (Transform t in transform) {
				if (t.renderer != null) {
					t.renderer.enabled = false;
				}
			}
		}
		Vector3 corner = new Vector3(-World.width/2f, World.height/2f, transform.position.z);
		int height = Mathf.CeilToInt(World.height / spacing) + 1;
		int width = Mathf.CeilToInt(World.width / spacing) + 1;
		GameObject plus;
		Vector3 pos = corner;
		for (int i = 0; i < height; i++) {
			pos.x = corner.x;
			for (int j = 0; j < width; j++) {
				plus = (GameObject) Instantiate(section, pos, Quaternion.identity);
				plus.transform.parent = transform;
				pos.x += spacing;
			}
			pos.y -= spacing;
		}
	}
	/*
	public new IEnumerator FadeIn() {
		changeTime = Time.time;
		foreach (Transform t in transform) {
			if (t.renderer != null) {
				t.renderer.enabled = true;
				yield return null;
			}
		}
		visible = true;
		yield return null;
	}

	public new IEnumerator FadeOut() {
		changeTime = Time.time;
		foreach (Transform t in transform) {
			if (t.renderer != null) {
				t.renderer.enabled = false;
				yield return null;
			}
		}
		visible = false;
		yield return null;
	}
	*/

	public new IEnumerator DoFade (float delay, float time, bool fadeIn) {
		yield return new WaitForSeconds(delay);
		changeTime = Time.time;
		List<Renderer> children = new List<Renderer>();
		foreach (Transform t in transform) {
			if (t.renderer != null) children.Add (t.renderer);
		}
		int total = children.Count;
		int count = 0;
		int frameCount = 0;
		changeTime = Time.time;
		Renderer r;
		while (children.Count > 0) {
			frameCount = Mathf.CeilToInt(Ease((Time.time - changeTime)/time)*total) + 1;
			frameCount = Mathf.Min(frameCount, total);
			while (frameCount > count) {
				count += 1;
				r = children[Random.Range(0, children.Count)];
				r.enabled = fadeIn;
				children.Remove(r);
			}
			yield return null;
		}
		yield return null;
	}
}
