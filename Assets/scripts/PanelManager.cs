using UnityEngine;
using System.Collections;

public class PanelManager : MonoBehaviour {
	public static PanelManager instance;
	public GameObject drawingPanel;
	public GameObject actorSelect;
	// Use this for initialization
	void Awake () {
		instance = this;
	}
}
