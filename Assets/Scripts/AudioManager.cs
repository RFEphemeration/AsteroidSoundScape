using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	
	// Start Music
	void Start () {
		Fabric.EventManager.Instance.PostEvent("Music");
	}
	
	// Update is called once per frame
	void Update () {
		Fabric.EventManager.Instance.SetParameter("Music", "Combat", World.instance.shortTermChange, null);
	}

}
