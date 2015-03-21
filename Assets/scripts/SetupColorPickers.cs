using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SetupColorPickers : MonoBehaviour {
	public List<Color> colors;
	public GameObject ColorPicker;
	public DrawingArea drawingArea;
	// Use this for initialization
	void Start () {
		foreach (Color c in colors) {
			GameObject g = Instantiate (ColorPicker) as GameObject;
			g.transform.SetParent (transform, false);
			g.GetComponentInChildren<Image> ().color = c;
			Color thisColor = c;
			g.GetComponent<Button>().onClick.AddListener(()=>{
				Debug.Log("setting color: "+thisColor);
				drawingArea.SetColor(thisColor);
			});
		}
	}
	
}
