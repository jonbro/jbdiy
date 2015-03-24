using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorDisplay : MonoBehaviour {
	public float frameTiming = 0.5f;
	float currentFrameTime;
	public WW.Actor actor;
	int frameCount = 0;
	WW.ScriptType currentMovement;
	Rect movementArea;
	Vector2 movementDirection = Vector2.zero;
	Vector2 stageSize = new Vector2(11, 8);
	void Start(){
		currentFrameTime = 0;
	}
	// Update is called once per frame
	void Update () {
		if (actor == null)
			return;
		currentFrameTime -= Time.deltaTime;
		if (currentFrameTime < 0) {
			currentFrameTime += frameTiming;
			frameCount++;
			Texture2D displayTex = actor.GetCurrentArt ().GetFrame (frameCount).displayTex;

			// should actually store the sprites somewhere on the data itself so I don't need to rebuild everytime
			GetComponent<SpriteRenderer>().sprite = Sprite.Create (displayTex,
				new Rect (0, 0, displayTex.width, displayTex.height),
				new Vector2 (0.5f, 0.5f),
				50
			);
		}
		if (currentMovement == WW.ScriptType.A_BOUNCEINAREA) {
			transform.position += new Vector3 (movementDirection.x, movementDirection.y, 0) * Time.deltaTime;
			// check to make sure we are still within the area
			float stageAreaXMin = movementArea.xMin * stageSize.x - stageSize.x / 2;
			float stageAreaXMax = movementArea.xMax * stageSize.x - stageSize.x / 2;
			float stageAreaYMin = (1-movementArea.yMax) * stageSize.y - stageSize.y / 2;
			float stageAreaYMax = (1-movementArea.yMin) * stageSize.y - stageSize.y / 2;
			if (transform.position.x < stageAreaXMin) {
				movementDirection = new Vector2 (-movementDirection.x, movementDirection.y);
				transform.position = new Vector3 (stageAreaXMin, transform.position.y, 0);
			}else if(transform.position.x > stageAreaXMax){
				movementDirection = new Vector2 (-movementDirection.x, movementDirection.y);
				transform.position = new Vector3 (stageAreaXMax, transform.position.y, 0);
			}
			if (transform.position.y < stageAreaYMin) {
				movementDirection = new Vector2 (movementDirection.x, -movementDirection.y);
				transform.position = new Vector3 (transform.position.x, stageAreaYMin, 0);
			}else if(transform.position.y > stageAreaYMax){
				movementDirection = new Vector2 (movementDirection.x, -movementDirection.y);
				transform.position = new Vector3 (transform.position.x, stageAreaYMax, 0);
			}
		} else {
			transform.position += new Vector3 (movementDirection.x, movementDirection.y, 0) * Time.deltaTime;
		}
	}
	public void TouchDown(Vector2 position){
		foreach (WW.Script script in actor.Scripts) {
			// should check to make sure all the triggers are true before running the actions
			foreach(WW.Trigger trigger in script.Triggers){
				if (trigger.tType == WW.ScriptType.T_TOUCH_ANYWHERE) {
					ProcessActions (script.Actions);
				}
			}
		}
	}
	public void TimeUpdate(int time){
		foreach (WW.Script script in actor.Scripts) {
			// should check to make sure all the triggers are true before running the actions
			foreach(WW.Trigger trigger in script.Triggers){
				if (trigger.tType == WW.ScriptType.T_TIME_EXACTLY && trigger.time == time) {
					ProcessActions (script.Actions);
				}
			}
		}
	}
	void JumpToArea(Rect area){
		// pick a random point within the area, based on the size of the stage
		float xRandom = Random.value * area.width + area.xMin;
		float yRandom = 1-(Random.value * area.height + area.yMin);
		transform.position = new Vector3 (xRandom*stageSize.x-stageSize.x/2, yRandom*stageSize.y-stageSize.y/2, 0);
	}
	public void ProcessActions(List<WW.Action> actions){
		foreach (WW.Action action in actions) {
			if (action.aType == WW.ScriptType.A_MOVEDIRECTION) {
				movementDirection = action.direction;
			}
			if (action.aType == WW.ScriptType.A_JUMPTOAREA) {
				movementDirection = Vector2.zero;
				JumpToArea (movementArea);
			}
			if (action.aType == WW.ScriptType.A_BOUNCEINAREA) {
				currentMovement = action.aType;
				movementDirection = Vector2.zero;
				movementArea = action.area;
				// pick a random direction
				movementDirection = (new Vector2 (Random.value, Random.value)).normalized;
				JumpToArea (movementArea);
			}
		}
	}
}
