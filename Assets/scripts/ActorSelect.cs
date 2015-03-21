using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActorSelect : MonoBehaviour {
	public RectTransform actorList;
	public GameObject ActorButton;
	private WW.Game gameData;
	void OnEnable(){
		gameData = GameData.instance.gameData;
		// clear the actor list, then build all the buttons
		foreach (Transform t in actorList.transform) {
			Destroy (t.gameObject);
		}
		foreach (WW.Actor o in gameData.Objects) {
			GameObject go = Instantiate (ActorButton);
			ActorButton button = go.GetComponent<ActorButton>();
			button.actorName.text = o.Name;
			Texture2D displayTex = o.art [0].Frames [0].displayTex;
			Sprite actorSprite = Sprite.Create (displayTex,
				                     new Rect (0, 0, displayTex.width, displayTex.height),
				                     new Vector2 (0.5f, 0.5f)
			                     );
			button.actorImage.sprite = actorSprite;
			button.transform.SetParent (actorList, false);
			WW.Actor thisActor = o;

			button.GetComponent<Button> ().onClick.AddListener (() => {
				gameData.currentObject = thisActor;
				PanelManager.instance.drawingPanel.gameObject.SetActive(true);
				PanelManager.instance.actorSelect.gameObject.SetActive(false);
			});
		}

	}
	public void AddNewActor(){
		gameData.currentObject = gameData.GetNewObject ();
		gameData.currentObject.SetupNewArt ();
		gameData.currentObject.art [0].AddNewFrame ();
		PanelManager.instance.drawingPanel.gameObject.SetActive(true);
		PanelManager.instance.actorSelect.gameObject.SetActive(false);
	}
}
