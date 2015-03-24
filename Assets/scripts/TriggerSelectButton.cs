using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriggerSelectButton : MonoBehaviour {
	public WW.Script targetScript;
	public ScriptingSystem scriptingSystem;
	public ScriptingSystem.TriggerButtonCallback onPressCallback;
	public WW.ScriptType scriptType;
	// Use this for initialization
	void Start () {
		GetComponent<Button> ().onClick.AddListener(ButtonPressed);
	}
	void ButtonPressed(){
		if (onPressCallback != null)
			onPressCallback (targetScript, scriptType);
	}
}
