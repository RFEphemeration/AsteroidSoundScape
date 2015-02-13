using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Link : MonoBehaviour {

	public string url;

	public Color hoverColor;

	private Color originalColor;

	private Text canvas;

	public void Start() {
		canvas = gameObject.GetComponent<Text>();
		originalColor = canvas.color;
	}

	public void OpenURL() {
		if( Application.platform == RuntimePlatform.OSXWebPlayer
		   || Application.platform == RuntimePlatform.WindowsWebPlayer ) {
			Application.ExternalEval("window.open('http://" + url + "','_blank')");
		} else {
			Application.OpenURL("http://" + url);
		}
	}

	public void Highlight() {
		canvas.color = hoverColor;
	}
	public void Lowlight() {
		canvas.color = originalColor;
	}
}
