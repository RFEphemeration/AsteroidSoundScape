using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		Fabric.EventManager.Instance.SetParameter("Music", "Combat", World.instance.shortTermChange, null);
	}

	public static void Progress() {
		Fabric.EventManager.Instance.PostEvent("Progress", Fabric.EventAction.AdvanceSequence);
	}

	public static void Begin() {
		Fabric.EventManager.Instance.PostEvent("Music", Fabric.EventAction.ResetSequence);
		Fabric.EventManager.Instance.PostEvent("Music", Fabric.EventAction.PlaySound);
		Fabric.EventManager.Instance.PostEvent("Music", Fabric.EventAction.SetFadeIn);
	}

	public static void End() {
		Fabric.EventManager.Instance.PostEvent("Music", Fabric.EventAction.SetFadeOut);
	}

}
