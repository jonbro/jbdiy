using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePlayer : MonoBehaviour {
	WW.Game data;
	List<ActorDisplay> actors = new List<ActorDisplay>();
	bool playing;
	int currentTime, lastSentTime;
	float startTime;
	public void RunPlayerOnData(WW.Game _data){
		data = _data;
		// instantiate all the actors I need
		foreach (WW.Actor actor in data.Actors) {
			GameObject actorObject = (GameObject)Instantiate(Resources.Load ("ActorPrefab"));
			actorObject.GetComponent<ActorDisplay> ().actor = actor;
			actorObject.transform.SetParent (this.transform, false);
			actors.Add (actorObject.GetComponent<ActorDisplay> ());
		}
		playing = true;
		currentTime = 0;
		lastSentTime = -1;
		startTime = Time.time;
	}
	public void EndPlayer(){
		// destroy all of the child objects
		foreach (ActorDisplay actor in actors) {
			Destroy (actor.gameObject);
		}
		actors.Clear ();
	}
	void Update(){
		// handle inputs during update, and distribute to all the child objects
		if(playing){
			currentTime = Mathf.FloorToInt ((Time.time-startTime) * 4);
			if (currentTime > lastSentTime) {
				lastSentTime = currentTime;
				foreach (ActorDisplay ad in actors) {
					ad.TimeUpdate (currentTime);
				}
			}
			if (Input.GetMouseButtonDown (0)) {
				foreach (ActorDisplay ad in actors) {
					ad.TouchDown (Vector2.zero);
				}
			}
		}
	}
}
