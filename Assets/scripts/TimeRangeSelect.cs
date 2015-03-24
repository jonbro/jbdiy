using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeRangeSelect : MonoBehaviour {
	public Text timeDisplay;
	public Slider valueSliderLow;
	public Slider valueSliderHigh;
	int intValLow = 0;
	int intValHigh = 31;
	public delegate void TimeSelected(int timeLow, int timeHigh);
	public TimeSelected timeSelectedCallback;

	public void ValueChanged()
	{
		// translate into 32nds 
		float value = valueSliderLow.value*31;
		intValLow = Mathf.RoundToInt(value);
		value = valueSliderHigh.value*31;
		intValHigh = Mathf.RoundToInt(value);
		string lowText = (intValLow / 4 + 1) + "-" + (intValLow % 4 + 1);
		string highText = (intValHigh / 4 + 1) + "-" + (intValHigh % 4 + 1);
		timeDisplay.text = lowText + " : " + highText;
	}
	public void OkPressed(){
		if (timeSelectedCallback != null) {
			timeSelectedCallback (intValLow, intValHigh);
		}
	}
}
