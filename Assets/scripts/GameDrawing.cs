using UnityEngine;
using System.Collections;

public class GameDrawing : MonoBehaviour {
	void OnEnable(){
		// setup actor select back button
		ActorSelect selector = PanelManager.instance.actorSelect.GetComponent<ActorSelect> ();
		selector.onBack = OnBack;
		selector.displayAddNew = true;
		selector.onSelect = OnSelect;
		// display the actor select
		PanelManager.instance.actorSelect.SetActive (true);
	}
	void OnSelect(WW.Actor currentActor){
		GameData.instance.gameData.currentObject = currentActor;
		PanelManager.instance.ShowDrawingPanel ();
	}
	public void OnBack(){
		PanelManager.instance.ModeSelect ();
	}
}
