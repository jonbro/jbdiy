using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DirectionSelect : MonoBehaviour {
	public delegate void DirectionSelected(Vector2 direction);
	public DirectionSelected directionSelectedCallback;
	// Use this for initialization
	void Start () {
		foreach (Button b in GetComponentsInChildren<Button>()) {
			string name = b.gameObject.name;
			b.onClick.AddListener (() => {
				ButtonPressed(name);
			});
		}	
	}	
	void ButtonPressed(string name){
		if (directionSelectedCallback != null) {
			switch (name) {
			case "u":
				directionSelectedCallback ((new Vector2 (0, 1)).normalized);
				break;
			case "ul":
				directionSelectedCallback ((new Vector2 (-1, 1)).normalized);
				break;
			case "ur":
				directionSelectedCallback ((new Vector2 (1, 1)).normalized);
				break;
			case "d":
				directionSelectedCallback ((new Vector2 (0, -1)).normalized);
				break;
			case "dl":
				directionSelectedCallback ((new Vector2 (-1, -1)).normalized);
				break;
			case "dr":
				directionSelectedCallback ((new Vector2 (1, -1)).normalized);
				break;
			case "l":
				directionSelectedCallback ((new Vector2 (-1, 0)).normalized);
				break;
			case "r":
				directionSelectedCallback ((new Vector2 (1, 0)).normalized);
				break;
			}
		}
	}
}
