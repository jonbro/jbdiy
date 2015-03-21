using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DrawingArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
	public Camera drawingCamera;
	public GameObject brush;
	public RawImage drawingImage;
	int lastX, lastY;
	int brushSize = 6;
	Color color = Color.black;
	void Start()
	{
		lastX = -1;
		brush.SetActive (false);
	}
	private static void Swap<T>(ref T lhs, ref T rhs) { T temp; temp = lhs; lhs = rhs; rhs = temp; }

	/// <summary>
	/// The plot function delegate
	/// </summary>
	/// <param name="x">The x co-ord being plotted</param>
	/// <param name="y">The y co-ord being plotted</param>
	/// <returns>True to continue, false to stop the algorithm</returns>
	public delegate bool PlotFunction(int x, int y);

	/// <summary>
	/// Plot the line from (x0, y0) to (x1, y10
	/// </summary>
	/// <param name="x0">The start x</param>
	/// <param name="y0">The start y</param>
	/// <param name="x1">The end x</param>
	/// <param name="y1">The end y</param>
	/// <param name="plot">The plotting function (if this returns false, the algorithm stops early)</param>
	public static void Line(int x0, int y0, int x1, int y1, PlotFunction plot)
	{
		bool steep = Mathf.Abs(y1 - y0) > Mathf.Abs(x1 - x0);
		if (steep) { Swap<int>(ref x0, ref y0); Swap<int>(ref x1, ref y1); }
		if (x0 > x1) { Swap<int>(ref x0, ref x1); Swap<int>(ref y0, ref y1); }
		int dX = (x1 - x0), dY = Mathf.Abs(y1 - y0), err = (dX / 2), ystep = (y0 < y1 ? 1 : -1), y = y0;

		for (int x = x0; x <= x1; ++x)
		{
			if (!(steep ? plot(y, x) : plot(x, y))) return;
			err = err - dY;
			if (err < 0) { y += ystep;  err += dX; }
		}
	}
	public void OnPointerDown(PointerEventData eventData)
	{
//		// get position within rect
		Vector2 localPoint = new Vector2();
		RectTransform r = GetComponent<RectTransform> ();
		if(RectTransformUtility.RectangleContainsScreenPoint(r, eventData.position, eventData.enterEventCamera) ){
			RectTransformUtility.ScreenPointToLocalPointInRectangle (r, eventData.position, eventData.enterEventCamera, out localPoint);
			Texture2D tex = (Texture2D)drawingImage.texture;
			localPoint = new Vector2 (localPoint.x / r.rect.width * tex.width, localPoint.y / r.rect.height * tex.height);
			if (lastX > 0) {
				Line ((int)localPoint.x, (int)localPoint.y, lastX, lastY, (int x, int y)=>{AddPoint(x, y, brushSize); return true;});
			} else {
				AddPoint ((int)localPoint.x, (int)localPoint.y, brushSize);
			}
			lastX = (int)localPoint.x;
			lastY = (int)localPoint.y;
		}
	}
	public bool AddPoint(int x, int y, int size = 1){
		Texture2D tex = (Texture2D)drawingImage.texture;
		for(int xw=0;xw<size;xw++){
			for(int yw=0;yw<size;yw++){
				tex.SetPixel (x+xw-xw/2, y+yw-yw/2, color);
			}
		}
		tex.Apply ();	
		return true;
	}
	public void OnDrag(PointerEventData eventData)
	{

		OnPointerDown (eventData);
//		brush.transform.position = drawingCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 5));
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		lastX = -1;
	}
	public void SetBrush(int size){
		brushSize = size;
	}
	public void SetColor(Color c){
		color = c;
	}
}
