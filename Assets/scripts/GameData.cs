using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour {
	public static GameData instance;
	public WW.Game gameData;
	void Awake(){
		instance = this;
		// load from save
		Debug.Log (Application.persistentDataPath + "/Test.xml");
		if (System.IO.File.Exists (Application.persistentDataPath + "/Test.xml")) {
			gameData = WW.Game.Load (Application.persistentDataPath + "/Test.xml");
		} else {
			gameData = new WW.Game ();
		}
		if (gameData.currentObject == null) {
			gameData.GetNewObject ();
			gameData.currentObject.SetupNewArt ();
		}
	}
}
