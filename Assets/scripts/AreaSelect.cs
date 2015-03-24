using UnityEngine;
using System.Collections;

public class AreaSelect : MonoBehaviour {
	public RectTransform areaBoundsPanel;
	public RectTransform areaPanel;
	public delegate void AreaSelected(Rect area);
	public AreaSelected areaSelectedCallback;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void OnOk(){
		// extract the boundaries of the rectangle that the user input
		float totalWidth = areaBoundsPanel.rect.width;
		float totalHeight = areaBoundsPanel.rect.height;
		float anchorPositionX = areaPanel.anchoredPosition.x / totalWidth;
		float anchorPositionY = -(areaPanel.anchoredPosition.y / totalHeight);
		float width = areaPanel.rect.width / totalWidth;
		float height = areaPanel.rect.height / totalHeight;
		if (areaSelectedCallback != null) {
			areaSelectedCallback (new Rect (anchorPositionX, anchorPositionY, width, height));
		}
	}
}
