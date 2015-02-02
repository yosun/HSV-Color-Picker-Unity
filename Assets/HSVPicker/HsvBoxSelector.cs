using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HsvBoxSelector : MonoBehaviour, IDragHandler, IPointerDownHandler {

    public HSVPicker picker;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void PlaceCursor(PointerEventData eventData)
    {
		Vector2 pos = new Vector2();
		pos.y= picker.hsvImage.rectTransform.rect.height * picker.hsvImage.transform.lossyScale.y - (picker.hsvImage.rectTransform.position.y - eventData.position.y);
		pos.x=picker.hsvImage.rectTransform.rect.width * picker.hsvImage.transform.lossyScale.x - (picker.hsvImage.rectTransform.position.x - eventData.position.x);
        pos.x /= picker.hsvImage.rectTransform.rect.width * picker.hsvImage.transform.lossyScale.x;
        pos.y /= picker.hsvImage.rectTransform.rect.height * picker.hsvImage.transform.lossyScale.y;
        pos.x -=.5f;
//		Debug.Log("pos is "+pos);

        pos.x = Mathf.Clamp(pos.x, 0, .9999f);  //1 is the same as 0
        pos.y = Mathf.Clamp(pos.y, 0, .9999f);

        picker.MoveCursor(pos.x, pos.y);
    }


    public void OnDrag(PointerEventData eventData)
    {
        PlaceCursor(eventData);
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlaceCursor(eventData);
    }
}
