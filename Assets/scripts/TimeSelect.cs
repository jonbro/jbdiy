using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeSelect : MonoBehaviour {
	public Text timeDisplay;
	public Slider valueSlider;
	int intVal = 0;
	public delegate void TimeSelected(int time);
	public TimeSelected timeSelectedCallback;


	public void ValueChanged()
	{
		// translate into 32nds 
		float value = valueSlider.value*31;
		intVal = Mathf.RoundToInt(value);
		timeDisplay.text = (intVal / 4 + 1) + "-" + (intVal % 4 + 1);
	}
	public void OkPressed(){
		if (timeSelectedCallback != null) {
			timeSelectedCallback (intVal);
		}
	}
}
