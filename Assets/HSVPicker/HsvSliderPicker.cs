using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HsvSliderPicker : MonoBehaviour, IDragHandler, IPointerDownHandler
{

    public HSVPicker picker;

	public enum SliderModes{
		Vertical,
		Horizontal
	}
	public SliderModes sm = SliderModes.Vertical;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlacePointer(PointerEventData eventData)
    {
		if (sm == SliderModes.Horizontal) {
  
			var pos = new Vector2 (picker.hsvSlider.rectTransform.position.x - eventData.position.x, eventData.position.y - picker.hsvSlider.rectTransform.position.y);

			pos.x /= picker.hsvSlider.rectTransform.rect.height * picker.hsvSlider.canvas.transform.lossyScale.y;

			//Debug.Log(eventData.position.ToString() + " " + picker.hsvSlider.rectTransform.position + " " + picker.hsvSlider.rectTransform.rect.height);
			pos.x = Mathf.Clamp (pos.x, 0, 1f);

			picker.MovePointer (pos.x);

		} else if (sm == SliderModes.Vertical) {

			var pos = new Vector2(eventData.position.x - picker.hsvSlider.rectTransform.position.x, picker.hsvSlider.rectTransform.position.y - eventData.position.y);
			
			pos.y /= picker.hsvSlider.rectTransform.rect.height * picker.hsvSlider.canvas.transform.lossyScale.y;
			
			//Debug.Log(eventData.position.ToString() + " " + picker.hsvSlider.rectTransform.position + " " + picker.hsvSlider.rectTransform.rect.height);
			pos.y = Mathf.Clamp(pos.y, 0, 1f);
			
			picker.MovePointer(pos.y);

		}
    }


    public void OnDrag(PointerEventData eventData)
    {
        PlacePointer(eventData);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlacePointer(eventData);
    }
}