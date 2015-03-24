using UnityEngine;
using System.Collections;

public class GameScriptingManager : MonoBehaviour {
	/* game scripting manager flow
	 * 
	 * select actor
	 * add triggers / actions to actor
	 * 
	 * */
	void OnEnable(){
		// show the actor select
		ActorSelect selector = PanelManager.instance.actorSelect.GetComponent<ActorSelect> ();
		selector.displayAddNew = false;
		selector.onBack = OnBack;
		selector.onSelect = OnSelect;
		PanelManager.instance.actorSelect.SetActive (true);
	}
	void OnSelect(WW.Actor currentActor){
		GameData.instance.gameData.currentObject = currentActor;
		PanelManager.instance.ShowScriptingPanel ();
	}
	public void OnBack(){
		PanelManager.instance.ModeSelect ();
	}

}
