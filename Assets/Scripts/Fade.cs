using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {

	public float fadeRate = 0.6f;
	public bool visible;

	private float changeTime = 0f;

	// Use this for initialization
	void Start () {
		renderer.enabled = true;
		Color c = renderer.material.color;
		c.a = (visible) ? 1 : 0;
		renderer.material.color = c;
	}

	public IEnumerator FadeOut() {
		while (renderer.material.color.a > 0) {
			Color c = renderer.material.color;
			c.a -= fadeRate * Time.deltaTime;
			renderer.material.color = c;
			yield return null;
		}
	}

	public IEnumerator FadeIn() {
		while (renderer.material.color.a < 1) {
			Color c = renderer.material.color;
			c.a += fadeRate * Time.deltaTime;
			renderer.material.color = c;
			yield return null;
		}
	}

	public IEnumerator DoFade (float delay, float time, bool fadeIn) {
		yield return new WaitForSeconds(delay);
		Color c = renderer.material.color;
		c.a = (fadeIn) ? 0 : 1;
		changeTime = Time.time;
		while ((fadeIn) ? renderer.material.color.a < 1f : renderer.material.color.a > 0f) {
			if (fadeIn) {
				c.a = Ease ((Time.time - changeTime)/time);
			} else {
				c.a = 1f - Ease((Time.time - changeTime)/time);
			}
			c.a += ((fadeIn)? 1f : -1f) / time * Time.deltaTime;
			renderer.material.color = c;
			yield return null;
		}
	}

	public float Ease(float a) {
		if (a > 1f) return 1f;
		if (a < 0f) return 0f;
		return a * a * (3.0f - 2.0f * a);
	}
}
