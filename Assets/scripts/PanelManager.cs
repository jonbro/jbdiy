using UnityEngine;
using System.Collections;

public class PanelManager : MonoBehaviour {
	public static PanelManager instance;
	public GameObject drawingPanel, drawingSelectActor, actorSelect, scriptingPanelGlobal, scriptingPanelActor, modeSelect, exitPlay;
	public GamePlayer gamePlayer;
	// Use this for initialization
	void Awake () {
		instance = this;
	}
	public void HideAll(){
		drawingPanel.SetActive (false);
		actorSelect.SetActive (false);
		scriptingPanelGlobal.SetActive (false);
		scriptingPanelActor.SetActive (false);
		modeSelect.SetActive (false);
		exitPlay.SetActive (false);
	}
	public void ModeSelect(){
		HideAll ();
		modeSelect.SetActive (true);
	}
	public void DrawingMode(){
		HideAll ();
		drawingSelectActor.SetActive (true);
		drawingSelectActor.GetComponent<GameDrawing> ().enabled = false;
		drawingSelectActor.GetComponent<GameDrawing> ().enabled = true;
	}
	public void ScriptingMode(){
		HideAll ();
		scriptingPanelGlobal.SetActive (true);
		scriptingPanelGlobal.GetComponent<GameScriptingManager> ().enabled = false;
		scriptingPanelGlobal.GetComponent<GameScriptingManager> ().enabled = true;
	}
	public void PlayMode(){
		HideAll ();
		gamePlayer.RunPlayerOnData (GameData.instance.gameData);
		exitPlay.SetActive (true);
	}
	public void ExitPlay(){
		ModeSelect ();
		gamePlayer.EndPlayer ();
	}
	public void ShowDrawingPanel(){
		HideAll ();
		drawingPanel.SetActive (true);
		drawingPanel.GetComponent<DrawingSystem> ().enabled = false;
		drawingPanel.GetComponent<DrawingSystem> ().enabled = true;
	}
	public void ShowScriptingPanel(){
		HideAll ();
		scriptingPanelActor.SetActive (true);
		scriptingPanelActor.GetComponent<ScriptingSystem> ().enabled = false;
		scriptingPanelActor.GetComponent<ScriptingSystem> ().enabled = true;
	}
}
