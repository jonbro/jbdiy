﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreationUI : MonoBehaviour {
	enum creationMode{
		DRAW,
		ACTION,
		LAYOUT,
		ENDOFMODES
	}
	creationMode currentMode = creationMode.DRAW;
	public Text actorName, modeButton;
	string currentActorName = "";
	WW.WWGame game;
	public DrawToFBO drawingSystem;
	public GameObject drawingPanel;
	public RawImage drawingImage, onionSkin;
	public ActorDisplay actor;
	void Start(){
		// load from save
		Debug.Log (Application.persistentDataPath + "/Test.xml");
		if (System.IO.File.Exists (Application.persistentDataPath + "/Test.xml")) {
			Debug.Log ("found test");
			game = WW.WWGame.Load ("Test.xml");
		} else {
			game = new WW.WWGame ();
			Debug.Log ("xfound test");
		}
		if (game.currentObject == null) {
			game.GetNewObject ();
			game.currentObject.SetupNewArt ();
			Debug.Log ("creating new art");
		}
		actor.actor = game.currentObject;
		drawingImage.texture = game.currentObject.GetCurrentArt ().currentFrame.displayTex;
		onionSkin.enabled = false;
		drawingPanel.GetComponent<DrawingArea> ().drawingImage = drawingImage;
	}
	void SaveCurrentActor(){
		// store the art in the fbo into the current filename
		game.currentObject.OverwriteArtWithRenderTexture (drawingSystem.GetDrawTexture());
	}
	public void AddNewActor(){
		StartCoroutine (_AddNewActor ());
	}
	IEnumerator _AddNewActor(){
		// store the current art to the current actor, and setup a game object if it doesn't have one already
		yield return new WaitForEndOfFrame ();
		game.currentObject.OverwriteArtWithRenderTexture (drawingSystem.GetDrawTexture());
		drawingSystem.ClearDrawTexture ();
		game.GetNewObject ();
		game.currentObject.SetupNewArt ();
	}
	public void ChangeMode(){
		currentMode = (creationMode)(((int)currentMode + 1) % ((int)creationMode.ENDOFMODES));
		modeButton.text = "Mode: " + currentMode.ToString ().ToLower();
		switch (currentMode) {
		case creationMode.DRAW:
			drawingPanel.SetActive (true);
			break;
		default:
			drawingPanel.SetActive (false);
			break;
		}
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
		game.Save ();
	}
	void UpdateDrawingTextures(){
		drawingImage.texture = game.currentObject.GetCurrentArt ().currentFrame.displayTex;
		onionSkin.texture = game.currentObject.GetCurrentArt ().prevFrame.displayTex;
	}
	public void AddTriger(){
		// show the add trigger panel
	}
	void UpdateActorName(){
	}
}
