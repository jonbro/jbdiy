using UnityEngine;
using System.Collections;

public class ScriptItem : MonoBehaviour {
	public WW.Script targetScript;
	public WW.Trigger targetTrigger;
	public WW.Action targetAction;
	public ScriptingSystem.ScriptItemDeleteCallback deleteCallback;
	public void DeleteButton(){
		if (deleteCallback != null)
			deleteCallback (targetScript, targetTrigger, targetAction);
	}
}
