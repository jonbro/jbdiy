using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Util
{
	/*
	 * from phaser.js
	 * 0 = top to bottom
	 * 1 = bottom to top
	 * 2 = left to right
	 * 3 = right to left
	*/
	public static Vector2 FindFirstPixel (Texture2D tex, int direction)
	{
		Color32[] pix = tex.GetPixels32 ();
		int x = 0;
		int y = 0;
		int v = 1;
		var scan = false;

		if (direction == 1) {
			v = -1;
			y = tex.height - 1;
		} else if (direction == 3) {
			v = -1;
			x = tex.width - 1;
		}

		Color32 pixel;
		do {
			pixel = pix [y * tex.width + x];
			if (direction == 0 || direction == 1) {
				//  Top to Bottom / Bottom to Top
				x++;

				if (x == tex.width - 1) {
					x = 0;
					y += v;

					if (y >= tex.height - 1 || y <= 0) {
						scan = true;
					}
				}
			} else if (direction == 2 || direction == 3) {
				//  Left to Right / Right to Left
				y++;

				if (y == tex.height - 1) {
					y = 0;
					x += v;

					if (x >= tex.width - 1 || x <= 0) {
						scan = true;
					}
				}
			}
		} while (pixel.a == 0 && !scan);
		return new Vector2 (x, y);
	}
}
namespace WW
{
	[XmlRoot ("WWGame")]
	public class Game
	{
		// some temp variables for maintaining state in the ui
		public static Game currentGame;
		public static WW_Action currentAction;

		public static void LoadPlaceholder ()
		{
			currentGame = Load (Application.persistentDataPath + "/" + "Test.xml");
		}
		public static string[] ListObjects ()
		{
			if (currentGame == null)
				LoadPlaceholder ();
			List<string> itemList = new List<string> ();
			foreach (Actor f in currentGame.Objects) {
				itemList.Add (f.Name);
			}
			return itemList.ToArray ();
		}

		[XmlAttribute ("name")]
		public string Name;
		[XmlArray ("Objects")]
		[XmlArrayItem ("Actor")]
		public List<Actor> Objects = new List<Actor> ();

		[XmlIgnore]
		public Actor currentObject;
		public string filename = "Test.xml";

		public void Save (string path)
		{
			filename = path;
			var serializer = new XmlSerializer (typeof(Game));
			using (var stream = new FileStream (path, FileMode.Create)) {
				serializer.Serialize (stream, this);
			}
		}

		public void Save ()
		{
			Save (filename);
		}

		public static Game Load (string path)
		{
			var serializer = new XmlSerializer (typeof(Game));
			using (var stream = new FileStream (path, FileMode.Open)) {
				Game rGame = serializer.Deserialize (stream) as Game;
				rGame.filename = path;
				if(rGame.Objects.Count > 0)
					rGame.currentObject = rGame.Objects [0];
				return rGame;
			}
		}

		public Actor GetNewObject ()
		{
			Actor newObject = new Actor ();
			currentObject = newObject;
			Objects.Add (currentObject);
			currentObject.Name = "Actor " + Objects.Count;
			return currentObject;
		}
		//Loads the xml directly from the given string. Useful in combination with www.text.
		public static Game LoadFromText (string text)
		{
			var serializer = new XmlSerializer (typeof(Game));
			return serializer.Deserialize (new StringReader (text)) as Game;
		}
	}

	public class Actor
	{
		[XmlAttribute ("currentArt")]
		public int currentArt = 0;
		[XmlArray ("Arts")]
		[XmlArrayItem ("WWArt")]
		public List<Art> art = new List<Art> ();
		[XmlAttribute ("name")]
		public string Name;
		[XmlAttribute ("stagePos_x")]
		public float stagePos_x;
		[XmlAttribute ("stagePos_y")]
		public float stagePos_y;
		[XmlArray ("Actions")]
		[XmlArrayItem ("WW_Action")]
		public List<WW_Action> Actions = new List<WW_Action> ();
		GameObject displayObject;

		public Art SetupNewArt ()
		{
			currentArt = art.Count;
			Art newart = new Art (Name + "_" + currentArt);
			art.Add (newart);
			return newart;
		}

		public Art GetCurrentArt ()
		{
			return art [currentArt];
		}

		public void OverwriteArtWithRenderTexture (RenderTexture drawTexture)
		{
			Texture2D saveTex = new Texture2D (drawTexture.width, drawTexture.height, TextureFormat.ARGB32, false, true);

			RenderTexture.active = drawTexture;
			saveTex.ReadPixels (new Rect (0, 0, drawTexture.width, drawTexture.height), 0, 0);
			saveTex.Apply ();
			// determine the edges of the texture that have stuff in them
			int top = (int)Util.FindFirstPixel (saveTex, 0).y;
			int bottom = (int)Util.FindFirstPixel (saveTex, 1).y;
			int left = (int)Util.FindFirstPixel (saveTex, 2).x;
			int right = (int)Util.FindFirstPixel (saveTex, 3).x;
			RenderTexture.active = null;
			if (displayObject == null) {
				displayObject = (GameObject)GameObject.Instantiate ((Resources.Load ("ActorPrefab", typeof(GameObject))as GameObject), new Vector3 (0, 0, 0), Quaternion.identity);
				displayObject.GetComponent<SpriteRenderer> ().sprite = Sprite.Create (saveTex, new Rect (left, top, right - left, bottom - top), new Vector2 (0.5f, 0.5f));
				displayObject.GetComponent<SpriteRenderer> ().sprite.texture.filterMode = FilterMode.Point;
				displayObject.AddComponent<PolygonCollider2D> ();
				// TODO: determine the offset of the actor based on the cropping of the sprite
				// TODO: this is kinda an arbitrary scaling value that happens to match, but I think will need to be fixed
				displayObject.transform.localScale = Vector3.one * 8;
			}
		}
	}

	public class Art
	{

		[XmlArray ("Frames")]
		[XmlArrayItem ("Frame")]
		public List<Frame> Frames = new List<Frame> ();

		public string name;
		[XmlIgnore]
		public int currentFrameCounter = 0;
		public Art(){
			Debug.Log (Frames.Count);

		}
		[XmlIgnore]
		public Frame currentFrame {
			get{ return Frames [currentFrameCounter]; }
		}
		[XmlIgnore]
		public Frame prevFrame {
			get {
				int prevFrame = currentFrameCounter - 1;
				if (prevFrame < 0)
					prevFrame = Frames.Count - 1;
				return Frames [prevFrame];
			}
		}

		public Art (string _name)
		{
			name = _name;
			AddNewFrame ();
		}

		public Frame GetFrame (int count)
		{
			return Frames [count % Frames.Count];
		}

		public void DeleteCurrentFrame ()
		{
			if(Frames.Count > 1)
				Frames.RemoveAt (currentFrameCounter);
		}

		public void AddNewFrame ()
		{
			Frame frame = new Frame ();
			Frames.Add (frame);
			frame.filename = name + "_" + Frames.Count;
			currentFrameCounter = Frames.Count - 1;
		}

		public void MoveAnimationForward ()
		{
			currentFrameCounter = (currentFrameCounter + 1) % Frames.Count;
		}

		public void MoveAnimationBackward ()
		{
			currentFrameCounter--;
			if (currentFrameCounter < 0)
				currentFrameCounter = Frames.Count - 1;
		}
	}

	[System.Serializable]
	public class Frame
	{
		[XmlAttribute ("filename")]
		public string filename;

		[XmlIgnore]
		public Texture2D displayTex;
		[XmlAttribute]
		public int placeHolder{
			get{ SaveToPng (); return 0; }
			set{ LoadFromPng (Application.persistentDataPath + "/" + filename + ".png"); }
		}
		public Frame ()
		{
			displayTex = new Texture2D (64, 64, TextureFormat.ARGB32, false);
			Color32 fillColor = new Color32 (0xff, 0xff, 0xff, 0x0);
			Color32[] pixels = displayTex.GetPixels32 ();
			for (int i = 0; i < pixels.Length; i++) {
				pixels [i] = fillColor;
			}
			displayTex.SetPixels32 (pixels);
			displayTex.Apply ();
			displayTex.filterMode = FilterMode.Point;
		}
		public void LoadFromPng(string filePath){
			byte[] fileData;
			Debug.Log ("loading png");
			if (File.Exists(filePath))     {
				Debug.Log ("found file");
				fileData = File.ReadAllBytes(filePath);
				displayTex = new Texture2D(200, 200);
				displayTex.filterMode = FilterMode.Point;
				displayTex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
			}
		}
		public void OnAfterDeserialize(){
			Debug.Log ("after deserialize");
			// load the texture from the serialized object
		}

		[System.Runtime.Serialization.OnSerializedAttribute]
		public void OnBeforeSerialize(System.Runtime.Serialization.StreamingContext context){
			Debug.Log ("should be saving to png");
			// save the texuter to a png
			SaveToPng ();
		}

		public void SaveToPng ()
		{
			var bytes = displayTex.EncodeToPNG ();
			// should have a better datapath to write to
			string pngPath = Application.persistentDataPath + "/" + filename + ".png";
			Debug.Log ("should be saving png to "+pngPath);
			File.WriteAllBytes (pngPath, bytes);
		}
	}

	public class WW_Action
	{
		[XmlAttribute ("name")]
		public string Name;
		[XmlArray ("Triggers")]
		[XmlArrayItem ("WW_Trigger")]
		public List<WW_Trigger> Triggers = new List<WW_Trigger> ();
		[XmlArray ("Behaivors")]
		[XmlArrayItem ("WW_Behaivor")]
		public List<WW_Behaivor> Behaivors = new List<WW_Behaivor> ();
	}

	public class WW_Trigger
	{
	}

	public class WW_Behaivor
	{
	}
}