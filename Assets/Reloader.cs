using UnityEngine;
using System.Collections;

public class Reloader : MonoBehaviour {

	public enum State {loading, loaded, empty};
	public State state = State.empty;

	public Control player;

	public float maxScaleX = 1f;
	public float currentScaleX = 0f;

	public float reloadInterval = 4.4f;
	public float reloadTime;

	void Start() {
		transform.localScale = new Vector3(currentScaleX, transform.localScale.y, transform.localScale.z);
	}

	// Update is called once per frame
	void Update () {
		if (state == State.loading) {
			if (currentScaleX < maxScaleX) {
				currentScaleX += Time.deltaTime / reloadInterval * maxScaleX;
				transform.localScale = new Vector3(currentScaleX, transform.localScale.y, transform.localScale.z);
			} else {
				state = State.loaded;
			}
		}
	}

	public bool Fire() {
		if (state != State.loaded) return false;
		state = State.empty;
		currentScaleX = 0f;
		transform.localScale = new Vector3(currentScaleX, transform.localScale.y, transform.localScale.z);
		return true;
	}

	public bool Reload() {
		if (state != State.empty) return false;
		state = State.loading;
		return true;
	}
}
