using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetScript : MonoBehaviour {
	public WW.Script targetScript;
	public bool Action;
	void Start(){
		GetComponent<Button>().onClick.AddListener(OnClick);
	}
	void OnClick(){
		if(Action)
			PanelManager.instance.scriptingPanelActor.GetComponent<ScriptingSystem> ().AddAction (targetScript);
		else
			PanelManager.instance.scriptingPanelActor.GetComponent<ScriptingSystem> ().AddTrigger (targetScript);
	}
}
