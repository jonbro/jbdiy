using UnityEngine;
using System.Collections;

public class ModeSelect : MonoBehaviour {
	public void SelectDrawing(){
		PanelManager.instance.DrawingMode ();
	}
	public void SelectScripting(){
		PanelManager.instance.ScriptingMode();
	}
	public void SelectPlay(){
		PanelManager.instance.PlayMode();
	}
}
