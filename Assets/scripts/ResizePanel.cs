using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResizePanel : MonoBehaviour, IPointerDownHandler, IDragHandler {

	public Vector2 minSize;
	public Vector2 maxSize;

	public RectTransform rectTransform;
	private Vector2 currentPointerPosition;
	private Vector2 previousPointerPosition;
	private Vector2 startPosition;
	void Awake () {

	}

	public void OnPointerDown (PointerEventData data) {
		RectTransformUtility.ScreenPointToLocalPointInRectangle (GetComponent<RectTransform>(), data.position, data.pressEventCamera, out previousPointerPosition);
		rectTransform.localPosition = startPosition = previousPointerPosition;
		rectTransform.sizeDelta = Vector2.zero;
	}

	public void OnDrag (PointerEventData data) {
		if (rectTransform == null)
			return;

		Vector2 sizeDelta = rectTransform.sizeDelta;

		RectTransformUtility.ScreenPointToLocalPointInRectangle (GetComponent<RectTransform>(), data.position, data.pressEventCamera, out currentPointerPosition);

		// need to reverse the y values :/
		Vector2 minValue = new Vector2 (Mathf.Min(startPosition.x, currentPointerPosition.x), Mathf.Max(startPosition.y, currentPointerPosition.y));
		Vector2 maxValue = new Vector2 (Mathf.Max(startPosition.x, currentPointerPosition.x), Mathf.Min(startPosition.y, currentPointerPosition.y));

		Vector2 resizeValue = currentPointerPosition - previousPointerPosition;


		rectTransform.localPosition = minValue;
		rectTransform.sizeDelta = new Vector2(maxValue.x-minValue.x, -maxValue.y+minValue.y);
	}
}