using UnityEngine;
using System.Collections;

public class ActorDisplay : MonoBehaviour {
	public float frameTiming = 0.5f;
	float currentFrameTime;
	public WW.Actor actor;
	int frameCount = 0;
	void Start(){
		currentFrameTime = frameTiming;
	}
	// Update is called once per frame
	void Update () {
		if (actor == null)
			return;
		currentFrameTime -= Time.deltaTime;
		if (currentFrameTime < 0) {
			currentFrameTime += frameTiming;
			frameCount++;
			GetComponent<Renderer>().material.mainTexture = actor.GetCurrentArt ().GetFrame (frameCount).displayTex;
		}
	}
}
