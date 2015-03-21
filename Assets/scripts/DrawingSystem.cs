using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class DrawingSystem : MonoBehaviour {
	WW.Game game;
	public GameObject drawingPanel;
	public RawImage drawingImage, onionSkin;
	void OnEnable(){
		game = GameData.instance.gameData;
		Debug.Log (game.currentObject.art[0].Frames.Count);
		drawingImage.texture = game.currentObject.GetCurrentArt ().currentFrame.displayTex;
		onionSkin.enabled = false;
		drawingPanel.GetComponent<DrawingArea> ().drawingImage = drawingImage;
	}
	void OnApplicationQuit(){
		Save ();
	}
	public bool onionSkinEnabled = false;
	public void ToggleOnionSkin(){
		onionSkinEnabled = !onionSkinEnabled;
		if (onionSkinEnabled) {
			onionSkin.color = new Color (1, 1, 1, 0.35f);
			onionSkin.enabled = true;
		} else {
			onionSkin.enabled = false;
		}
	}
	public void AddCel(){
		game.currentObject.GetCurrentArt ().AddNewFrame ();
		UpdateDrawingTextures ();
	}
	public void NextCel(){
		game.currentObject.GetCurrentArt ().MoveAnimationForward();
		UpdateDrawingTextures ();
	}
	public void PrevCel(){
		game.currentObject.GetCurrentArt ().MoveAnimationBackward();
		UpdateDrawingTextures ();
	}
	public void DeleteCurrentFrame(){
		game.currentObject.GetCurrentArt ().DeleteCurrentFrame ();
	}
	public void Save(){
		game.Save (Application.persistentDataPath + "/Test.xml");
	}
	public void Back(){
		Save ();
		PanelManager.instance.drawingPanel.SetActive (false);
		PanelManager.instance.actorSelect.SetActive (true);
	}
	void UpdateDrawingTextures(){
		drawingImage.texture = game.currentObject.GetCurrentArt ().currentFrame.displayTex;
		onionSkin.texture = game.currentObject.GetCurrentArt ().prevFrame.displayTex;
	}
}
