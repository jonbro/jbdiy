using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScriptingSystem : MonoBehaviour {
	// Use this for initialization
	// base panels
	public GameObject triggerSelectionPanel, actionSelectionPanel, scriptSetPrefab, scrollArea, scriptItem;
	// parameter panels
	public GameObject directionSelectPanel, timeSelectPanel, timeRangeSelectPanel, areaSelectPanel;

	public delegate void TriggerButtonCallback(WW.Script targetScript, WW.ScriptType stype);
	public delegate void ScriptItemDeleteCallback  (WW.Script targetScript, WW.Trigger trigger, WW.Action action);

	void OnEnable () {
		RebuildScriptDisplay ();
	}
	void RebuildScriptDisplay(){
		// clear out the children on the scroll area
		foreach (Transform child in scrollArea.transform) {
			Destroy (child.gameObject);
		}
		// instantiate all the scripts that already exist on the object
		foreach (WW.Script script in GameData.instance.gameData.currentObject.Scripts) {
			GameObject scriptSetGo = Instantiate (scriptSetPrefab);
			scriptSetGo.transform.SetParent (scrollArea.transform, false);
			ScriptSet scriptSet = scriptSetGo.GetComponent<ScriptSet> ();
			// link up the buttons for add trigger / add script
			foreach (TargetScript ts in scriptSetGo.GetComponentsInChildren<TargetScript>()) {
				ts.targetScript = script;
			}
			foreach (WW.Trigger trigger in script.Triggers) {
				GameObject scriptItemGO = Instantiate (scriptItem) as GameObject;
				scriptItemGO.transform.SetParent (scriptSet.triggerArea.transform, false);
				scriptItemGO.GetComponent<Text> ().text = trigger.tType.ToString();
				ScriptItem si = scriptItemGO.GetComponent<ScriptItem> ();
				si.targetScript = script;
				si.deleteCallback = ScriptItemDelete;
				si.targetTrigger = trigger;
			}
			// list all the actions that are on the target script
			foreach (WW.Action action in script.Actions) {
				GameObject scriptItemGO = Instantiate (scriptItem) as GameObject;
				scriptItemGO.transform.SetParent (scriptSet.actionArea.transform, false);
				scriptItemGO.GetComponent<Text> ().text = action.aType.ToString();
				ScriptItem si = scriptItemGO.GetComponent<ScriptItem> ();
				si.targetScript = script;
				si.deleteCallback = ScriptItemDelete;
				si.targetAction = action;
			}
			/**/
		}
	}
	public void Back(){
		PanelManager.instance.ScriptingMode ();
	}
	public void AddScriptSet(){
		GameObject scriptSetGo = Instantiate (scriptSetPrefab);
		scriptSetGo.transform.SetParent (scrollArea.transform, false);

		WW.Script newScript = new WW.Script ();
		GameData.instance.gameData.currentObject.Scripts.Add (newScript);
		foreach (TargetScript ts in scriptSetGo.GetComponentsInChildren<TargetScript>()) {
			ts.targetScript = newScript;
		}
	}
	public void AddTrigger(WW.Script target){
		triggerSelectionPanel.SetActive (true);
		foreach (TriggerSelectButton tsb in triggerSelectionPanel.GetComponentsInChildren<TriggerSelectButton>()) {
			tsb.targetScript = target;
			tsb.scriptingSystem = this;
			tsb.onPressCallback = AddTriggerButtonCallback;
		}
	}
	public void AddAction(WW.Script target){
		actionSelectionPanel.SetActive (true);
		foreach (TriggerSelectButton tsb in actionSelectionPanel.GetComponentsInChildren<TriggerSelectButton>()) {
			tsb.targetScript = target;
			tsb.scriptingSystem = this;
			tsb.onPressCallback = AddActionButtonCallback;
		}
	}
	public void AddTriggerButtonCallback(WW.Script targetScript, WW.ScriptType stype){
		Debug.Log ("adding trigger");
		// add the current trigger to the current object
		WW.Trigger trigger = new WW.Trigger{tType=stype};
		targetScript.Triggers.Add (trigger);
		GameData.instance.gameData.Save ();
		// should add support for additional data on triggers (i.e. rectangle selector, object selector)
		triggerSelectionPanel.gameObject.SetActive (false);
		WW.ParameterSet pSet = WW.Script.GetParameterSetForAction (stype);
		if(pSet.time){
			// get direction parameter
			timeSelectPanel.SetActive (true);
			timeSelectPanel.GetComponent<TimeSelect> ().timeSelectedCallback = (int time) => {
				trigger.time = time;
				timeSelectPanel.SetActive (false);
			};
		}
		if (pSet.timeRange) {
			timeRangeSelectPanel.SetActive (true);
			timeRangeSelectPanel.GetComponent<TimeRangeSelect> ().timeSelectedCallback = (int timeLow, int timeHigh) => {
				trigger.time = timeLow;
				trigger.time = timeHigh;
				timeRangeSelectPanel.SetActive (false);
			};
		}
		RebuildScriptDisplay ();
	}
	public void AddActionButtonCallback(WW.Script targetScript, WW.ScriptType stype){
		Debug.Log ("adding action");
		// add the current trigger to the current object
		WW.Action action = new WW.Action{aType=stype};
		targetScript.Actions.Add (action);
		// if the action has parameters, then we need to set them
		WW.ParameterSet pSet = WW.Script.GetParameterSetForAction (stype);
		if(pSet.direction){
			// get direction parameter
			directionSelectPanel.SetActive (true);
			directionSelectPanel.GetComponent<DirectionSelect> ().directionSelectedCallback = (Vector2 dir) => {
				action.direction = dir;
				directionSelectPanel.SetActive (false);
			};
		}
		if(pSet.area){
			// get direction parameter
			areaSelectPanel.SetActive (true);
			areaSelectPanel.GetComponent<AreaSelect> ().areaSelectedCallback = (Rect area) => {
				action.area = area;
				areaSelectPanel.SetActive (false);
			};
		}
		GameData.instance.gameData.Save ();
		// should add support for additional data on triggers (i.e. rectangle selector, object selector)
		actionSelectionPanel.gameObject.SetActive (false);
		RebuildScriptDisplay ();
	}
	void SetParameters(){

	}
	public void ScriptItemDelete (WW.Script targetScript, WW.Trigger trigger, WW.Action action){
		if(trigger != null)
			targetScript.Triggers.Remove (trigger);
		else
			targetScript.Actions.Remove (action);
		GameData.instance.gameData.Save ();
		RebuildScriptDisplay ();
	}
}
